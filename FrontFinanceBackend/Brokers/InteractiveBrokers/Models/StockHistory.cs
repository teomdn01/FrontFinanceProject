namespace Brokers.InteractiveBrokers.Models;

// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
public class Data
{
    public double o { get; set; }
    public double c { get; set; }
    public double h { get; set; }
    public double l { get; set; }
    public int v { get; set; }
    public long t { get; set; }
}

public class StockHistory
{
    public string serverId { get; set; }
    public string symbol { get; set; }
    public string text { get; set; }
    public int priceFactor { get; set; }
    public string startTime { get; set; }
    public string high { get; set; }
    public string low { get; set; }
    public string timePeriod { get; set; }
    public int barLength { get; set; }
    public string mdAvailability { get; set; }
    public int mktDataDelay { get; set; }
    public bool outsideRth { get; set; }
    public int volumeFactor { get; set; }
    public int priceDisplayRule { get; set; }
    public string priceDisplayValue { get; set; }
    public bool negativeCapable { get; set; }
    public int messageVersion { get; set; }
    public List<Data> Data { get; set; }
    public int points { get; set; }
    public int travelTime { get; set; }
}

