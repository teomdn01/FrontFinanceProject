namespace Org.Front.Core.Contracts.Models.Brokers
{
    public class BrokerAccountTokens
    {
        public BrokerAccount Account { get; set; }

        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }
    }
}
