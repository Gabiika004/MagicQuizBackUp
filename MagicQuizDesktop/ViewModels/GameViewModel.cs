using MagicQuizDesktop.Commands;
using MagicQuizDesktop.Models;
using MagicQuizDesktop.Repositories;
using MagicQuizDesktop.Services;
using MagicQuizDesktop.View.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using static System.Formats.Asn1.AsnWriter;

namespace MagicQuizDesktop.ViewModels
{
    public class GameViewModel : ViewModelBase
    { 

        private User _currentUser;
        Random rand = new Random();
        private string _errorMessage;
        private string _message;


        private DispatcherTimer questionTimer;
        private int _timeLeft;
        public string Clock
        {
            get { return _timeLeft.ToString(); }
        }

        private bool gameRunning = false;


        public User CurrentUser
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

        public string Message
        {
            get => _message;
            set
            {
                if (_message != value)
                {
                    _message = value;
                    OnPropertyChanged(nameof(Message));
                    if (!string.IsNullOrEmpty(value))
                    {
                        // Csak akkor állítjuk az ErrorMessage-t, ha a Message nem üres
                        ErrorMessage = string.Empty;
                    }
                }
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                if (_errorMessage != value)
                {
                    _errorMessage = value;
                    OnPropertyChanged(nameof(ErrorMessage));
                    if (!string.IsNullOrEmpty(value))
                    {
                        // Csak akkor állítjuk a Message-t, ha az ErrorMessage nem üres
                        Message = string.Empty;
                    }
                }
            }
        }


        private int _score;
        public int Score
        {
            get { return _score; }
            set
            {
                _score = value;
                OnPropertyChanged(nameof(Score));
            }
        }

        private int actualScore;
        private string _topicName;
        public string TopicName
        {
            get { return _topicName; }
            set
            {
                _topicName = value;
                OnPropertyChanged(nameof(TopicName));
            }
        }

        private string _questionText;
        public string QuestionText
        {
            get { return _questionText; }
            set
            {
                _questionText = value;
                OnPropertyChanged(nameof(QuestionText));
            }
        }

        private Answer _answer1;
        public Answer Answer1
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

        private Answer _answer2;
        public Answer Answer2
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

        private Answer _answer3;
        public Answer Answer3
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

        private Answer _answer4;
        public Answer Answer4
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

        private Brush _mouseOverBackground = new SolidColorBrush(Colors.DarkBlue); // Vagy bármilyen más alapértelmezett érték.
        public Brush MouseOverBackground
        {
            get { return _mouseOverBackground; }
            set
            {
                if (_mouseOverBackground != value)
                {
                    _mouseOverBackground = value;
                    OnPropertyChanged(nameof(MouseOverBackground));
                }
            }
        }

        private bool _phoneFriendHelpStatus;
        public bool PhoneFriendHelpStatus
        {
            get { return _phoneFriendHelpStatus; }
            set
            {
                _phoneFriendHelpStatus = value;
                OnPropertyChanged(nameof(PhoneFriendHelpStatus));
            }
        } 

        private bool halfBoosterStatus;
        public bool HalfBoosterStatus
        {
            get { return halfBoosterStatus; }
            set
            {
                halfBoosterStatus = value;
                OnPropertyChanged(nameof(HalfBoosterStatus));
            }
        } 

        private bool _audienceHelpStatus;
        public bool AudienceHelpStatus
        {
            get { return _audienceHelpStatus; }
            set
            {
                _audienceHelpStatus = value;
                OnPropertyChanged(nameof(AudienceHelpStatus));
            }
        } 

        private bool _updated = false;
        public bool Updated
        {
            get { return _updated; }
            set
            {
                _updated = value;
                ErrorMessage = string.Empty;
                Message = string.Empty;
                OnPropertyChanged(nameof(Updated));
                OnPropertyChanged(nameof(ErrorMessage));
                OnPropertyChanged(nameof(Message));
            }
        }

        public Rank UserRank
        {
            get { return _userRank; }
            set
            {
                if (_userRank != value)
                {
                    _userRank = value;
                    OnPropertyChanged(nameof(UserRank));
                }
            }
        }


        private Brush _answer1Background = new SolidColorBrush(Colors.Blue);
        public Brush Answer1Background
        {
            get => _answer1Background;
            set
            {
                if (_answer1Background != value)
                {
                    _answer1Background = value;
                    OnPropertyChanged(nameof(Answer1Background));
                }
            }
        }

