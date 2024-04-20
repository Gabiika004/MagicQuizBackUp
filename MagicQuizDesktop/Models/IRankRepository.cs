using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicQuizDesktop.Models
{
    public interface IRankRepository
    {
        Task<ApiResponse<Rank>> PutScore(int userId, Rank rank, string authToken);
        Task<ApiResponse<List<Rank>>> GetRanks(string authToken);
        Task<ApiResponse<Rank>> GetScore(int userId, string authToken);
        Task<ApiResponseWithNoData> ResetRanks(string authToken);
    }
}
