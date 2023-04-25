using System.Collections.Generic;

namespace Brokers.Coinbase.Models
{
    public class ExchangeRatesModel
    {
        public string Currency { get; set; }
        public Dictionary<string, string> Rates { get; set; }
    }
}
