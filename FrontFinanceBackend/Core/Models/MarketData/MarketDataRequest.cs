namespace Core.Models.MarketData;

public class MarketDataRequest : BrokerBaseRequest
{
    public List<string> Symbols { get; set; }
}