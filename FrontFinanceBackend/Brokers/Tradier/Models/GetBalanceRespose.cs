using System.Text.Json.Serialization;

namespace Brokers.Tradier.Models;

public class GetBalanceRespose
{
    [JsonPropertyName("balances")] public Balances Balances { get; set; }
}

public class Balances
{
    [JsonPropertyName("option_short_value")]
    public int OptionShortValue { get; set; }

    [JsonPropertyName("total_equity")] public double TotalEquity { get; set; }

    [JsonPropertyName("account_number")] public string AccountNumber { get; set; }

    [JsonPropertyName("account_type")] public string AccountType { get; set; }

    [JsonPropertyName("close_pl")] public double ClosePl { get; set; }

    [JsonPropertyName("current_requirement")]
    public double CurrentRequirement { get; set; }

    [JsonPropertyName("equity")] public int Equity { get; set; }

    [JsonPropertyName("long_market_value")]
    public double LongMarketValue { get; set; }

    [JsonPropertyName("market_value")] public double MarketValue { get; set; }

    [JsonPropertyName("open_pl")] public double OpenPl { get; set; }

    [JsonPropertyName("option_long_value")]
    public double OptionLongValue { get; set; }

    [JsonPropertyName("option_requirement")]
    public int OptionRequirement { get; set; }

    [JsonPropertyName("pending_orders_count")]
    public int PendingOrdersCount { get; set; }

    [JsonPropertyName("short_market_value")]
    public int ShortMarketValue { get; set; }

    [JsonPropertyName("stock_long_value")] public double StockLongValue { get; set; }

    [JsonPropertyName("total_cash")] public double TotalCash { get; set; }

    [JsonPropertyName("uncleared_funds")] public int UnclearedFunds { get; set; }

    [JsonPropertyName("pending_cash")] public int PendingCash { get; set; }

    [JsonPropertyName("margin")] public Margin Margin { get; set; }

    [JsonPropertyName("cash")] public Cash Cash { get; set; }

    [JsonPropertyName("pdt")] public Pdt Pdt { get; set; }
}

public class Cash
{
    [JsonPropertyName("cash_available")] public double CashAvailable { get; set; }

    [JsonPropertyName("sweep")] public int Sweep { get; set; }

    [JsonPropertyName("unsettled_funds")] public double UnsettledFunds { get; set; }
}

public class Margin
{
    [JsonPropertyName("fed_call")] public int FedCall { get; set; }

    [JsonPropertyName("maintenance_call")] public int MaintenanceCall { get; set; }

    [JsonPropertyName("option_buying_power")]
    public double OptionBuyingPower { get; set; }

    [JsonPropertyName("stock_buying_power")]
    public double StockBuyingPower { get; set; }

    [JsonPropertyName("stock_short_value")]
    public int StockShortValue { get; set; }

    [JsonPropertyName("sweep")] public int Sweep { get; set; }
}

public class Pdt
{
    [JsonPropertyName("fed_call")] public int FedCall { get; set; }

    [JsonPropertyName("maintenance_call")] public int MaintenanceCall { get; set; }

    [JsonPropertyName("option_buying_power")]
    public double OptionBuyingPower { get; set; }

    [JsonPropertyName("stock_buying_power")]
    public double StockBuyingPower { get; set; }

    [JsonPropertyName("stock_short_value")]
    public int StockShortValue { get; set; }
}