namespace Core.Models.MarketData;

public class StockMarketData
{
    public string Symbol { get; set; }
    public string Name { get; set; }
    public string? Exchange { get; set; }
    public double MarketPrice { get; set; }
    public double AskPrice { get; set; }
    public double BidPrice { get; set; }
    public string Currency { get; set; }
    public long LastTradeTimestamp { get; set; }

    public string Error { get; set; }
}