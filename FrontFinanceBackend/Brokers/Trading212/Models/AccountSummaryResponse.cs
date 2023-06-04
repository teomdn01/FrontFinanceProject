using System.Text.Json.Serialization;

namespace Brokers.Trading212.Models;


public class AccountSummaryResponse
{
    [JsonPropertyName("cash")] public Cash Cash { get; set; }

    [JsonPropertyName("open")] public Open Open { get; set; }

    [JsonPropertyName("orders")] public Orders Orders { get; set; }

    [JsonPropertyName("valueOrders")] public ValueOrders ValueOrders { get; set; }
}

public class Cash
{
    [JsonPropertyName("free")] public double Free { get; set; }

    [JsonPropertyName("total")] public double Total { get; set; }

    [JsonPropertyName("interest")] public double Interest { get; set; }

    [JsonPropertyName("indicator")] public int Indicator { get; set; }

    [JsonPropertyName("commission")] public double Commission { get; set; }

    [JsonPropertyName("cash")] public double CashLiquid { get; set; }

    [JsonPropertyName("ppl")] public int Ppl { get; set; }

    [JsonPropertyName("result")] public double Result { get; set; }

    [JsonPropertyName("spreadBack")] public double SpreadBack { get; set; }

    [JsonPropertyName("nonRefundable")] public double NonRefundable { get; set; }

    [JsonPropertyName("dividend")] public double Dividend { get; set; }

    [JsonPropertyName("stockInvestment")] public double StockInvestment { get; set; }

    [JsonPropertyName("freeForStocks")] public double FreeForStocks { get; set; }

    [JsonPropertyName("totalCashForWithdraw")]
    public double TotalCashForWithdraw { get; set; }

    [JsonPropertyName("blockedForStocks")] public double BlockedForStocks { get; set; }

    [JsonPropertyName("pieCash")] public int PieCash { get; set; }
}

public class Open
{
    [JsonPropertyName("unfilteredCount")] public int UnfilteredCount { get; set; }

    [JsonPropertyName("items")] public List<object> Items { get; set; }
}

public class Orders
{
    [JsonPropertyName("unfilteredCount")] public int UnfilteredCount { get; set; }

    [JsonPropertyName("items")] public List<object> Items { get; set; }
}

public class ValueOrders
{
    [JsonPropertyName("unfilteredCount")] public int UnfilteredCount { get; set; }

    [JsonPropertyName("items")] public List<object> Items { get; set; }
}