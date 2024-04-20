using MagicQuizDesktop.Commands;
using MagicQuizDesktop.Models;
using MagicQuizDesktop.Repositories;
using MagicQuizDesktop.Services;
using MagicQuizDesktop.View.Pages;
using MagicQuizDesktop.View.Windows;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace MagicQuizDesktop.ViewModels
{
    public class QuestionViewModel : ViewModelBase
    {


        //Private fields

        //For the user
        private User? _currentUser;

        //For the questions
        private Question? _selectedQuestion;
        private string? _searchText;
        private string? _questionText;
        private Answer? _answer1;
        private Answer? _answer2;
        private Answer? _answer3;
        private Answer? _answer4;
        private string? _topicName;
        private int? _correctAnswerNumber;
        private string _messageColor;

        public string MessageColor
        {
            get { return _messageColor; }
            set
            {
                _messageColor = value;
                OnPropertyChanged(nameof(MessageColor));
            }
        }

        //Lists
        private List<Question>? _questions;
        private ObservableCollection<Question>? _filteredQuestions;
        private List<Topic>? _topics;
        private List<Answer>? _answers;

        //For errors
        string? _errorMessage = string.Empty;
        string? _message = string.Empty;


        //Interfaces
        private readonly ITopicRepository _topicRepository;
        private readonly IQuestionRepository _questionRepository;


        //Public fields

        //For the user
        public User? CurrentUser
        {
            get { return _currentUser; }
            set
            {
                if (_currentUser != value)
                {
                    _currentUser = value;
                    OnPropertyChanged(nameof(CurrentUser));
                    // Itt frissítheted a userId-t is, ha szükséges
                }
            }
        }

        //For the questions
        public Question? SelectedQuestion
        {
            get { return _selectedQuestion; }
            set
            {
                if (_selectedQuestion != value)
                {
                    _selectedQuestion = value;
                    OnPropertyChanged(nameof(SelectedQuestion));
                    // Itt frissítheted a userId-t is, ha szükséges
                }
            }
        }
        public string? SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
                // Call the method if the text is empty
                if (string.IsNullOrEmpty(SearchText))
                {
                    ResetToDefault();
                }
            }
        }
        public string? QuestionText
        {
            get { return _questionText; }
            set
            {
                _questionText = value;
                OnPropertyChanged(nameof(QuestionText));

            }
        }
        public int? CorrectAnswerNumber
        {
            get { return _correctAnswerNumber; }
            set
            {
                _correctAnswerNumber = value;
                OnPropertyChanged(nameof(CorrectAnswerNumber));

            }
        }

        private void ResetToDefault()
        {
            FilteredQuestions = new ObservableCollection<Question>(Questions);
        }
        public List<int> AnswerNumbers { get; }
        public Answer? Answer1
        {
            get { return _answer1; }
            set
            {
                if (_answer1 != value)
                {
                    _answer1 = value;
                    OnPropertyChanged(nameof(Answer1));
                }
            }
        }
        public Answer? Answer2
        {
            get { return _answer2; }
            set
            {
                if (_answer2 != value)
                {
                    _answer2 = value;
                    OnPropertyChanged(nameof(Answer2));
                }
            }
        }
        public Answer? Answer3
        {
            get { return _answer3; }
            set
            {
                if (_answer3 != value)
                {
                    _answer3 = value;
                    OnPropertyChanged(nameof(Answer3));
                }
            }
        }
        public Answer? Answer4
        {
            get { return _answer4; }
            set
            {
                if (_answer4 != value)
                {
                    _answer4 = value;
                    OnPropertyChanged(nameof(Answer4));
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

        //Lists
        public List<Question>? Questions
        {
            get => _questions;
            set
            {
                _questions = value;
                OnPropertyChanged(nameof(Questions));
            }
        }
        public ObservableCollection<Question>? FilteredQuestions
        {
            get => _filteredQuestions;
            set
            {
                _filteredQuestions = value;
                OnPropertyChanged(nameof(FilteredQuestions));
            }
        }
        public List<Topic>? Topics
        {
            get => _topics;
            set
            {
                _topics = value;
                OnPropertyChanged(nameof(Topics));
            }
        }
        public List<Answer>? Answers
        {
            get => _answers;
            set
            {
                _answers = value;
                OnPropertyChanged(nameof(Answers));
            }
        }

        //For errors           
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


        //Commands

        public ICommand SearchCommand { get; private set; }
        public ICommand UpdateDatasCommand { get; private set; }
        public ICommand OpenQuestionWindowCommand { get; private set; }
        public ICommand OpenNewQuestionWindowCommand { get; private set; }
        public ICommand SubmitQuestionCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }


        //Constructors


        /// <summary>
        /// Constructor for QuestionPage and QuestionWindow to add new question
        /// </summary>
        /// <param name="user"></param>
        /// <param name="question"></param>
        /// 

        public QuestionViewModel(IQuestionRepository questionRepository, ITopicRepository topicRepository)
        {
            CurrentUser = SessionManager.Instance.CurrentUser;
            _topicRepository = topicRepository;
            _questionRepository = questionRepository;
            AnswerNumbers = new List<int> { 1, 2, 3, 4 };
            InitializeCommands();
            _ = InitializeAsync();
        }


        public void InitializeCommands()
        {
            UpdateDatasCommand = new AsyncRelayCommand(async _ => await GetQuestions());
            SubmitQuestionCommand = new AsyncRelayCommand(async _ => await QuestionCommand());
            OpenQuestionWindowCommand = new RelayCommand(PerformOpenQuestionWindow);
            OpenNewQuestionWindowCommand = new RelayCommand(_ => PerformOpenNewQuestionWindow());
            DeleteCommand = new AsyncRelayCommand(async _ => await DeleteQuestion());
            SearchCommand = new RelayCommand(_ => PerformSearch());
        }


        public async Task InitializeAsync()
        {
            if (SelectedQuestion != null)
            {
                SetFieldsFromSelectedQueston(SelectedQuestion);
            }
            else
            {
                await GetQuestions();
                SetAnswers();
            }
        }

        //Actions


        //For open windows

        /// <summary>
        /// Open a new QuestionWindow to update data/datas
        /// </summary>
        /// <param name="param"></param>
        public void PerformOpenQuestionWindow(object param)
        {
            if (param is Question question)
            {
                SelectedQuestion = question;  // Beállítja az aktuális kérdést
                _ = InitializeAsync();
                QuestionWindow window = new QuestionWindow { DataContext = this };
                window.Closed += (sender, args) =>
                {
                    ErrorMessage = string.Empty;  // Nullázás csak az ablak bezárása után
                };
                window.ShowDialog();
            }
            else
            {
                ErrorMessage = "Hoppá! Nem megfelelő objektum";
            }
        }

        /// <summary>
        /// Open a new QuestionWindow to add a new Question
        /// </summary>
        public void PerformOpenNewQuestionWindow()
        {
            QuestionWindow window = new();
            window.Closed += (sender, args) =>
            {
                ErrorMessage = string.Empty;
                ResetCurrentQuestion();
            };
            window.ShowDialog();
        }


        //For update data

        /// <summary>
        /// Get all Questons with Answers and Topics
        /// </summary>
        /// <returns></returns>
        public async Task GetQuestions()
        {
            try
            {
                var topicResponse = await _topicRepository.GetByAll(CurrentUser.AuthToken);
                var questionResponse = await _questionRepository.GetByAll(CurrentUser.AuthToken);

                if (topicResponse.Success && questionResponse.Success)
                {
                    Topics = topicResponse.Data;
                    Questions = questionResponse.Data;
                    FilteredQuestions = new ObservableCollection<Question>(Questions);
                }
                else
                {
                    ErrorMessage = $"Hiba történt az adatok lekérésekor: {topicResponse.Message} {questionResponse.Message}";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Hiba történt az adatok frissítése közben: {ex.Message}";
            }
        }


        public void SetFieldsFromSelectedQueston(Question question)
        {
            TopicName = GetTopicNameById(question.TopicId);
            for (int i = 0; i < question.Answers.Count; i++)
            {
                if (question.Answers[i].IsCorrect)
                {
                    CorrectAnswerNumber = i + 1;
                }
                switch (i)
                {
                    case 0:
                        Answer1 = new Answer
                        {
                            AnswerText = question.Answers[0].AnswerText,
                            Id = question.Answers[0].Id
                        };
                        break;
                    case 1:
                        Answer2 = new Answer
                        {
                            AnswerText = question.Answers[1].AnswerText,
                            Id = question.Answers[1].Id
                        };
                        break;
                    case 2:
                        Answer3 = new Answer
                        {
                            AnswerText = question.Answers[2].AnswerText,
                            Id = question.Answers[2].Id
                        };
                        break;
                    case 3:
                        Answer4 = new Answer
                        {
                            AnswerText = question.Answers[3].AnswerText,
                            Id = question.Answers[3].Id
                        };
                        break;
                }
            }
            QuestionText = SelectedQuestion.QuestionText;
        }

        /// <summary>
        /// Update Question/Answers
        /// </summary>
        /// <param name="question"></param>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
        /// 

        public async Task QuestionCommand()
        {
            if (SelectedQuestion != null)
            {
                if (!MakeQuestionObject())
                {
                    return;
                }
                try
                {
                    ApiResponse<Question> questionResponse;
                    questionResponse = await _questionRepository.UpdateAsync(SelectedQuestion, CurrentUser.AuthToken);

                    if (questionResponse.Success)
                    {
                        Message = "Adatok sikeresen frissítve!";
                        SelectedQuestion = new Question();
                    }
                    else
                    {
                        ErrorMessage = $"Sikertelen API kérés: {questionResponse.Message}";
                        SelectedQuestion = new Question();
                    }
                }
                catch (Exception ex)
                {
                    ErrorMessage = $"A kérdés feldolgozása nem sikerült: {ex.Message}";
                    SelectedQuestion = new Question();
                }
            }
            else
            {
                if (!MakeQuestionObject())
                {
                    return;
                }

                try
                {
                    ApiResponse<Question> questionResponse;
                    questionResponse = await _questionRepository.AddAsync(SelectedQuestion, CurrentUser.AuthToken);
                    if (questionResponse.Success)
                    {
                        Message = "Adatok sikeresen frissítve!";
                        SelectedQuestion = new Question();
                    }
                    else
                    {
                        ErrorMessage = $"Sikertelen API kérés: {questionResponse.Message}";
                        SelectedQuestion = new Question();
                    }
                }
                catch (Exception ex)
                {
                    ErrorMessage = $"A kérdés feldolgozása nem sikerült: {ex.Message}";
                    SelectedQuestion = new Question();
                }

            }
        }


        //For searching data

        /// <summary>
        /// Get all questions with a specific topic name
        /// </summary>
        public void PerformSearch()
        {
            // Ellenőrizzük, hogy a SearchText nem üres vagy null-e
            if (string.IsNullOrEmpty(SearchText))
            {
                ErrorMessage = "Keresési szöveg nem lehet üres.";
                return; // Kilépés a metódusból, ha nincs értelmezhető keresési szöveg
            }

            // Biztosítjuk, hogy a Topics nem null
            if (Topics == null)
            {
                ResetToDefault();
                return;
            }

            // Keresési logika...
            var relevantTopicIds = Topics
                .Where(t => t.TopicName != null && t.TopicName.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                .Select(t => t.Id)
                .ToList();
            var filtered = new ObservableCollection<Question>(Questions.Where(q => relevantTopicIds.Contains(q.TopicId)).ToList());

            FilteredQuestions.Clear();
            FilteredQuestions = filtered; // ObservableCollection, nincs szükség egyesével hozzáadni.

            // Nincs szükség minden egyes hozzáadás után OnPropertyChanged-t hívni.
            if (!FilteredQuestions.Any())
            {
                ErrorMessage = "Nincs ilyen Téma";
            }
        }

        /// <summary>
        /// Get an id of a topic by its name
        /// </summary>
        /// <param name="topicName"></param>
        /// <returns></returns>
        public int GetTopicIdByName(string topicName)
        {
            if (topicName == null)
                return 0;
            // Ellenőrizzük, hogy a 'Topics' nem null-e
            if (Topics == null)
                return 0; // Vagy dobhatunk egy kivételt, ha ez jobban illik az alkalmazás logikájához

            var topicId = Topics
                .Where(t => t.TopicName != null && t.TopicName.Contains(topicName, StringComparison.OrdinalIgnoreCase))
                .Select(t => t.Id)
                .FirstOrDefault();  // Visszaadja az első talált Id-t, vagy 0-t, ha nincs találat.

            return topicId;  // Visszaadja az Id-t, vagy 0-t, ha nincs találat.
        }
        public string GetTopicNameById(int selectedTopicId)
        {
            // Ellenőrizzük, hogy a 'Topics' lista nem null-e és van-e benne elem
            if (Topics == null || !Topics.Any())
            {
                ErrorMessage = "A témák listája üres vagy nem inicializált!";
                return "";
            }

            // Keresés a listában az adott ID-val rendelkező témára
            var topicName = Topics
                .Where(t => t.Id == selectedTopicId)
                .Select(t => t.TopicName)
                .FirstOrDefault();

            // Ellenőrizzük, hogy sikerült-e érvényes témanevet találni
            if (string.IsNullOrEmpty(topicName))
            {
                ErrorMessage = "Nem található téma az adott azonosítóval.";
                return "";
            }

            return topicName;  // Sikeres lekérdezés esetén visszaadjuk a témanevet
        }

        //New Data

        public void SetCorrectAnswer(Question question, int correctAnswerNumber)
        {

            foreach (var answer in question.Answers)
            {
                answer.IsCorrect = false;
            }

            switch (correctAnswerNumber)
            {
                case 1:
                    question.Answers[0].IsCorrect = true;
                    break;
                case 2:
                    question.Answers[1].IsCorrect = true;
                    break;
                case 3:
                    question.Answers[2].IsCorrect = true;
                    break;
                case 4:
                    question.Answers[3].IsCorrect = true;
                    break;
                default:
                    ErrorMessage = "Invalid CorrectAnswerNumber value. It must be between 1 and 4.";
                    break;
            }
        }

        public bool MakeQuestionObject()
        {
            if (!ValidateQuestionFields())
            {
                return false;
            }

            int topicId = GetTopicIdByName(TopicName);
            if (topicId == 0)
            {
                ErrorMessage = $"Nincs ilyen téma, vegye fel a következőt: {TopicName}";
                return false;
            }

            SetAnswersList();

            if (SelectedQuestion == null)
            {
                SelectedQuestion = new Question();
            }
            SelectedQuestion.TopicId = topicId;
            SelectedQuestion.Answers = _answers;

            if (CorrectAnswerNumber == 0 || CorrectAnswerNumber == null)
            {
                ErrorMessage = "A helyes válasz száma érvénytelen! (1-4)";
                return false;
            }
            SetCorrectAnswer(SelectedQuestion, (int)CorrectAnswerNumber);
            SelectedQuestion.QuestionText = QuestionText;
            return true;
        }

        public bool ValidateQuestionFields()
        {

            if (string.IsNullOrEmpty(TopicName))
            {
                ErrorMessage = "A téma megadása kötelező.";
                return false;
            }

            SetAnswers();
            if (string.IsNullOrEmpty(Answer1.AnswerText) || string.IsNullOrEmpty(Answer2.AnswerText) ||
                string.IsNullOrEmpty(Answer3.AnswerText) || string.IsNullOrEmpty(Answer4.AnswerText))
            {
                ErrorMessage = "Minden válaszlehetőség kitöltése kötelező.";
                return false;
            }

            if (string.IsNullOrEmpty(QuestionText))
            {
                ErrorMessage = "A kérdés szövege nem lehet üres.";
                return false;
            }

            if (CorrectAnswerNumber == 0 || CorrectAnswerNumber > 4)
            {
                ErrorMessage = "A helyes válasz száma érvénytelen!(0-4)";
                return false;
            }

            return true;
        }

        public void SetAnswersList()
        {
            _answers = new()
                    {
                        Answer1 ?? new Answer(),  // Ha Answer1 null, akkor egy új Answer példányt ad hozzá
                        Answer2 ?? new Answer(),  // Ha Answer2 null, akkor egy új Answer példányt ad hozzá
                        Answer3 ?? new Answer(),  // Ha Answer3 null, akkor egy új Answer példányt ad hozzá
                        Answer4 ?? new Answer()   // Ha Answer4 null, akkor egy új Answer példányt ad hozzá
                    };
        }
        public void SetAnswers()
        {
            // Ellenőrizzük, hogy az Answer1 null-e, ha igen, akkor új példányt hozunk létre.
            if (Answer1 == null)
            {
                Answer1 = new Answer();
            }

            // Ugyanez az eljárás az Answer2, Answer3 és Answer4 változókkal.
            if (Answer2 == null)
            {
                Answer2 = new Answer();
            }

            if (Answer3 == null)
            {
                Answer3 = new Answer();
            }

            if (Answer4 == null)
            {
                Answer4 = new Answer();
            }
        }


        public void ResetCurrentQuestion()
        {
            SelectedQuestion = null;
        }


        public async Task DeleteQuestion()
        {
            try
            {
                var response = await _questionRepository.DeleteAsync(SelectedQuestion.Id, CurrentUser.AuthToken);
                if (response.Success)
                {
                    Message = "A kérdés sikeresen törölve.";
                    // Itt törölheted a kérdést a helyi listából is, ha szükséges
                    Questions.Remove(SelectedQuestion);
                    SelectedQuestion = null;  // Töröljük a kiválasztott kérdést, ha a törlés megtörtént
                }
                else
                {
                    // Ha a törlés sikertelen, megjelenítjük a hibaüzenetet a válaszból
                    ErrorMessage = $"Hiba a törlés során: {response.Message}";
                }
            }
            catch (Exception ex)
            {
                // Általános hibakezelés
                ErrorMessage = $"Hiba az alkalmazásban: {ex.Message}";
            }
        }

    }
}
