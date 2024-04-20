using MagicQuizDesktop.Models;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicQuizDesktop.Repositories
{
    public class RankRepository : IRankRepository
    {
        private readonly QuizApiService _apiService;

        public RankRepository()
        {
            _apiService = new QuizApiService();
        }

        public async Task<ApiResponse<Rank>> PutScore(int userId, Rank rank, string authToken)
        {
            return await _apiService.PutAsync<Rank>($"/user-ranks/{userId}", rank, authToken);
        }   
        public async Task<ApiResponse<Rank>> GetScore(int userId, string authToken)
        {
            return await _apiService.GetAsync<Rank>($"/user-ranks/{userId}", authToken);
 
        }
        public async Task<ApiResponse<List<Rank>>> GetRanks(string authToken)
        {
            return await _apiService.GetAsync<List<Rank>>($"/user-ranks", authToken);
        }

        public async Task<ApiResponseWithNoData> ResetRanks(string authToken)
        {
            return await _apiService.PostAsyncWithNoData("/user-ranks-reset", authToken);
        }

    }
}
