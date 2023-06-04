namespace Brokers.Alpaca.Configuration;

public class AlpacaConfig
{
    public bool EnableAlpacaPriceApi { get; set; }
    public int MaximumSymbolsPerRequest { get; set; }

    public bool IsLiveTradingEnvironment { get; set; } = true;
    public bool IsLiveDataEnvironment { get; set; } = true;

    public string AlpacaApiKey { get; set; }
    public string AlpacaApiSecret { get; set; }
    public string AlpacaClientId { get; set; }
    public string AlpacaClientSecret { get; set; }
    public string AlpacaOAuthRedirectLink { get; set; }
    public int? MinAmountOfSymbolsToUseParallelization { get; set; }

    public string[] ExcludingStockSymbols { get; set; } = Array.Empty<string>();
}