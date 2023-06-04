using System.Text.Json.Serialization;

namespace Brokers.Trading212.Models;

public class GetQuotesResponse
{
    [JsonPropertyName("request")]
    public Candle Request { get; set; }

    [JsonPropertyName("response")]
    public Response Response { get; set; }
}

public class Response
{
    [JsonPropertyName("candles")]
    public List<List<double>> Candles { get; set; }
}