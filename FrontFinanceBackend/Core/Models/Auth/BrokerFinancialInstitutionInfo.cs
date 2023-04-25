namespace Org.Front.Core.Contracts.Models.Brokers
{
    public class BrokerFinancialInstitutionInfo
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool SupportsStocks { get; set; }
        public bool SupportsNfts { get; set; }
        public bool SupportsCryptocurrencies { get; set; }
    }
}
