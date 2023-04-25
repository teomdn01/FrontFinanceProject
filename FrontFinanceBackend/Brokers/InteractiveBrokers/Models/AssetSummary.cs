namespace Brokers.InteractiveBrokers.Models;

public class AssetSummary
{
    public string Symbol { get; set; }
    public string Name { get; set; }

    public Contract Contract { get; set; }

    public double MarketPrice { get; set; }
    public long LastTradeTimestamp { get; set; }

    public string Error { get; set; }
}