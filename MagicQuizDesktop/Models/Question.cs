using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicQuizDesktop.Models
{
    public class Question
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("question_text")]
        public string QuestionText { get; set; }

        [JsonProperty("topic_id")] 
        public int TopicId { get; set; }

        [JsonProperty("answers")]
        public List<Answer> Answers { get; set; }
    }
}
