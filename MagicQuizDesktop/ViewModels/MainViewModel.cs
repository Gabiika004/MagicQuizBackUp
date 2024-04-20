using FontAwesome.Sharp;
using MagicQuizDesktop.Commands;
using MagicQuizDesktop.Models;
using MagicQuizDesktop.Repositories;
using MagicQuizDesktop.Services;
using MagicQuizDesktop.View.Pages;
using MagicQuizDesktop.View.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MagicQuizDesktop.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private Page _currentChildView;
        private string _caption;
        private IconChar _icon;
        private readonly IUserRepository _userRepository;

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

        public ICommand LoadUserDataCommand { get; }
        public ICommand ShowHomeViewCommand { get; }
        public ICommand ShowProfileViewCommand { get; }
        public ICommand ShowGameViewCommand { get; }
        public ICommand ShowUsersViewCommand { get; }
        public ICommand ShowTopicViewCommand { get; }
        public ICommand ShowQuestionViewCommand { get; }
        public ICommand ShowRankViewCommand { get; }
        public ICommand LogOutCurrentUserCommand { get; }

        public Page CurrentChildView 
        { get => _currentChildView;
            set 
            {
                _currentChildView = value;
                OnPropertyChanged(nameof(CurrentChildView));
            }
        }
        public string Caption
        {
            get => _caption;
            set
            {
                _caption = value;
                OnPropertyChanged(nameof(Caption));
            }
        }
        public IconChar Icon 
        { 
            get => _icon;
            set
            {
                _icon = value;
                OnPropertyChanged(nameof(Icon));
            }
        }

        public MainViewModel()
        {
            CurrentUser = SessionManager.Instance.CurrentUser;
            _userRepository = new UserRepository();
            ShowHomeViewCommand = new RelayCommand(_=>ExecuteShowHomeViewCommand());
            ShowUsersViewCommand = new RelayCommand(_=>ExecuteShowUsersViewCommand());
            ShowTopicViewCommand = new RelayCommand(_=>ExecuteShowTopicViewCommand());
            ShowQuestionViewCommand = new RelayCommand(_=>ExecuteShowQuestionViewCommand());
            ShowRankViewCommand = new RelayCommand(_=>ExecuteShowRankViewCommand());
            LogOutCurrentUserCommand = new AsyncRelayCommand(async _ => await LogOutCurrentUser());
            ExecuteShowHomeViewCommand();
        }

        private async Task LogOutCurrentUser()
        {

            ApiResponseWithNoData response = await _userRepository.LogOut(CurrentUser.AuthToken);
            if (response.Success)
            {

                SessionManager.Instance.ClearCurrentUser();
                System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                Application.Current.Shutdown();
      
            }
            else
            {

                StringBuilder result = new();
                result.AppendLine("Kijelentkezés sikertelen:");
                result.AppendLine(response.Message);
                result.AppendLine("Bezárja az alkalmazást?");
                
                var answer = MessageBox.Show(result.ToString(),"Hiba", MessageBoxButton.YesNo);
                if (answer == MessageBoxResult.Yes)
                {
                    Application.Current.Shutdown();
                }                
            }
        }


        private void ExecuteShowTopicViewCommand()
        {
            CurrentChildView = new TopicPage();
            Caption = "Témák";
            Icon = IconChar.Earth;
        }

        private void ExecuteShowRankViewCommand()
        {            
            CurrentChildView = new RankPage();
            Caption = "Ranglista";
            Icon = IconChar.RankingStar;
        }
        private void ExecuteShowQuestionViewCommand()
        {         
            CurrentChildView = new QuestionsPage();
            Caption = "Profil";
            Icon = IconChar.UserAlt;
        }
        private void ExecuteShowHomeViewCommand()
        {
            CurrentChildView = new HomePage();
            Caption = "Kezdőlap";
            Icon = IconChar.Home;
        }

        private void ExecuteShowUsersViewCommand()
        {
            CurrentChildView = new UsersPage();
            Caption = "Felhasználók";
            Icon = IconChar.UserGroup;
        }

    }
}
