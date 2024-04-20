using MagicQuizDesktop.Commands;
using MagicQuizDesktop.Models;
using MagicQuizDesktop.Repositories;
using MagicQuizDesktop.Services;
using MagicQuizDesktop.View.Windows;
using Microsoft.VisualBasic.ApplicationServices;
using Newtonsoft.Json; // Newtonsoft.Json importálása
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace MagicQuizDesktop.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {

        private readonly IUserRepository _userRepository;

        private string email;
        public string Email
        {
            get => email;
            set
            {
                if (email != value)
                {
                    email = value;
                    OnPropertyChanged(nameof(Email));
                }
            }
        }

        private bool _isViewVisible = true;
        public bool IsViewVisible
        {
            get
            {
                return _isViewVisible;
            }
            set
            {
                _isViewVisible = value;
                OnPropertyChanged(nameof(IsViewVisible));
            }
        }

        private string password;
        public string Password
        {
            get { return password; }
            set
            {
                if (password != value)
                {
                    password = value;
                    OnPropertyChanged(nameof(Password));
                }
            }
        }

        private string _errorMessage = string.Empty; 
        private string _message;
        public string ErrorMessage
        {
            get
            {
                return _errorMessage;
            }

            set
            {
                _errorMessage = value;
                Message = string.Empty;
                OnPropertyChanged(nameof(ErrorMessage));
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


        public ICommand LoginCommand { get; private set; }

        public LoginViewModel()
        {
            _userRepository = new UserRepository();
            LoginCommand = new AsyncRelayCommand(ExecuteLoginCommand, (o) => CanExecuteLoginCommand(o));
        }


        private bool ValidateLoginInput()
        {
            if (string.IsNullOrWhiteSpace(Email))
            {
                ErrorMessage = "Az e-mail cím megadása kötelező.";
                return false;
            }
            // Egy egyszerű email cím validálás
            if (!Email.Contains("@") || !Email.Contains("."))
            {
                ErrorMessage = "Érvénytelen email cím.";
                return false;
            }
            if (string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "A jelszó megadása kötelező.";
                return false;
            }
            if (Password.Length < 8)
            {
                ErrorMessage = "A jelszónak legalább 8 karakter hosszúnak kell lennie.";
                return false;
            }

            return true;
        }

        private async Task ExecuteLoginCommand(object obj)
        {
            if (!ValidateLoginInput())
            {
                return;
            }

            // Megpróbálja azonosítani a felhasználót.
            var loginResponse = await _userRepository.AuthenticateUser(Email, Password);
            if (loginResponse.Success && loginResponse.Data != null && !string.IsNullOrEmpty(loginResponse.Data.Token))
            {
                    Message = "Sikeres bejelentkezés";
                // Sikeres azonosítás esetén lekéri a felhasználó adatait.
                var userResponse = await _userRepository.GetById(loginResponse.Data.UserId, loginResponse.Data.Token);
                if (userResponse.Success && userResponse.Data != null)
                {
                    userResponse.Data.AuthToken = loginResponse.Data.Token; // Beállítjuk az AuthToken-t a felhasználói objektumban.
                    SessionManager.Instance.SetCurrentUser(userResponse.Data);

                    // Felhasználói felület frissítése.
                    var mainWindow = new MainWindow();
                    mainWindow.Show();
                    IsViewVisible = false;  // Az aktuális ablakot elrejti.
                }
                else
                {
                    // Ha nem sikerült a felhasználó adatainak lekérése.
                    ErrorMessage = "Sikertelen feldolgozás: " + userResponse.Message;
                }
            }
            else
            {
                // Ha az azonosítás nem sikerült.
                ErrorMessage = loginResponse.Message;
            }
        }


        private bool CanExecuteLoginCommand(object obj)
        {
            return !string.IsNullOrWhiteSpace(Email) && !string.IsNullOrWhiteSpace(Password);
   
        }





    }
}
