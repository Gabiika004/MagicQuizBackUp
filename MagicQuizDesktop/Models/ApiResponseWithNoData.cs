using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MagicQuizDesktop.Models
{
    public class ApiResponseWithNoData
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }
        public bool Success => StatusCode == HttpStatusCode.OK || StatusCode == HttpStatusCode.NoContent;

        public static string ParseErrorMessage(string jsonResponse)
        {
            try
            {
                var errorObject = JsonConvert.DeserializeObject<dynamic>(jsonResponse);
                return errorObject?.message;
            }
            catch
            {
                return jsonResponse; // Ha nem sikerül a parse, visszaadja az eredeti választ
            }
        }
    }
}
