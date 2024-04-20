using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MagicQuizDesktop.Models
{
    public interface IUserRepository
    {

        Task<ApiResponse<LoginUser>> AuthenticateUser (string username, string password);
        Task<ApiResponse<User>> GetById (int id, string authToken);
        Task<ApiResponse<List<User>>> GetByAll (string authToken);
        Task<ApiResponse<User>> UpdateUser (User user, string authToken);
        Task<ApiResponse<User>> Inactivate (User user, string authToken);

        Task<ApiResponseWithNoData> LogOut(string authToken);


    }


}
