namespace Core.Models.AccountInfo;

public class BrokerBalance
{
    public BrokerRequestStatus Status { get; set; }
    /// <summary>
    /// BuyingPower indicates total amount of money the user can spend for buying stock. Always includes cash and
    /// can also include margin
    /// </summary>
    public decimal? BuyingPower { get; set; }

    /// <summary>
    /// Indicates total value of all stocks in portfolio
    /// </summary>
    public double PortfolioStockValue { get; set; }

    /// <summary>
    /// Indicates total value of all cryptocurrencies in portfolio
    /// </summary>
    public decimal? PortfolioCryptocurrenciesValue { get; set; }

    /// <summary>
    /// Indicates total value of all NFTs in portfolio
    /// </summary>
    public decimal? PortfolioNftsValue { get; set; }

    /// <summary>
    /// Account cash indicates total amount of money in USD the user has on his/her account
    /// </summary>
    public double Cash { get; set; }

    public decimal? OtherAssetsValue { get; set; }

    /// <summary>
    /// The list of all asset balances of account 
    /// </summary>
    public IReadOnlyCollection<BrokerFiatBalance> Balances { get; set; }

    /// <summary>
    /// Total USD value of all currencies
    /// </summary>
    public decimal? TotalCashUsdValue { get; set; }

    /// <summary>
    /// Total USD value of all Buying Power
    /// </summary>
    public decimal? TotalBuyingPowerUsdValue { get; set; }
}

public class BrokerFiatBalance
{
    /// <summary>
    /// Account balance currency
    /// </summary>
    public string Symbol { get; set; }

    /// <summary>
    /// BuyingPower indicates total amount of money the user can spend for buying stock. Always includes cash and
    /// can also include margin
    /// </summary>
    public decimal? BuyingPower { get; set; }

    /// <summary>
    /// BuyingPower indicates total amount of money the user can spend for buying crypto.
    /// </summary>
    public decimal? CryptoBuyingPower { get; set; }

    /// <summary>
    /// Account cash indicates total amount of money
    /// </summary>
    public decimal? Cash { get; set; }
}