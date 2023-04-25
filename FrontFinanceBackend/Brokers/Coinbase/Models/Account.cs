using System.Text.Json.Serialization;
using Brokers.Coinbase.Extensions;

namespace Brokers.Coinbase.Models
{
    public class Account
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        public string Name { get; set; }
        public bool Primary { get; set; }
        public string Type { get; set; }
        public Currency Currency { get; set; }
        public Balance Balance { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string Resource { get; set; }
        public string ResourcePath { get; set; }
        public bool Ready { get; set; }
        [JsonIgnore]
        public AccountType AccountType => Type.ParseCoinbaseStringToEnum<AccountType>();
    }

    public class Balance
    {
        [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
        public decimal? Amount { get; set; }
        public string Currency { get; set; }
    }

    public class Currency
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
    }

    public enum AccountType
    {
        Unknown,
        Wallet,
        Fiat,
        Vault
    }
}
