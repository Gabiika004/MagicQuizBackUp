using MagicQuizDesktop.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace MagicQuizDesktop.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly QuizApiService _apiService;

        public UserRepository()
        {
            _apiService = new QuizApiService();
        }

        public async Task<ApiResponse<LoginUser>> AuthenticateUser(string email, string password)
        {
            var data = new { email = email, password = password };
            return await _apiService.PostAsync<LoginUser>("/login", data);
        }

        public async Task<ApiResponse<User>> GetById(int id, string authToken)
        {
            return await _apiService.GetAsync<User>($"/users/{id}", authToken);
        }

        public async Task<ApiResponse<List<User>>> GetByAll(string authToken)
        {
            return await _apiService.GetAsync<List<User>>("/users", authToken);
        }

        public async Task<ApiResponse<User>> UpdateUser(User user, string authToken)
        {
            return await _apiService.PutAsync<User>($"/users/{user.Id}", user, authToken);
        }

        public async Task<ApiResponse<User>> Inactivate(User user, string authToken)
        {
            return await _apiService.PostAsync<User>($"/users/inactivate/{user.Id}", user, authToken);
        }
        public async Task<ApiResponseWithNoData> LogOut(string authToken)
        {
            return await _apiService.PostAsyncWithNoData("/logout", authToken);
        }

    }

}
