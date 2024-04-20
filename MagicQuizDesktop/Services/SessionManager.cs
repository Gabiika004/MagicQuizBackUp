using MagicQuizDesktop.Models;
using MagicQuizDesktop.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicQuizDesktop.Services
{
    public class SessionManager
    {
        private static SessionManager _instance;
        private UsersViewModel _usersViewModel;

        public static SessionManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SessionManager();
                }
                return _instance;
            }
        }

        public UsersViewModel UsersViewModel
        {
            get
            {
                if (_usersViewModel == null)
                {
                    _usersViewModel = new UsersViewModel();
                }
                return _usersViewModel;
            }
        }



        public User CurrentUser { get; private set; }
        public Question CurrentQuestion { get; private set; }

        public void SetCurrentUser(User user)
        {
            CurrentUser = user;
        }
        public void SetCurrentQuestion(Question question)
        {
            CurrentQuestion = question;
        }

        public void ClearCurrentUser()
        {
            CurrentUser = null;
        }
    }

}
