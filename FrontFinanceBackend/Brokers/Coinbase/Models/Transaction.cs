using System;
using System.Text.Json.Serialization;
using Brokers.Coinbase.Extensions;
using Brokers.Coinbase.Models;

namespace Brokers.Coinbase.Models
{
    public class Transaction
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public Balance Amount { get; set; }
        public Balance NativeAmount { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public TransactionDetails Details { get; set; }
        public TransactionDestination To { get; set; }
        public TransactionNetwork Network { get; set; }
        public Trade Trade { get; set; }

        [JsonIgnore]
        public TransactionType TransactionType => Type.ParseCoinbaseStringToEnum<TransactionType>();
        [JsonIgnore]
        public TransactionStatus TransactionStatus => Status.ParseCoinbaseStringToEnum<TransactionStatus>();
    }

    public class TransactionDetails
    {
        public string Title { get; set; }
        public string Subtitle { get; set; }
    }

    public class TransactionNetwork
    {
        public string Status { get; set; }
        public string StatusDescription { get; set; }
        public string Hash { get; set; }
        public Balance TransactionFee { get; set; }
        public Balance TransactionAmount { get; set; }
        public long? Confirmations { get; set; }
    }

    public class TransactionDestination
    {
        public string Resource { get; set; }
        public string Address { get; set; }
        public string Currency { get; set; }
        public string AddressUrl { get; set; }
        public TransactionAddressInfo AddressInfo { get; set; }
    }

    public class TransactionAddressInfo
    {
        public string Address { get; set; }
    }


    public enum TransactionType
    {
        Unknown,
        Send,
        Request,
        Transfer,
        Buy,
        Sell,
        Trade,
        Interest,
        InflationReward,
        StakingReward,
        FiatDeposit,
        FiatWithdrawal,
        ExchangeDeposit,
        ExchangeWithdrawal,
        VaultWithdrawal
    }

    public enum TransactionStatus
    {
        Unknown,
        Pending,
        Completed,
        Failed,
        Expired,
        Canceled,
        WaitingForSignature,
        WaitingForClearing
    }
}
