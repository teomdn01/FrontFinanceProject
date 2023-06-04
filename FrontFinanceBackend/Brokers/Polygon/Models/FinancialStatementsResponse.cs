using System.Text.Json.Serialization; 
using System.Collections.Generic; 
namespace Brokers.Polygon.Models{ 

    public class FinancialStatementsResponse
    {
        [JsonPropertyName("results")]
        public List<Result> Results { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("request_id")]
        public string RequestId { get; set; }

        [JsonPropertyName("next_url")]
        public string NextUrl { get; set; }
    }

}