        private Brush _answer2Background = new SolidColorBrush(Colors.Blue);
        public Brush Answer2Background
        {
            get => _answer2Background;
            set
            {
                if (_answer2Background != value)
                {
                    _answer2Background = value;
                    OnPropertyChanged(nameof(Answer2Background));
                }
            }
        }

        private Brush _answer3Background = new SolidColorBrush(Colors.Blue);
        public Brush Answer3Background
        {
            get => _answer3Background;
            set
            {
                if (_answer3Background != value)
                {
                    _answer3Background = value;
                    OnPropertyChanged(nameof(Answer3Background));
                }
            }
        }

        private Brush _answer4Background = new SolidColorBrush(Colors.Blue);
        public Brush Answer4Background
        {
            get => _answer4Background;
            set
            {
                if (_answer4Background != value)
                {
                    _answer4Background = value;
                    OnPropertyChanged(nameof(Answer4Background));
                }
            }
        }


        private readonly ITopicRepository _topicRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IRankRepository _rankRepository;
        private List<Question> _questions;
        private List<Topic> _topics;
        private List<Answer> _answers;
        private List<int> _usedQuestionIndexes = new List<int>();
        private Rank _userRank;

        public ICommand UpdateDatasCommand { get; }
        public ICommand StartGameCommand { get; }
        public ICommand AnswerCommand { get; private set; }
        public ICommand CloseWindowCommand { get; }
        public ICommand HalfBoosterCommand { get; }
        public ICommand FriendPhoneCommand { get; }
        public ICommand AudienceHelpCommand { get; }


        public GameViewModel()
        {
            CurrentUser = SessionManager.Instance.CurrentUser;
            _topicRepository = new TopicRepository();
            _questionRepository = new QuestionRepository();
            _rankRepository = new RankRepository();
            UpdateDatasCommand = new AsyncRelayCommand(async _ => await UpdateDatas());
            StartGameCommand = new RelayCommand(_ => StartGame());
            AnswerCommand = new RelayCommand(AnswerClicked);
            HalfBoosterCommand = new RelayCommand(_=>ApplyHalfBoosterEffect());
            FriendPhoneCommand = new RelayCommand(_=> ApplyPhoneFriendHelp());
            AudienceHelpCommand = new RelayCommand(_=>ApplyAudienceHelp());
            HalfBoosterStatus = false;
            PhoneFriendHelpStatus = false;
            AudienceHelpStatus = false;
            _answers = new List<Answer>();
            questionTimer = new DispatcherTimer();
            questionTimer.Interval = TimeSpan.FromSeconds(1);
            questionTimer.Tick += QuestionTimer_Tick;
            InitializeAnswers();
        }

        private async Task UpdateDatas()
        {
            Updated = false;
            try
            {
                var topicResponse = await _topicRepository.GetByAll(CurrentUser.AuthToken);
                var questionResponse = await _questionRepository.GetByAll(CurrentUser.AuthToken);
                await GetUserRank();

                if (topicResponse.Success && questionResponse.Success)
                {
                    _topics = topicResponse.Data;
                    _questions = questionResponse.Data;
                    Message = "Adatok sikeresen frissítve";
                    await GetUserRank();
                    if (actualScore == 0 )
                    {
                        ErrorMessage = "Nem sikerült lekérni a felhasználó eredményét.";
                        Updated = false;
                    }
                }
                else
                {
                    Updated = false;
                    ErrorMessage = $"Hiba történt az adatok lekérésekor: {topicResponse.Message} {questionResponse.Message}";
                }
            }
            catch (Exception ex)
            {
                Updated = false;

                ErrorMessage = $"Hiba történt az adatok frissítése közben: {ex.Message}";
            }
            gameRunning = false;
        }


        private async Task GetUserRank()
        {
            var response = await _rankRepository.GetScore(CurrentUser.Id, CurrentUser.AuthToken);
            if (response.Success)
            {
                actualScore = response.Data.Score;

            }
        }


        private void QuestionTimer_Tick(object sender, EventArgs e)
        {
            _timeLeft--;
            OnPropertyChanged(nameof(Clock));
            if (_timeLeft <= 0)
            {
                questionTimer.Stop();
                MessageBox.Show("Az idő lejárt! Következő kérdés...");
                DisplayCurrentQuestion(); // Következő kérdésre lépés
            }
        }

        private void StartQuestionTimer(int seconds)
        {
            _timeLeft = seconds;
            OnPropertyChanged(nameof(Clock));
            questionTimer.Start();
        }


