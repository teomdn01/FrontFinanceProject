using System.Text.Json.Serialization;

namespace Brokers.Trading212.Models;

public class GetQuotesRequest
{
    [JsonPropertyName("candles")]
    public List<Candle> Candles { get; set; }
}

public class Candle
{
    [JsonPropertyName("period")]
    public string Period { get; set; }

    [JsonPropertyName("size")]
    public int Size { get; set; }

    [JsonPropertyName("ticker")]
    public string Ticker { get; set; }

    [JsonPropertyName("useAskPrice")]
    public bool UseAskPrice { get; set; }
}
