namespace Core.Models.MarketData;

public class StockMarketDataResponse
{
    public string BrokerType { get; set; }
    public List<StockMarketData> Data { get; set; }
    public BrokerRequestStatus Status { get; set; }
}
