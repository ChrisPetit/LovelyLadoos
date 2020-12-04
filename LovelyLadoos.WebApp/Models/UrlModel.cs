using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LovelyLadoos.WebApp.Models
{
    public class UrlModel
    {
        public UrlModel(string predictionKeyHeader)
        {
            PredictionKeyHeader = predictionKeyHeader;
            ContentTypeHeader = "application/json";
        }
        public string ImageUrl { get; set; }
        public string PredictionKeyHeader { get; set; }
        public string ContentTypeHeader { get; set; }
        [JsonPropertyName("Url")]
        public string Body { get; set; }
    }
}
