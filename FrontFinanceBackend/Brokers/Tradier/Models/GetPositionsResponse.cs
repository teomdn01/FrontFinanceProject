namespace Brokers.Tradier.Models;


public class GetPositionsResponse
{
    public Positions positions { get; set; }
}

public class Position
{
    public decimal? cost_basis { get; set; }
    public DateTime date_acquired { get; set; }
    public int id { get; set; }
    public decimal quantity { get; set; }
    public string symbol { get; set; }
}

public class Positions
{
    public List<Position> position { get; set; }
}
