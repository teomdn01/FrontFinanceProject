using System.Text.Json.Serialization;

namespace Brokers.Coinbase.Models
{
    internal sealed class PriceInfo
    {
        [JsonPropertyName("amount")]
        public string Amount { get; set; }
        [JsonPropertyName("currency")]
        public string Currency { get; set; }
    }
}
