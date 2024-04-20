using MagicQuizDesktop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicQuizDesktop.Repositories
{
    public class TopicRepository : ITopicRepository
    {
        private readonly QuizApiService _apiService;

        public TopicRepository()
        {
            _apiService = new QuizApiService();
        }

        public async Task<ApiResponse<List<Topic>>> GetByAll(string authToken)
        {
            return await _apiService.GetAsync<List<Topic>>($"/topics", authToken);
        }

        public async Task<ApiResponse<Topic>> AddAsync(Topic topic, string authToken)
        {
            return await _apiService.PostAsync<Topic>("/topics", topic, authToken);
        }

        public async Task<ApiResponse<Topic>> UpdateAsync(Topic topic, string authToken)
        {
            return await _apiService.PutAsync<Topic>($"/topics/{topic.Id}", topic, authToken);
        }

        public async Task<ApiResponseWithNoData> DeleteAsync(int topicId, string authToken)
        {
            return await _apiService.DeleteAsync($"/topics/{topicId}", authToken);
        }
    }
}
