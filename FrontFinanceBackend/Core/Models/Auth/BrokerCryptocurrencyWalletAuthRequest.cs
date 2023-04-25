namespace Core.Models;

public class BrokerCryptocurrencyWalletAuthRequest
{
    /// <summary>
    /// Optional symbol, e.g. for ERC-20 coins
    /// </summary>
    public string Symbol { get; set; }
    public string Nickname { get; set; }

    /// <summary>
    /// Blockchain to connect to
    /// </summary>
    public CryptocurrencyAddressType? CryptocurrencyAddressType { get; set; }

    /// <summary>
    /// Optional wallet type
    /// </summary>
    public CryptocurrencyWalletType? CryptocurrencyWalletType { get; set; }
}