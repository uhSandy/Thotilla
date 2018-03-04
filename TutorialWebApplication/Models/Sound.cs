using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace TutorialWebApplication.Models
{
    public class Sound
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "guId")]
        public String GuId { get; set; }

        [JsonProperty(PropertyName = "dateTime")]
        public DateTime DateTime { get; set; }

        [JsonProperty(PropertyName = "status")]
        public bool Status { get; set; }

        [JsonProperty(PropertyName = "response")]
        public String Response { get; set; }

        [JsonProperty(PropertyName = "responseDone")]
        public bool ResponseDone { get; set; }
    }
}