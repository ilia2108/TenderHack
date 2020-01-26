using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace TenderHack_Back
{
    [JsonObject]
    public class ReviewResponse
    {
        [JsonProperty("is_fake")]
        public bool IsFake { get; set; }

        [JsonProperty("usability")]
        public double Usability { get; set; }

        [JsonProperty("summary")]
        public string Summary { get; set; }

        [JsonProperty("tags")]
        public List<string> Tags { get; set; }

        [JsonProperty("polarity")]
        public double Polarity { get; set; }

        [JsonProperty("subjectivity")]
        public double Subjectivity { get; set; }
    }
}
