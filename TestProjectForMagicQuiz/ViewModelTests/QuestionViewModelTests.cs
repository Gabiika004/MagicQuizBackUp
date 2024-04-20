using MagicQuizDesktop.Models;
using MagicQuizDesktop.ViewModels;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProjectForMagicQuiz.ViewModelTests
{
    public class QuestionViewModelTests
    {
        private readonly Mock<ITopicRepository> _mockTopicRepository;
        private readonly Mock<IQuestionRepository> _mockQuestionRepository;
        private readonly QuestionViewModel _viewModel;
        private readonly User _mockUser;

        public QuestionViewModelTests()
        {
            _mockTopicRepository = new Mock<ITopicRepository>();
            _mockQuestionRepository = new Mock<IQuestionRepository>();
            _mockUser = new User { AuthToken = "mock-auth-token" };

            // Configure the _mockTopicRepository as needed for the initialization
            _mockTopicRepository.Setup(repo => repo.GetByAll(It.IsAny<string>()))
                .ReturnsAsync(new ApiResponse<List<Topic>>
                {
                    Success = true,
                    Data = new List<Topic> { new Topic { Id = 1, TopicName = "Test Topic" } }
                });

            // Configure the _mockQuestionRepository as needed for the initialization
            _mockQuestionRepository.Setup(repo => repo.GetByAll(It.IsAny<string>()))
                .ReturnsAsync(new ApiResponse<List<Question>>
                {
                    Success = true,
                    Data = new List<Question> { new Question { Id = 1, TopicId = 1, QuestionText = "Test Question", Answers = new List<Answer>()} }
                });

            // Create an instance of the ViewModel with the mocked repository
            // and the mock user with a non-null AuthToken
            _viewModel = new QuestionViewModel(_mockQuestionRepository.Object,_mockTopicRepository.Object)
            {
                // Set CurrentUser for testing
                CurrentUser = _mockUser
            };
        }

        [Fact]
        public async Task GetQuestions_WhenApiCallIsSuccessful_ShouldUpdateQuestionAndFilteredQuestion()
        {
            // Act
            await _viewModel.GetQuestions();

            // Assert
            Assert.NotEmpty(_viewModel.Topics);
            Assert.NotEmpty(_viewModel.Questions);
            Assert.Equal("Test Topic", _viewModel.Topics[0].TopicName);
            Assert.Equal("Test Question", _viewModel.Questions[0].QuestionText);
            Assert.NotEmpty(_viewModel.FilteredQuestions);
        }

        [Fact]
        public async Task GetQuestions_WhenApiCallIsUnsuccessful_ShouldSetErrorMessage()
        {
            // Arrange
            _mockTopicRepository.Setup(repo => repo.GetByAll(It.IsAny<string>()))
                .ReturnsAsync(new ApiResponse<List<Topic>> { Success = false, Message = "Hiba történt." });

            // Act
            await _viewModel.GetQuestions();

            // Assert
            Assert.Equal("Hiba történt az adatok lekérésékor: Hiba történt.", _viewModel.ErrorMessage);
        }

        [Fact]
        public async Task InitializeAsync_ShouldNotThrowException_WhenSelectedQuestionIsNull()
        {
            // Arrange
            _viewModel.SelectedQuestion = null;

            // Act & Assert
            var exception = await Record.ExceptionAsync(() => _viewModel.InitializeAsync());
            Assert.Null(exception);
        }

        [Fact]
        public void PerformSearch_ShouldNotThrowException_WhenTopicsIsNull()
        {
            // Arrange
            _viewModel.Topics = null;
            _viewModel.SearchText = "some search text";

            // Act & Assert
            var exception = Record.Exception(() => _viewModel.PerformSearch());
            Assert.Null(exception);
            Assert.NotNull(_viewModel.ErrorMessage);
        }

        [Fact]
        public void GetTopicIdByName_ShouldReturnZero_WhenTopicsIsNull()
        {
            // Arrange
            _viewModel.Topics = null;

            // Act
            var result = _viewModel.GetTopicIdByName("NonExistingTopic");

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public async Task DeleteQuestion_ShouldNotThrowException_WhenSelectedQuestionIsNull()
        {
            // Arrange
            _viewModel.SelectedQuestion = null;

            // Act & Assert
            var exception = await Record.ExceptionAsync(() => _viewModel.DeleteQuestion());
            Assert.Null(exception);
            Assert.NotNull(_viewModel.ErrorMessage);
        }

        [Fact]
        public void MakeQuestionObject_ShouldReturnFalse_WhenRequiredFieldsAreEmpty()
        {
            // Arrange
            _viewModel.TopicName = ""; // szándékosan üres
            _viewModel.QuestionText = null; // szándékosan null
            _viewModel.Answer1 = new Answer { AnswerText = null }; // szándékosan null

            // Act
            var result = _viewModel.MakeQuestionObject();

            // Assert
            Assert.False(result);
            Assert.NotNull(_viewModel.ErrorMessage);
        }


    }
}
