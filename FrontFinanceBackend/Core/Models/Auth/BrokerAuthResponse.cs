using Org.Front.Core.Contracts.Models.Brokers;

namespace Core.Models;

public class BrokerAuthResponse
{
    public BrokerAuthStatus Status { get; set; }

    public string ChallengeId { get; set; }

    public string ChallengeText { get; set; }

    public int? ChallengeExpiresInSeconds { get; set; }
    
    public string ErrorMessage { get; set; }
    
    public string DisplayMessage { get; set; }

    public string AccessToken { get; set; }

    public string RefreshToken { get; set; }
    
    public int? ExpiresInSeconds { get; set; }
    
    public int? RefreshTokenExpiresInSeconds { get; set; }

    public BrokerAccount Account { get; set; }

    public BrokerBrandInfo BrokerBrandInfo { get; set; }

    public IReadOnlyCollection<BrokerAccountTokens> BrokerAccountTokens { get; set; }
    
    public bool? RequiresReauthentication { get; set; }
    
    public string BrokerName => BrokerInfo?.Name;
    
    public BrokerFinancialInstitutionInfo BrokerInfo { get; set; }
    
    public IReadOnlyCollection<BrokerType> OtherConnectedBrokers { get; set; }
    
    public IReadOnlyCollection<CryptocurrencyWalletType> OtherConnectedWallets { get; set; }
    
    public IReadOnlyCollection<CryptocurrencyAddressType> OtherConnectedAddresses { get; set; }
}