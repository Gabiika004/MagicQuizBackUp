using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MagicQuizDesktop.Models
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }


        // Hibaüzenet deszerializáló és formázó függvény
        public static string ParseErrorMessage(string jsonResponse)
        {
            try
            {
                var messageObject = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonResponse);
                if (messageObject.ContainsKey("message"))
                {
                    return messageObject["message"].ToString();
                }
            }
            catch (JsonException ex)
            {
                Debug.WriteLine("Hiba a JSON válasz deszerializálásakor: " + ex.Message);
                // Itt visszaadhatunk egy alapértelmezett hibaüzenetet, ha a JSON nem megfelelő
                return "A válasz formátuma nem megfelelő.";
            }
            return jsonResponse; // Alapértelmezett esetben visszaadjuk az eredeti JSON üzenetet.
        }
    }

}
