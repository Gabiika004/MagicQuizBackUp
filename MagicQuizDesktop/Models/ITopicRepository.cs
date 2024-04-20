using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicQuizDesktop.Models
{
    public interface ITopicRepository
    {
        Task<ApiResponse<List<Topic>>> GetByAll(string authToken);
        Task<ApiResponse<Topic>> AddAsync(Topic topic, string authToken);

        // Kérdés frissítése
        Task<ApiResponse<Topic>> UpdateAsync(Topic topic, string authToken);

        // Kérdés törlése
        Task<ApiResponseWithNoData> DeleteAsync(int topicId, string authToken);

    }
}
