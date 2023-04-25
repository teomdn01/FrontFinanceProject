using System;
using System.Text.Json.Serialization;
using Brokers.Coinbase.Extensions;

namespace Brokers.Coinbase.Models
{
    public class PaymentMethod
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Currency { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool InstantBuy { get; set; }
        public bool InstantSell { get; set; }
        public bool AllowBuy { get; set; }
        public bool Verified { get; set; }
        [JsonIgnore]
        public PaymentMethodType PaymentMethodType => Type.ParseCoinbaseStringToEnum<PaymentMethodType>();
    }

    public enum PaymentMethodType
    {
        Unknown,
        AchBankAccount,
        SepaBankAccount,
        IdealBankAccount,
        FiatAccount,
        BankWire,
        CreditCard,
        Secure3dCard,
        EftBankAccount,
        Interac,
        ApplePay
    }
}
