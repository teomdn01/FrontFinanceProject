using System.Text.Json.Serialization;

namespace Brokers.Trading212.Models;


public class AuthResponse
{
    [JsonPropertyName("tradingType")] public string TradingType { get; set; }

    [JsonPropertyName("accountId")] public int AccountId { get; set; }

    [JsonPropertyName("accountSession")] public string AccountSession { get; set; }

    [JsonPropertyName("customerSession")] public string CustomerSession { get; set; }

    [JsonPropertyName("email")] public string Email { get; set; }

    [JsonPropertyName("rememberMeCookie")] public RememberMeCookie RememberMeCookie { get; set; }

    [JsonPropertyName("subSystem")] public string SubSystem { get; set; }

    [JsonPropertyName("customerId")] public string CustomerId { get; set; }

    [JsonPropertyName("backupCode")] public object BackupCode { get; set; }

    [JsonPropertyName("loginToken")] public string LoginToken { get; set; }

    [JsonPropertyName("sessionCookieName")]
    public string SessionCookieName { get; set; }

    [JsonPropertyName("customerCookieName")]
    public string CustomerCookieName { get; set; }

    [JsonPropertyName("customer")] public Customer Customer { get; set; }

    [JsonPropertyName("account")] public Account Account { get; set; }

    [JsonPropertyName("serverTimestamp")] public string ServerTimestamp { get; set; }
}

public class Account
{
    [JsonPropertyName("id")] public int Id { get; set; }

    [JsonPropertyName("customerId")] public int CustomerId { get; set; }

    [JsonPropertyName("type")] public string Type { get; set; }

    [JsonPropertyName("createdDate")] public DateTime CreatedDate { get; set; }

    [JsonPropertyName("lastSwitchedDate")] public object LastSwitchedDate { get; set; }

    [JsonPropertyName("tradingType")] public string TradingType { get; set; }

    [JsonPropertyName("status")] public string Status { get; set; }

    [JsonPropertyName("registerSource")] public string RegisterSource { get; set; }

    [JsonPropertyName("currencyCode")] public string CurrencyCode { get; set; }

    [JsonPropertyName("readyToTrade")] public bool ReadyToTrade { get; set; }
}

public class Customer
{
    [JsonPropertyName("id")] public int Id { get; set; }

    [JsonPropertyName("uuid")] public string Uuid { get; set; }

    [JsonPropertyName("email")] public string Email { get; set; }

    [JsonPropertyName("dealer")] public string Dealer { get; set; }

    [JsonPropertyName("lang")] public string Lang { get; set; }

    [JsonPropertyName("timezone")] public string Timezone { get; set; }

    [JsonPropertyName("registerDate")] public DateTime RegisterDate { get; set; }
}

public class RememberMeCookie
{
    [JsonPropertyName("path")] public string Path { get; set; }

    [JsonPropertyName("maxAge")] public int MaxAge { get; set; }

    [JsonPropertyName("domain")] public string Domain { get; set; }

    [JsonPropertyName("value")] public string Value { get; set; }
}