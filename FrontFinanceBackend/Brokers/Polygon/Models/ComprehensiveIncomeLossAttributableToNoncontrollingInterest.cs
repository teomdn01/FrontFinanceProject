using System.Text.Json.Serialization; 
namespace Brokers.Polygon.Models{ 

    public class ComprehensiveIncomeLossAttributableToNoncontrollingInterest
    {
        [JsonPropertyName("value")]
        public int Value { get; set; }

        [JsonPropertyName("unit")]
        public string Unit { get; set; }

        [JsonPropertyName("label")]
        public string Label { get; set; }

        [JsonPropertyName("order")]
        public int Order { get; set; }
    }

}