using MagicQuizDesktop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicQuizDesktop.Services
{
    public class LoginSuccessEventArgs : EventArgs
    {
        public LoginUser User { get; set; }

        public LoginSuccessEventArgs(LoginUser user)
        {
            User = user;
        }
    }

}
