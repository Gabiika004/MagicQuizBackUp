using MagicQuizDesktop.Commands;
using MagicQuizDesktop.Models;
using MagicQuizDesktop.Repositories;
using MagicQuizDesktop.Services;
using MagicQuizDesktop.View.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace MagicQuizDesktop.ViewModels
{
    public class TopicViewModel: ViewModelBase
    {
        /// <summary>------------------------------
        /// Constructor
        /// </summary>-----------------------------       

        public TopicViewModel(ITopicRepository topicRepository)
        {
            Topics = new List<Topic>();
            _topicRepository = topicRepository ?? throw new ArgumentNullException(nameof(topicRepository));
            CurrentUser = SessionManager.Instance.CurrentUser;
            InitializeCommands();
            _ = GetTopics();
            Initialize();
        }

        //public TopicViewModel()
        //{
        //    CurrentUser = SessionManager.Instance.CurrentUser;
        //    _topicRepository = new TopicRepository();
        //    InitializeCommands();
        //    _ = GetTopics();
        //    Initialize();

        //}

        /// <summary>------------------------------
        /// Private Fields
        /// </summary>----------------------------

        private User _currentUser;
        private List<Topic> _topics;
        private string _errorMessage = string.Empty;
        private string _message = string.Empty;
        private string _searchText;
        private string _topicName;

        /// <summary>----------------------------
        ///  Interfaces
        /// </summary>---------------------------

        private readonly ITopicRepository _topicRepository;

        /// <summary>----------------------------
        /// Public Fields
        /// </summary>---------------------------

        public User CurrentUser
        {
            get { return _currentUser; }
            set
            {
                if (_currentUser != value)
                {
                    _currentUser = value;
                    OnPropertyChanged(nameof(CurrentUser));
                }
            }
        }
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));

                if (string.IsNullOrEmpty(SearchText))
                {
                    ResetToDefault();
                }
            }
        }
        public List<Topic> Topics
        {
            get => _topics;
            set
            {
                _topics = value;
                OnPropertyChanged(nameof(Topics));
            }
        }

        public Topic SelectedTopic
        {
            get => _selectedTopic;
            set
            {
                _selectedTopic = value;
                OnPropertyChanged(nameof(SelectedTopic));
            }
        }
        private Topic _selectedTopic;
        private ObservableCollection<Topic> _filteredTopics; 

        /// <summary>
        /// Every error get by API request to
        /// inform the user
        /// </summary>
        public string? ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                if (_errorMessage != value)
                {
                    _errorMessage = value;
                    _message = string.Empty;
                    OnPropertyChanged(nameof(ErrorMessage));
                    OnPropertyChanged(nameof(Message));
                }
            }
        }

        /// <summary>
        /// Inform the user if the request was succesful
        /// </summary>
        public string? Message
        {
            get { return _message; }
            set
            {
                if (_message != value)
                {
                    _message = value;
                    _errorMessage = string.Empty;
                    OnPropertyChanged(nameof(Message));
                    OnPropertyChanged(nameof(ErrorMessage));
                }
            }
        }
        public string? TopicName
        {
            get { return _topicName; }
            set
            {
                if (_topicName != value)
                {
                    _topicName = value;
                    OnPropertyChanged(nameof(TopicName));
                }
            }
        }
        public ObservableCollection<Topic> FilteredTopics
        {
            get => _filteredTopics;
            set
            {
                _filteredTopics = value;
                OnPropertyChanged(nameof(FilteredTopics));
            }
        }

        /// <summary>--------------------------
        /// Commands
        /// </summary>-------------------------
     
        public ICommand SearchCommand { get; private set; }
        public ICommand UpdateDatasCommand { get; private set; }
        public ICommand OpenTopicWindowCommand { get; private set;}
        public ICommand OpenNewTopicWindowCommand { get; private set; }
        public ICommand SubmitTopicCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }


        /// <summary>------------------------------
        /// Methods
        /// </summary>>---------------------




        /// <summary>
        /// Get all topics --> API request
        /// </summary>
        public async Task GetTopics()
        {

            try
            {
                var topicResponse = await _topicRepository.GetByAll(CurrentUser.AuthToken);

                if (topicResponse.Success)
                {
                    Topics = topicResponse.Data; // Itt a Topics sosem lesz null, mivel új listát kapsz.
                    FilteredTopics = new ObservableCollection<Topic>(Topics);
                }
                else
                {
                    FilteredTopics = new ObservableCollection<Topic>(); // Ha a válasz sikertelen, üres ObservableCollection-t hozunk létre.
                    ErrorMessage = $"Hiba történt az adatok lekérésékor: {topicResponse.Message}";
                }
            }
            catch (Exception ex)
            {
                FilteredTopics = new ObservableCollection<Topic>(); // Kivétel esetén is üres ObservableCollection-t hozunk létre.
                ErrorMessage = $"Hiba történt az adatok frissítése közben: {ex.Message}";
            }
        }


        /// <summary>
        /// Reset the list used by the view to
        /// update the user interface
        /// </summary>
        private void ResetToDefault()
        {
            if (Topics != null)
            {
                _ = GetTopics();
            }
        }

        /// <summary>
        /// Get the topic with a specific topic name
        /// </summary>
        private void PerformSearch()
        {
            if (string.IsNullOrEmpty(SearchText))
            {
                ErrorMessage = "Keresési szöveg nem lehet üres.";
                return; 
            }

            if (Topics == null)
            {
                ResetToDefault();
                return;
            }

            var relevantTopic = Topics
                .FirstOrDefault(t => t.TopicName != null && t.TopicName.Equals(SearchText, StringComparison.OrdinalIgnoreCase));

            if (relevantTopic == null)
            {
                ErrorMessage = "Nincs ilyen téma";
                FilteredTopics.Clear();
                return;
            }

            FilteredTopics.Clear();
            FilteredTopics.Add(relevantTopic);
        }

        /// <summary>
        /// Open a new TopicWindow to update selected topic
        /// </summary>
        /// <param name="param">Selected topic as a Topic object</param>
        private void PerformOpenTopicWindow(object param)
        {
            if (param is Topic topic)
            {
                SelectedTopic= topic;  
                Initialize();
                TopicWindow window = new () { DataContext = this };
                window.Closed += (sender, args) => {
                    ErrorMessage = string.Empty;
                };
                window.ShowDialog();
            }
            else
            {
                ErrorMessage = "Hoppá! Nem megfelelő objektum";
            }
        }

        /// <summary>
        /// Open a new TopicWindow to add a new Question
        /// </summary>
        private void PerformOpenNewTopicWindow()
        {
            TopicWindow window = new();
            window.Closed += (sender, args) => {
                ErrorMessage = string.Empty;
                Initialize();            
            };
            window.ShowDialog();
        }

        /// <summary>
        /// Initialize -> Select the desired data
        /// Depend on what we use TopicPage/TopicWindow(new/update)
        /// </summary>
        private void Initialize()
        {
            if (SelectedTopic != null && SelectedTopic != new Topic())
            {
                TopicName = SelectedTopic.TopicName;
            }
            else
            {
                SelectedTopic = new Topic();
                TopicName = string.Empty;
            }
        }

        /// <summary>
        /// Initialize Commands
        /// </summary>
        private void InitializeCommands()
        {
            UpdateDatasCommand = new AsyncRelayCommand(async _ => await GetTopics());
            OpenTopicWindowCommand = new RelayCommand(PerformOpenTopicWindow);
            OpenNewTopicWindowCommand = new RelayCommand(_ => PerformOpenNewTopicWindow());
            SubmitTopicCommand = new AsyncRelayCommand(async _ => await TopicCommand());
            DeleteCommand = new AsyncRelayCommand(async _ => await DeleteTopic(), (o) => CanExecuteCommand(o));
            SearchCommand = new RelayCommand(_ => PerformSearch());
        }

        /// <summary>
        /// Async Task for add or update Topic
        /// Depends on wether we selected a topic or not
        /// </summary>
        /// <returns>"Step out" of the Task is there is something
        /// wrong with the validation or the object</returns>
        public async Task TopicCommand()
        {
            if (SelectedTopic != null && SelectedTopic != new Topic() && SelectedTopic.Id > 0)
            {
                if (!MakeTopicObject())
                {
                    return;
                }
                try
                {
                    ApiResponse<Topic> topicResponse;
                    topicResponse = await _topicRepository.UpdateAsync(SelectedTopic, CurrentUser.AuthToken);

                    if (topicResponse.Success)
                    {
                        Message = "Téma sikeresen frissítve!";
                        SelectedTopic = new Topic();
                        ResetToDefault();
                    }
                    else
                    {
                        ErrorMessage = $"Sikertelen API kérés: {topicResponse.Message}";
                    }
                }
                catch (Exception ex)
                {
                    ErrorMessage = $"A téma feldolgozása nem sikerült: {ex.Message}";
                }
            }
            else
            {
                if (!MakeTopicObject())
                {
                    return;
                }

                try
                { 
                    ApiResponse<Topic> topicResponse;
                    topicResponse = await _topicRepository.AddAsync(SelectedTopic, CurrentUser.AuthToken);
                    if (topicResponse.Success)
                    {
                        Message = "Téma sikeresen felvéve!";
                        SelectedTopic = new Topic();
                        ResetToDefault();
                    }
                    else
                    {
                        ErrorMessage = $"Sikertelen API kérés: {topicResponse.Message}";
                        SelectedTopic = new Topic();
                    }
                }
                catch (Exception ex)
                {
                    ErrorMessage = $"A téma feldolgozása nem sikerült: {ex.Message}";
                    SelectedTopic = new Topic();
                }

            }

        }

        /// <summary>
        /// Make a Topic Object from the TopicName field for async requests
        /// </summary>
        /// <returns>False if a the TopicName is not given</returns>
        private bool MakeTopicObject()
        {
            if (string.IsNullOrEmpty(TopicName))
            {
                ErrorMessage = "A téma megnevezése kötelező!";
                return false;
            }
               
            int topicId = GetTopicIdByName(TopicName);

            if (topicId == 0)
            {
                if (SelectedTopic != null)
                {
                    SelectedTopic.TopicName = TopicName;
                }
                else
                {
                    SelectedTopic = new Topic()
                    {
                        TopicName = TopicName
                    };
                }
            }
            else
            {
                SelectedTopic.TopicName = TopicName;
                SelectedTopic.Id = topicId;
            }
    
            return true;
        }
        private int GetTopicIdByName(string topicName)
        {
            if (Topics == null)
                return 0;

            var topicId = Topics
                .Where(t => t.TopicName != null && t.TopicName.Contains(topicName, StringComparison.OrdinalIgnoreCase))
                .Select(t => t.Id)
                .FirstOrDefault();

            return topicId; 
        }

        /// <summary>
        /// Async Task for deleting a Task
        /// </summary>
        /// <returns></returns>
        public async Task DeleteTopic()
        {
            if (string.IsNullOrEmpty(TopicName))
            {
                ErrorMessage = "A téma megnevezése kötelező!";
                return;
            }
            int topicId = GetTopicIdByName(TopicName);

            if (topicId == 0)
            {
                ErrorMessage = "Nincs ilyen téma!";
                return;
            }
            try
            {
                var response = await _topicRepository.DeleteAsync(topicId, CurrentUser.AuthToken);
                if (response.Success)
                {
                    Message = "A téma sikeresen törölve.";
                    SelectedTopic = new Topic();
                    ResetToDefault();
                }
                else
                {
                    // Ha a törlés sikertelen, megjelenítjük a hibaüzenetet a válaszból
                    ErrorMessage = $"Hiba a törlés során: {response.Message}";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Hiba az alkalmazásban: {ex.Message}";
            }

        }

        /// <summary>
        /// Can Excetue method for allowing the button press
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private bool CanExecuteCommand(object obj)
        {
            return !string.IsNullOrWhiteSpace(TopicName) && (SelectedTopic.Id > 0);
        }
    }
}
