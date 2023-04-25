namespace Core.Models.MarketData;

public class StockMarketDataResponse
{
    public string Symbol { get; set; }
    public string Name { get; set; }
    public string? Exchange { get; set; }
    public double MarketPrice { get; set; }
    public long LastTradeTimestamp { get; set; }

    public string Error { get; set; }
}