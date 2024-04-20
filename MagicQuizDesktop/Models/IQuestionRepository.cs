using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicQuizDesktop.Models
{
    public interface IQuestionRepository
    {
        // Lekéri az összes kérdést
        Task<ApiResponse<List<Question>>> GetByAll(string authToken);

        // Új kérdés hozzáadása
        Task<ApiResponse<Question>> AddAsync(Question question, string authToken);

        // Kérdés frissítése
        Task<ApiResponse<Question>> UpdateAsync(Question question, string authToken);

        // Kérdés törlése
        Task<ApiResponseWithNoData> DeleteAsync(int questionId, string authToken);

    }
}
