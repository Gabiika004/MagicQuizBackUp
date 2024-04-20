using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicQuizDesktop.Services
{
    public class UserEventArgs : EventArgs
    {
        public Models.User SelectedUser { get; private set; }

        public UserEventArgs(Models.User user)
        {
            SelectedUser = user;
        }
    }
}
