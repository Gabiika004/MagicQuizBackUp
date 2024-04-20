using Xunit;
using MagicQuizDesktop.Models;
using MagicQuizDesktop.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using MagicQuizDesktop.ViewModels;



namespace TestProjectForMagicQuiz.ViewModelTests
{
    public class TopicViewModelTests
    {
        private readonly Mock<ITopicRepository> _mockRepository;
        private readonly TopicViewModel _viewModel;
        private readonly User _mockUser;

        public TopicViewModelTests()
        {
            _mockRepository = new Mock<ITopicRepository>();
            _mockUser = new User { AuthToken = "mock-auth-token" };

            // Configure the mock as needed for the initialization
            _mockRepository.Setup(repo => repo.GetByAll(It.IsAny<string>()))
                .ReturnsAsync(new ApiResponse<List<Topic>>
                {
                    Success = true,
                    Data = new List<Topic> { new Topic { Id = 1, TopicName = "Test Topic" } }
                });

            // Create an instance of the ViewModel with the mocked repository
            // and the mock user with a non-null AuthToken
            _viewModel = new TopicViewModel(_mockRepository.Object)
            {
                // Set CurrentUser for testing
                CurrentUser = _mockUser
            };
        }

        [Fact]
        public async Task GetTopics_WhenApiCallIsSuccessful_ShouldUpdateTopicsAndFilteredTopics()
        {
            // Act
            await _viewModel.GetTopics();

            // Assert
            Assert.NotEmpty(_viewModel.Topics);
            Assert.Equal("Test Topic", _viewModel.Topics[0].TopicName);
            Assert.NotEmpty(_viewModel.FilteredTopics);
            Assert.All(_viewModel.FilteredTopics, topic => Assert.Contains(topic, _viewModel.Topics));
        }

        [Fact]
        public async Task GetTopics_WhenApiCallIsUnsuccessful_ShouldSetErrorMessage()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetByAll(It.IsAny<string>()))
                .ReturnsAsync(new ApiResponse<List<Topic>> { Success = false, Message = "Hiba történt." });

            // Act
            await _viewModel.GetTopics();

            // Assert
            Assert.Equal("Hiba történt az adatok lekérésékor: Hiba történt.", _viewModel.ErrorMessage);
        }

        [Fact]
        public async Task DeleteTopic_WhenCalledWithValidTopicId_ShouldRemoveTopicFromTopics()
        {
            // Arrange
            var mockData = new List<Topic> { new Topic { Id = 1, TopicName = "Test Topic" } };
            _viewModel.TopicName = mockData[0].TopicName;

            _mockRepository.Setup(repo => repo.GetByAll(It.IsAny<string>()))
                .ReturnsAsync(new ApiResponse<List<Topic>> { Success = true, Data = mockData });

            await _viewModel.GetTopics();


            _viewModel.SelectedTopic = _viewModel.Topics.First();

            _mockRepository.Setup(repo => repo.DeleteAsync(_viewModel.SelectedTopic.Id, It.IsAny<string>()))
                .ReturnsAsync(new ApiResponseWithNoData { StatusCode = System.Net.HttpStatusCode.OK });


            _mockRepository.Setup(repo => repo.GetByAll(It.IsAny<string>()))
                .ReturnsAsync(new ApiResponse<List<Topic>> { Success = true, Data = new List<Topic>() });

            // Act
            await _viewModel.DeleteTopic();

            await _viewModel.GetTopics();

            // Assert
            Assert.DoesNotContain(_viewModel.SelectedTopic, _viewModel.Topics);
            Assert.Empty(_viewModel.ErrorMessage);
        }

        [Fact]
        public async Task DeleteTopic_WhenDeletionFails_ShouldNotRemoveTopicAndShouldSetErrorMessage()
        {
            // Arrange
            var mockData = new List<Topic> { new Topic { Id = 1, TopicName = "Test Topic" } };
            _viewModel.TopicName = mockData[0].TopicName;
            var ErrorMessage = "A témát nem lehet törölni, mert hozzá tartoznak kérdések.";
            var ExpectedErrorMessage = "Hiba a törlés során: A témát nem lehet törölni, mert hozzá tartoznak kérdések.";

            _mockRepository.Setup(repo => repo.GetByAll(It.IsAny<string>()))
                .ReturnsAsync(new ApiResponse<List<Topic>> { Success = true, Data = mockData });

            await _viewModel.GetTopics();

            _viewModel.SelectedTopic = _viewModel.Topics.First();

            _mockRepository.Setup(repo => repo.DeleteAsync(_viewModel.SelectedTopic.Id, It.IsAny<string>()))
                .ReturnsAsync(new ApiResponseWithNoData
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = ErrorMessage
                });

            // Act
            // Töröljük a kiválasztott témát.
            await _viewModel.DeleteTopic();

            // Assert
            // Ellenőrizzük, hogy a törölni kívánt téma még mindig megtalálható a listában.
            Assert.Contains(_viewModel.SelectedTopic, _viewModel.Topics);

            // Ellenőrizzük, hogy a helyes hibaüzenet jelenik-e meg.
            Assert.Equal(ExpectedErrorMessage, _viewModel.ErrorMessage);
        }

        [Fact]
        public async Task TopicCommand_WhenUpdateIsSuccessful_ShouldSetSuccessMessage()
        {
            // Arrange
            var topicToUpdate = new Topic { Id = 1, TopicName = "Updated Topic" };
            _viewModel.TopicName = topicToUpdate.TopicName;
            _viewModel.SelectedTopic = topicToUpdate;

            _mockRepository.Setup(repo => repo.UpdateAsync(topicToUpdate, It.IsAny<string>()))
                .ReturnsAsync(new ApiResponse<Topic> { Success = true, Data = topicToUpdate });

            // Act
            await _viewModel.TopicCommand();

            // Assert
            Assert.Equal("Téma sikeresen frissítve!", _viewModel.Message);
            Assert.Null(_viewModel.SelectedTopic.TopicName);
        }

        [Fact]
        public async Task TopicCommand_WhenUpdateFails_ShouldSetErrorMessage()
        {
            // Arrange
            var topicToUpdate = new Topic { Id = 1, TopicName = "Original Topic" };
            _viewModel.TopicName = topicToUpdate.TopicName;
            _viewModel.SelectedTopic = topicToUpdate;

            var errorMessage = "Frissítés sikertelen. Próbálja újra.";
            var expectedErrorMessage = "Sikertelen API kérés: Frissítés sikertelen. Próbálja újra.";
            _mockRepository.Setup(repo => repo.UpdateAsync(topicToUpdate, It.IsAny<string>()))
                .ReturnsAsync(new ApiResponse<Topic> { Success = false, Message = errorMessage });

            // Act
            await _viewModel.TopicCommand();

            // Assert
            Assert.Equal(expectedErrorMessage, _viewModel.ErrorMessage);
            Assert.Equal("Original Topic", _viewModel.SelectedTopic.TopicName);
        }



    }

}
