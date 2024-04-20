using MagicQuizDesktop.Commands;
using MagicQuizDesktop.Models;
using MagicQuizDesktop.Repositories;
using MagicQuizDesktop.View.Windows;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System.IO;
using System.Security.Cryptography;
using MagicQuizDesktop.Services;

namespace MagicQuizDesktop.ViewModels
{
    public class ProfileViewModel : ViewModelBase
    {

        string _errorMessage = string.Empty;
        private readonly IUserRepository _userRepository;
        public string _baseAvatarUri = "/media/";


        private User _currentUser;
        public User CurrentUser
        {
            get => _currentUser;
            set
            {
                _currentUser = value;
                OnPropertyChanged(nameof(CurrentUser));
            }
        } 
        private User _selectedUser;
        public User SelectedUser
        {
            get
            {
                switch (_selectedUser.Gender)
                {
                    case "male":
                        _selectedUser.Avatar = $"{_baseAvatarUri}male_avatar.png";
                        break;
                    case "female":
                        _selectedUser.Avatar = $"{_baseAvatarUri}female_avatar.png";
                        break;
                    default:
                        _selectedUser.Avatar = $"{_baseAvatarUri}unKnown_avatar.png";
                        break;
                }
                return _selectedUser;
            }
            set
            {
                _selectedUser = value;
                OnPropertyChanged(nameof(SelectedUser));
            }
        }
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                if (_errorMessage != value)
                {
                    _errorMessage = value;
                    OnPropertyChanged(nameof(ErrorMessage));
                }
            }
        }

        public ProfileViewModel()
        {
            CurrentUser = SessionManager.Instance.CurrentUser;
            SelectedUser ??= new User();
            _userRepository = new UserRepository();
            SubmitProfileCommand = new AsyncRelayCommand(_ => UpdateCurrentUserAsync());
        }

        public ICommand SubmitProfileCommand { get; }
        public ICommand InactivateCommand { get; }


        public async Task UpdateCurrentUserAsync()
        {         
            ErrorMessage = string.Empty;
            if (!ValidateProfileFields())
            {
                return;
            }
            SetUserGender();
            using (var client = new HttpClient())
            {
                // Cím és token beállítása
                client.BaseAddress = new Uri("http://127.0.0.1:8000/");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CurrentUser.AuthToken);

                // Felhasználó adatainak szerializálása JSON formátumba
                var userJson = JsonConvert.SerializeObject(CurrentUser);
                var content = new StringContent(userJson, Encoding.UTF8, "application/json");

                try
                {
                    // Küldés a szerverre
                    var response = await client.PutAsync($"api/users/{CurrentUser.Id}", content);
                    if (!response.IsSuccessStatusCode)
                    {
                        ErrorMessage = $"Server error: {response.ReasonPhrase}";
                        return;
                    }
                }
                catch (HttpRequestException e)
                {
                    ErrorMessage = $"Request error: {e.Message}";
                    return;
                }
                catch (Exception e)
                {
                    ErrorMessage = $"General error: {e.Message}";
                    return;
                }
            }
            return;
        }

        private bool ValidateProfileFields()
        {

            if (string.IsNullOrEmpty(CurrentUser.Name))
            {
                ErrorMessage = "A név megadása kötelező.";
                return false;
            }

            if (string.IsNullOrEmpty(CurrentUser.Email))
            {
                ErrorMessage = "E-mail cím megadása kötelező.";
                return false;
            }

            if (string.IsNullOrEmpty(CurrentUser.Gender))
            {
                ErrorMessage = "A nem megadása kötelező";
                return false;
            }

            return true;
        }

        public void SetUserGender()
        {
            switch (CurrentUser.Gender.ToLower())
            {
                case "férfi":
                    CurrentUser.Gender = "male";
                    break;
                case "nő":
                    CurrentUser.Gender = "female";
                    break;
                default:
                    CurrentUser.Avatar = "unKnown";
                    break;
            }

            OnPropertyChanged(nameof(CurrentUser.Avatar));  // Notify the change
        }

    }
}
