using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicQuizDesktop.Models
{
    public class Topic
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("topicname")]
        public string TopicName { get; set; }
    }
}
