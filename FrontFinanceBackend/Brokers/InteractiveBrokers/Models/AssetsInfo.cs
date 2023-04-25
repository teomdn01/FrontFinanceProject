namespace Brokers.InteractiveBrokers.Models;

// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
public class AssetsInfo
{
    public Dictionary<string, List<Asset>> Data { get; set; }
}

public class Asset
{
    public string Name { get; set; }
    public string ChineseName { get; set; }
    public string AssetClass { get; set; }
    public List<Contract> Contracts { get; set; }
}

public class Contract
{
    public int ConId { get; set; }
    public string Exchange { get; set; }
    public bool IsUS { get; set; }
}

