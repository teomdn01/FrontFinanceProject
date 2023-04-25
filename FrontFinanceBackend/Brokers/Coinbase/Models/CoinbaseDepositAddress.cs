namespace Brokers.Coinbase.Models
{
    public class CoinbaseDepositAddress
    {
        public string Id { get; set; }
        public string Address { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Network { get; set; }
    }
}
