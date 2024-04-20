using MagicQuizDesktop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicQuizDesktop.Repositories
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly QuizApiService _apiService;

        public QuestionRepository()
        {
            _apiService = new QuizApiService();
        }

        public async Task<ApiResponse<List<Question>>> GetByAll(string authToken)
        {
            return await _apiService.GetAsync<List<Question>>($"/questions", authToken);
        }

        public async Task<ApiResponse<Question>> GetByIdAsync(int questionId, string authToken)
        {
            return await _apiService.GetAsync<Question>($"/questions/{questionId}", authToken);
        }

        public async Task<ApiResponse<Question>> AddAsync(Question question, string authToken)
        {
            return await _apiService.PostAsync<Question>("/questions", question, authToken);
        }

        public async Task<ApiResponse<Question>> UpdateAsync(Question question, string authToken)
        {
            return await _apiService.PutAsync<Question>($"/questions/{question.Id}", question, authToken);
        }

        public async Task<ApiResponseWithNoData> DeleteAsync(int questionId, string authToken)
        {
            return await _apiService.DeleteAsync($"/questions/{questionId}", authToken);
        }
    }
}
