namespace Core.Models.AccountInfo;

public class BrokerPositions
{
    public BrokerRequestStatus Status { get; set; }
    public List<Position> Positions { get; set; }
}

public class Position
{
    /// <summary>
    /// GUID list of related domains
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Symbol of the asset
    /// </summary>
    public string Symbol { get; set; }

    /// <summary>
    /// Amount of the asset
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// The total original value (or purchase price) of the asset
    /// </summary>
    public decimal? CostBasis { get; set; }
}