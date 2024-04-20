using MagicQuizDesktop.Commands;
using MagicQuizDesktop.Models;
using MagicQuizDesktop.Services;
using MagicQuizDesktop.View.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace MagicQuizDesktop.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        public User CurrentUser
        {
            get => _currentUser;
            set
            {
                _currentUser = value;
                OnPropertyChanged(nameof(CurrentUser));
            }
        }
        private User _currentUser;

        private List<string> _articles;

        public List<String> Articles
        {
            get => _articles;
            set
            {
                _articles = value;
                OnPropertyChanged(nameof(Articles));
            }
        }

        public ICommand StartGameClickCommand { get; } 
        public ICommand AddArticleClickCommand { get; } 
        public HomeViewModel()
        {
            Initialize();
            StartGameClickCommand = new RelayCommand(_ => OpenGameWindow());
        }

        private static void OpenGameWindow()
        {
            GameWindow window = new();
            window.ShowDialog();
        }

        private void Initialize()
        {
            if (SessionManager.Instance.CurrentUser != null)
            {
                CurrentUser = SessionManager.Instance.CurrentUser;

            }
            else
            {
                CurrentUser = new User();
            }
            SetArticles();
        }

        private void SetArticles()
        {
            Articles = new List<string>();
            string article1 =   "Köszöntünk a Magic Quiz-ben, ahol a tudásod varázslatos próbára teszed! " +
                                "Készülj fel egy izgalmas kalandra, ahol minden kérdés egy újabb lépés a tudás birodalmában." +
                                "A játék egyszerű: tíz különböző témájú kérdés, mindegyikre csak egy helyes válasz létezik.";

            string article2 =   "Figyelj, mert az idő szorít! Minden kérdésre csupán 20 másodperced van a válaszadásra," +
                                "így gyorsaságod és tudásod egyaránt próbára kerül. Minden helyes válaszért 100 pontot kapsz," +
                                "így a maximális pontszám elérése felé törhetsz. Ha elégséges pontot gyűjtesz," +
                                "bekerülhetsz a ranglistára, ahol összemérheted tudásodat más kvízvarázslókkal.";

            string article3 =   "Tipp: Elakadatál? Használd a segítségeket! Minden új játék kezdetekor kapsz 3 rendkívüli szolgáltatást:" +
                                "Felező/Közönség/Telefonhívás";

            Articles.Add(article1);
            Articles.Add(article2);
            Articles.Add(article3);
        }

    }
}
