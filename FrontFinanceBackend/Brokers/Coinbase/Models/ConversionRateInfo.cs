using System.Text.Json.Serialization;

namespace Brokers.Coinbase.Models
{
    internal sealed class ConversionRateInfo
    {
        [JsonPropertyName("currency")]
        public string Currency { get; set; }
        [JsonPropertyName("rates")]
        public Dictionary<string, string> Rates { get; set; }
    }
}
