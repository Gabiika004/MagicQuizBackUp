using MagicQuizDesktop.Commands;
using MagicQuizDesktop.Models;
using MagicQuizDesktop.Repositories;
using MagicQuizDesktop.Services;
using MagicQuizDesktop.View.Windows;
using MagicQuizDesktop.ViewModels;
using Microsoft.VisualBasic.ApplicationServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms.Design;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using System.Windows.Forms;

namespace MagicQuizDesktop.ViewModels
{
    public class UsersViewModel : ViewModelBase
    {
        private readonly IUserRepository _userRepository;
        private Models.User _currentUser;
        private Models.User _selectedUser;
        private string _selectedUserActiveText;
        private string _selectedUserColor;
        private string _isActiveActionText;
        private string _errorMessage = string.Empty;
        private string _message = string.Empty;
        private List<Models.User> _users;
        public string _baseAvatarUri = "/media/";
        public event EventHandler<UserEventArgs> UserSelected;

        public Models.User CurrentUser
        {
            get => _currentUser;
            set
            {
                _currentUser = value;
                OnPropertyChanged(nameof(CurrentUser));
            }
        }
        public string? ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                if (_errorMessage != value)
                {
                    _errorMessage = value;
                    Message = string.Empty;
                    OnPropertyChanged(nameof(ErrorMessage));
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
                    ErrorMessage = string.Empty;
                    OnPropertyChanged(nameof(Message));
                }
            }
        }
        public string SelectedUserActiveText
        {
            get => _selectedUserActiveText;
            set
            {
                _selectedUserActiveText = value;
                OnPropertyChanged(nameof(SelectedUserActiveText));
            }
        } 
        
        public string SelectedUserColor
        {
            get => _selectedUserColor;
            set
            {
                _selectedUserColor = value;
                OnPropertyChanged(nameof(SelectedUserColor));
            }
        }
        public string IsActiveActionText
        {
            get => _isActiveActionText;
            set
            {
                _isActiveActionText = value;
                OnPropertyChanged(nameof(IsActiveActionText));
            }
        }

        public List<Models.User> Users
        {
            get => _users;
            set
            {
                if (_users != value)
                {
                    _users = value;
                    SetUserAvatars(_users);
                    OnPropertyChanged(nameof(Users));
                }
            }
        }

        public Models.User SelectedUser
        {
            get => _selectedUser;
            set
            {
                if (_selectedUser != value)
                {
                    _selectedUser = value;
                    SetUserGender();
                    OnPropertyChanged(nameof(SelectedUser));

                    // Frissítsük az aktív szöveg és szín változókat
                    UpdateActiveStatusProperties(_selectedUser.IsActive);
                }
            }
        }

        public List<string> Genders { get; }

        public ICommand ShowUserWindowCommand { get; }
        public ICommand SubmitProfileCommand { get; }
        public ICommand DeOrActivateCommand { get; }

        public UsersViewModel()
        {
            CurrentUser = SessionManager.Instance.CurrentUser;
            Genders = new List<string> { "férfi", "nő", "egyéb" };
            _userRepository = new UserRepository();
            ShowUserWindowCommand = new RelayCommand(ExecuteShowUserWindowCommand);
            SubmitProfileCommand = new AsyncRelayCommand(_ => UpdateSelectedUserAsync());
            DeOrActivateCommand = new AsyncRelayCommand(_ => DeOrActivateSelectedUserAsync());
        }

        // Ez a metódus a normál inicializálást végzi, amit a Page használ
        public async Task InitializeAsync()
        {
            await GetUsers();
            SelectedUser = new Models.User();
        }

        // Ez a metódus akkor használatos, ha nem szeretnénk adatokat betölteni
        public void InitializeForWindow()
        {
            // Ellenőrizzük, hogy a SelectedUser null-e, és ha igen, akkor rendelünk neki egy új User objektumot.
            SelectedUser ??= new Models.User();
        }

        private void UpdateActiveStatusProperties(bool isActive)
        {
            SelectedUserActiveText = isActive ? "Aktív" : "Inaktív";
            SelectedUserColor = isActive ? "Green" : "Red";
            _isActiveActionText = isActive ? "Deaktiválás" : "Aktiválás";

            // Értesítsük a nézetet a változásokról
            OnPropertyChanged(nameof(SelectedUserActiveText));
            OnPropertyChanged(nameof(SelectedUserColor));
            OnPropertyChanged(nameof(IsActiveActionText));
        }

        public async Task DeOrActivateSelectedUserAsync()
        {
            if (SelectedUser.IsActive)
            {
                try
                {
                    var response = await _userRepository.Inactivate(SelectedUser, CurrentUser.AuthToken);
                    if (response.Success)
                    {
                        Message = "Felhasználó sikeresen deaktiválva!";
                    }
                    else
                    {
                        ErrorMessage = $"Sikertelen deaktiválás: {response.Message}";
                    }
                }
                catch (Exception ex)
                {
                    ErrorMessage = $"Hiba az alkalmazásban:{ex.Message}";

                }
            }
            else
            {
                if (!ValidateProfileFields())
                {
                    return;
                }
                try
                {
                    SelectedUser.IsActive = true;
                    SetUserGender();
                    var response = await _userRepository.UpdateUser(SelectedUser, CurrentUser.AuthToken);
                    if (response.Success)
                    {
                        Message = "Felhasználó sikeresen aktiválva!";
                    }
                    else
                    {
                        ErrorMessage = $"Sikertelen aktiválás: {response.Message}";
                    }
                }
                catch (Exception ex)
                {
                    ErrorMessage = $"Hiba az alkalmazásban:{ex.Message}";

                }

            }
        }

        public async Task GetUsers()
        {
            try
            {
                var response = await _userRepository.GetByAll(CurrentUser.AuthToken);
                if (response.Success)
                {
                    Users = response.Data;
                    
                    ErrorMessage = string.Empty;
                }
                else
                {
                    ErrorMessage = $"Error: {response.Message}";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Hiba az alkalmazásban:{ex.Message}";

            }
        }

        private void ExecuteShowUserWindowCommand(object param)
        {
            if (param is Models.User user && user != null)
            {
                SelectedUser = user;
                ProfileWindow window = new(){ DataContext = this };
                window.Closed += (sender, args) => {
                    SelectedUser = new Models.User();  
                    _=InitializeAsync();
                };
                window.ShowDialog();  

            }
            else
            {
                ErrorMessage = "Hoppá! A megadott objektum nem érvényes felhasználó!";
            }
        }


        public async Task UpdateSelectedUserAsync()
        {
            if (!ValidateProfileFields())
            {
                return;
            }
            try
            {
                SetUserGender();
                var response = await _userRepository.UpdateUser(SelectedUser, CurrentUser.AuthToken);
                if (response.Success)
                {
                    Message = "Felhasználó sikeresen aktiválva!";
                }
                else
                {
                    ErrorMessage = $"Frissítés sikertelen: {response.Message}";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Hiba az alkalmazásban:{ex.Message}";

            }
        }

        private bool ValidateProfileFields()
        {

            if (string.IsNullOrEmpty(SelectedUser.Name))
            {
                ErrorMessage = "A név megadása kötelező.";
                return false;
            }

            if (string.IsNullOrEmpty(SelectedUser.Email))
            {
                ErrorMessage = "E-mail cím megadása kötelező.";
                return false;
            }

            if (string.IsNullOrEmpty(SelectedUser.Gender))
            {
                ErrorMessage = "A nem megadása kötelező";
                return false;
            }

            return true;
        }

        public void SetUserGender()
        {
            if (SelectedUser.Gender == null)
            {
                return;
            }
            string lowerGender = SelectedUser.Gender.ToLower();
            if (lowerGender == "férfi" || lowerGender == "male")
            {
                SelectedUser.Gender = "férfi";
                SelectedUser.Avatar = $"{_baseAvatarUri}male_avatar.png";
            }
            else if (lowerGender == "nő" || lowerGender == "female")
            {
                SelectedUser.Gender = "nő";
                SelectedUser.Avatar = $"{_baseAvatarUri}female_avatar.png";
            }
            else
            {
                SelectedUser.Gender = "egyéb";
                SelectedUser.Avatar = $"{_baseAvatarUri}unknown_avatar.png";
            }
            OnPropertyChanged(nameof(SelectedUser.Gender));
            OnPropertyChanged(nameof(SelectedUser.Avatar));
        }

        private void SetUserAvatars(List<Models.User> users)
        {
            foreach (var user in users)
            {
                if (user.Gender == null)
                {
                    return;
                }
                string lowerGender = user.Gender.ToLower();
                if (lowerGender == "férfi" || lowerGender == "male")
                {
                    user.Gender = "férfi";
                    user.Avatar = $"{_baseAvatarUri}male_avatar.png";
                }
                else if (lowerGender == "nő" || lowerGender == "female")
                {
                    user.Gender = "nő";
                    user.Avatar = $"{_baseAvatarUri}female_avatar.png";
                }
                else
                {
                    user.Gender = "egyéb";
                    user.Avatar = $"{_baseAvatarUri}unknown_avatar.png";
                }
            }
        }

    }
}