        public void ResumeTimer()
        {
            if (!questionTimer.IsEnabled && _questions != null && gameRunning )
            {
                questionTimer.Start();
            }
        }

        public void PauseTimer()
        {
            if (questionTimer.IsEnabled)
            {
                questionTimer.Stop();
            }
        }

        private void StartGame()
        {
            // Feltételezve, hogy van egy alapértelmezett felhasználói ID

            if (_questions == null || _questions.Count == 0)
            {
                Updated = false;
                ErrorMessage = "Kérlek, először frissítsd az adatokat!";
                return;
            }

            Updated = true;
            // Játékállapot inicializálása
            ResetColors();
            _score = 0;
            Score = _score;
            HalfBoosterStatus = true;
            PhoneFriendHelpStatus = true;
            AudienceHelpStatus = true;
            DisplayCurrentQuestion();
        }

        private async void DisplayCurrentQuestion()
        {
            gameRunning = true;
            if (_usedQuestionIndexes.Count < 10)
            {
                Random rand = new ();
                int questionIndex;
                do
                {
                    questionIndex = rand.Next(_questions.Count);
                }
                while (_usedQuestionIndexes.Contains(questionIndex));

                _usedQuestionIndexes.Add(questionIndex);
                var currentQuestion = _questions[questionIndex];
                var topic = _topics.FirstOrDefault(t => t.Id == currentQuestion.TopicId);
                TopicName = $"Téma: {topic?.TopicName ?? "Ismeretlen téma"}";
                QuestionText = currentQuestion.QuestionText;

                // Válaszok beállítása
                _answers.Clear();

                Answer1 = currentQuestion.Answers.Count > 0 ? currentQuestion.Answers[0] : new Answer();              
                Answer2 = currentQuestion.Answers.Count > 1 ? currentQuestion.Answers[1] : new Answer();
                Answer3 = currentQuestion.Answers.Count > 2 ? currentQuestion.Answers[2] : new Answer();
                Answer4 = currentQuestion.Answers.Count > 3 ? currentQuestion.Answers[3] : new Answer();

                InOrActivateButtons(true);

                _answers.Add(Answer1);
                _answers.Add(Answer2);
                _answers.Add(Answer3);
                _answers.Add(Answer4);

                StartQuestionTimer(20);
            }
            else
            {
                Updated = false;
                gameRunning = false;
                actualScore += _score;
                Message = $"A játék véget ért! Pontszámod: {actualScore}";
                // Itt hívjuk meg az UpdateUserRankAsync metódust
                UserRank = new Rank
                {
                    UserId = CurrentUser.Id,
                    Score = actualScore
                };
                var response = await _rankRepository.PutScore(CurrentUser.Id, UserRank, CurrentUser.AuthToken);
                if (response.Success)
                {
                    MessageBox.Show("Sikeresen frissítettük a pontszámodat!");
                }
                else
                {
                    MessageBox.Show("Nem sikerült frissíteni a felhasználó rangját.");

                }
                Application.Current.Dispatcher.Invoke(() =>
                {
                    var result = MessageBox.Show("Szeretnél még egy játékot játszani?", "Játék vége", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                    {
                        _usedQuestionIndexes.Clear(); // Lista ürítése új játék indításakor
                        StartGame();
                    }
                    else
                    {
                        Updated = false;
                        _usedQuestionIndexes.Clear();
                        Message =$" Összpontszámod: {actualScore }";
                    }
                });
            }
        }

        private void ApplyHalfBoosterEffect()
        {
            int currentQuestionIndex = _usedQuestionIndexes.Last();
            var currentAnswers = _questions[currentQuestionIndex].Answers;
            var incorrectAnswers = currentAnswers.Where(a => !a.IsCorrect).ToList();


            // A számolás, hogy legalább kettő helytelen válasz maradjon látható
            int numberOfAnswersToRemove = incorrectAnswers.Count - 1;

            while (numberOfAnswersToRemove > 0)
            {
                var toRemove = incorrectAnswers[rand.Next(incorrectAnswers.Count)];
                // Hozzáférés a válasz indexéhez, hogy beállíthassuk a hátteret
                int index = currentAnswers.IndexOf(toRemove);
                SetAnswerInactiveAndRed(index);
                incorrectAnswers.Remove(toRemove);
                numberOfAnswersToRemove--;
            }
            HalfBoosterStatus = false; // A félidős segítség kikapcsolása
        }

        private void SetAnswerInactiveAndRed(int answerIndex)
        {
            // Az index alapján a megfelelő válasz és háttérszín deaktiválása és pirosra állítása
            switch (answerIndex)
            {
                case 0:
                    Answer1.IsActive = false;
                    Answer1Background = new SolidColorBrush(Colors.Red);
                    break;
                case 1:
                    Answer2.IsActive = false;
                    Answer2Background = new SolidColorBrush(Colors.Red);
                    break;
                case 2:
                    Answer3.IsActive = false;
                    Answer3Background = new SolidColorBrush(Colors.Red);
                    break;
                case 3:
                    Answer4.IsActive = false;
                    Answer4Background = new SolidColorBrush(Colors.Red);
                    break;
            }
        }

        public void ApplyPhoneFriendHelp()
        {

            int currentQuestionIndex = _usedQuestionIndexes.Last();
            var currentAnswers = _questions[currentQuestionIndex].Answers;

            // Valószínűség a helyes válasz kiválasztására
            double probabilityOfCorrectAnswer = 0.70; // 70% esély a helyes válaszra

            // Véletlenszerűen döntünk, hogy a segítő helyes választ ad-e
            bool helperIsCorrect = rand.NextDouble() < probabilityOfCorrectAnswer;

            int answerIndex;
            if (helperIsCorrect || !currentAnswers.Any(a => a.IsCorrect))
            {
                // Kiválasztjuk a helyes válaszok egyikét, ha a segítő helyes, vagy ha nincs helyes válasz
                var correctAnswers = currentAnswers.Where(a => a.IsCorrect).ToList();
                answerIndex = currentAnswers.IndexOf(correctAnswers[rand.Next(correctAnswers.Count)]);
            }
            else
            {
                // Kiválasztjuk a helytelen válaszok egyikét
                var incorrectAnswers = currentAnswers.Where(a => !a.IsCorrect).ToList();
                answerIndex = currentAnswers.IndexOf(incorrectAnswers[rand.Next(incorrectAnswers.Count)]);
            }

            // A kiválasztott válasz megjelölése, feltételezve, hogy van olyan UI elem, ami ezt megjeleníti
            PhoneFriendHelpStatus = false;
            HighlightAnswerForPhoneFriendHelp(answerIndex);
        }

        private void HighlightAnswerForPhoneFriendHelp(int answerIndex)
        {
            // Itt valósíthatod meg, hogyan jeleníted meg a segítő által adott választ a felhasználónak.
            MessageBox.Show($"A telefonos segítő válasza: {_questions[_usedQuestionIndexes.Last()].Answers[answerIndex].AnswerText}");
        }


        public void ApplyAudienceHelp()
        {
            int currentQuestionIndex = _usedQuestionIndexes.Last();
            var currentAnswers = _questions[currentQuestionIndex].Answers;

            // A közönség tagjainak számának generálása
            int audienceSize = rand.Next(50, 101); // Például 50 és 100 közötti érték

            // Elosztjuk a szavazatokat véletlenszerűen, de egy súlyozással, ami favorizálja a helyes választ
            Dictionary<int, int> votes = new Dictionary<int, int>();
            foreach (var answer in currentAnswers)
            {
                votes[currentAnswers.IndexOf(answer)] = 0;
            }

            int correctAnswerIndex = currentAnswers.IndexOf(currentAnswers.FirstOrDefault(a => a.IsCorrect));
            int bonusVotesForCorrect = (int)(audienceSize * 0.3); // Tegyük fel, hogy a helyes válasz +30% bónusz szavazatot kap

            // Szavazatok elosztása
            for (int i = 0; i < audienceSize; i++)
            {
                int voteIndex = rand.Next(currentAnswers.Count);
                if (rand.NextDouble() < 0.7) // 70% esély, hogy a szavazat a helyes válaszra megy
                {
                    voteIndex = correctAnswerIndex;
                }
                votes[voteIndex]++;
            }

            // A helyes válasz extra szavazatokat kap
            votes[correctAnswerIndex] += bonusVotesForCorrect;

            AudienceHelpStatus = false;

            // Eredmény megjelenítése
            ShowAudiencePollResults(votes, audienceSize);
        }

        private static void ShowAudiencePollResults(Dictionary<int, int> votes, int audienceSize)
        {
            // Itt valósíthatod meg, hogy hogyan jeleníted meg a közönség szavazatát.
            StringBuilder result = new ();
            result.AppendLine("Közönség szavazatai:");
            foreach (var vote in votes)
            {
                result.AppendLine($"Válasz {vote.Key + 1}: {(double)vote.Value / audienceSize * 100:0.0}%");
            }
            MessageBox.Show(result.ToString());
        }





        private async void AnswerClicked(object parameter)
        {
            questionTimer.Stop();
            if (int.TryParse(parameter.ToString(), out int answerIndex) && _usedQuestionIndexes.Any())
            {
                InOrActivateButtons(false);
                int currentQuestionIndex = _usedQuestionIndexes.Last();

                // Validáljuk a válaszokat mielőtt folytatnánk
                if (!ValidateAnswers(currentQuestionIndex))
                {
                    return; // Korai kilépés, ha a validáció sikertelen
                }

                var selectedAnswer = _questions[currentQuestionIndex].Answers[answerIndex];
                var correctAnswer = _questions[currentQuestionIndex].Answers.FirstOrDefault(a => a.IsCorrect);
                int correctAnswerIndex = _questions[currentQuestionIndex].Answers.IndexOf(correctAnswer);

                // A választott válasz sárgára állítása
                ChangeAnswerBackground(false, answerIndex);

                if (selectedAnswer.IsCorrect)
                {
                    ChangeAnswerBackground(true, answerIndex);
                    Score += 100;
                }
                else
                {
                    ChangeAnswerBackground(false, answerIndex);
                    ChangeAnswerBackground(true, correctAnswerIndex);
                }

                await Task.Delay(4000); // 4 másodperc késleltetés

                // A következő kérdésre lépés előkészítése
                DisplayCurrentQuestion();
                ResetColors();
            }
            else
            {
                Updated = false;
                Message = string.Empty;
                gameRunning = false;
                ErrorMessage = "Nem sikerült értelmezni a választ: " + parameter;
            }
        }


        private void ResetColors()
        {

            Answer1Background =new SolidColorBrush(Colors.Blue);
            Answer2Background =new SolidColorBrush(Colors.Blue);
            Answer3Background =new SolidColorBrush(Colors.Blue);
            Answer4Background =new SolidColorBrush(Colors.Blue);
        }

        // Segédmetódus a gomb hátterének beállításához
        // Logikai funkciók a háttérszínek megváltoztatására minden válasznál
        public void ChangeAnswerBackground(bool correct, int answerIndex)
        {
            switch (answerIndex)
            {
                case 0:
                    Answer1Background = correct ? new SolidColorBrush(Colors.Green) : new SolidColorBrush(Colors.Gold);
                    break;
                case 1:
                    Answer2Background = correct ? new SolidColorBrush(Colors.Green) : new SolidColorBrush(Colors.Gold);
                    break;
                case 2:
                    Answer3Background = correct ? new SolidColorBrush(Colors.Green) : new SolidColorBrush(Colors.Gold);
                    break;
                case 3:
                    Answer4Background = correct ? new SolidColorBrush(Colors.Green) : new SolidColorBrush(Colors.Gold);
                    break;
            }
        }

        private void InOrActivateButtons(bool status)
        {
            if (Answer1 != null) Answer1.IsActive = status;
            if (Answer2 != null) Answer2.IsActive = status;
            if (Answer3 != null) Answer3.IsActive = status;
            if (Answer4 != null) Answer4.IsActive = status;
        }

        private void InitializeAnswers()
        {
            Answer1 = new Answer { IsActive = false };
            Answer2 = new Answer { IsActive = false };
            Answer3 = new Answer { IsActive = false };
            Answer4 = new Answer { IsActive = false };
        }


        private bool ValidateAnswers(int currentQuestionIndex)
        {
            var question = _questions.ElementAtOrDefault(currentQuestionIndex);

            // Ellenőrizzük, hogy létezik-e a kérdés és vannak-e válaszok
            if (question == null || question.Answers == null || !question.Answers.Any())
            {
                Updated = false;
                Message = string.Empty;
                gameRunning = false;
                ErrorMessage = "Elnézést, valamilyen hiba történt, kezdje újra!";
                return false;
            }

            // Ellenőrizzük, hogy minden válasz létezik-e
            foreach (var answer in question.Answers)
            {
                if (answer == null || answer.AnswerText == null)
                {
                    Updated = false;
                    Message = string.Empty;
                    gameRunning = false;
                    ErrorMessage = "Elnézést, valamilyen hiba történt, kezdje újra!";
                    return false;
                }
            }

            return true;
        }

    }
}
