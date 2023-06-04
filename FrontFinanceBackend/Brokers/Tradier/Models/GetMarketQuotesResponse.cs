using System.Text.Json.Serialization;

namespace Brokers.Tradier.Models;

public class GetMarketQuotesResponse
{
    [JsonPropertyName("quotes")]
    public Quotes Quotes { get; set; }
}

public class Quote
{
    [JsonPropertyName("symbol")] public string Symbol { get; set; }

    [JsonPropertyName("description")] public string Description { get; set; }

    [JsonPropertyName("exch")] public string Exch { get; set; }

    [JsonPropertyName("type")] public string Type { get; set; }

    [JsonPropertyName("last")] public double Last { get; set; }

    [JsonPropertyName("change")] public double? Change { get; set; }

    [JsonPropertyName("volume")] public int Volume { get; set; }

    [JsonPropertyName("open")] public double? Open { get; set; }

    [JsonPropertyName("high")] public double? High { get; set; }

    [JsonPropertyName("low")] public double? Low { get; set; }

    [JsonPropertyName("close")] public double? Close { get; set; }

    [JsonPropertyName("bid")] public double Bid { get; set; }

    [JsonPropertyName("ask")] public double Ask { get; set; }

    [JsonPropertyName("change_percentage")]
    public double? ChangePercentage { get; set; }

    [JsonPropertyName("average_volume")] public int AverageVolume { get; set; }

    [JsonPropertyName("last_volume")] public int LastVolume { get; set; }

    [JsonPropertyName("trade_date")] public long TradeDate { get; set; }

    [JsonPropertyName("prevclose")] public double? Prevclose { get; set; }

    [JsonPropertyName("week_52_high")] public double Week52High { get; set; }

    [JsonPropertyName("week_52_low")] public double Week52Low { get; set; }

    [JsonPropertyName("bidsize")] public int Bidsize { get; set; }

    [JsonPropertyName("bidexch")] public string Bidexch { get; set; }

    [JsonPropertyName("bid_date")] public object BidDate { get; set; }

    [JsonPropertyName("asksize")] public int Asksize { get; set; }

    [JsonPropertyName("askexch")] public string Askexch { get; set; }

    [JsonPropertyName("ask_date")] public object AskDate { get; set; }

    [JsonPropertyName("root_symbols")] public string RootSymbols { get; set; }

    [JsonPropertyName("underlying")] public string Underlying { get; set; }

    [JsonPropertyName("strike")] public double? Strike { get; set; }

    [JsonPropertyName("open_interest")] public int? OpenInterest { get; set; }

    [JsonPropertyName("contract_size")] public int? ContractSize { get; set; }

    [JsonPropertyName("expiration_date")] public string ExpirationDate { get; set; }

    [JsonPropertyName("expiration_type")] public string ExpirationType { get; set; }

    [JsonPropertyName("option_type")] public string OptionType { get; set; }

    [JsonPropertyName("root_symbol")] public string RootSymbol { get; set; }
}

public class Quotes
{
    [JsonPropertyName("quote")] public List<Quote> Quote { get; set; }
}