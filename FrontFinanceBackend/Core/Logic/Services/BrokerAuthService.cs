using Brokers.Coinbase;
using Core.Contracts;
using Core.Contracts.Adapters.Alpaca;
using Core.Contracts.Adapters.Freedom;
using Core.Contracts.Adapters.InteractiveBrokers;
using Core.Contracts.Adapters.Tradier;
using Core.Contracts.Adapters.Trading212;
using Core.Models;
using Core.Models.AccountInfo;
using Core.Models.Auth;
using Microsoft.Extensions.Logging;
using Org.Front.Core.Contracts.Models.Brokers;

namespace Core.Logic.Services;

public class BrokerAuthService : IBrokerAuthService
{
    private const string BrokerNotSupportedMessage = "Broker not supported.";

    private readonly ILogger<IBrokerAuthService> logger;
    private readonly ICoinbaseAuthService coinbaseAuthService;
    private readonly IInteractiveBrokersAuthService interactiveBrokersAuthService;
    private readonly IFreedomAuthService freedomAuthService;
    private readonly ITradierAuthService tradierAuthService;
    private readonly IAlpacaAuthService alpacaAuthService;
    private readonly ITrading212AuthService trading212AuthService;

    public BrokerAuthService(ILogger<BrokerAuthService> logger,
        ICoinbaseAuthService coinbaseAuthService,
        IFreedomAuthService freedomAuthService, 
        IInteractiveBrokersAuthService interactiveBrokersAuthService, 
        ITradierAuthService tradierAuthService, 
        IAlpacaAuthService alpacaAuthService, 
        ITrading212AuthService trading212AuthService)
    {
        this.coinbaseAuthService = coinbaseAuthService;
        this.freedomAuthService = freedomAuthService;
        this.interactiveBrokersAuthService = interactiveBrokersAuthService;
        this.tradierAuthService = tradierAuthService;
        this.alpacaAuthService = alpacaAuthService;
        this.trading212AuthService = trading212AuthService;
        this.logger = logger;
    }

    public async Task<BrokerAuthResponse> GetBrokerAuthResponse(BrokerAuthRequest request, string deviceId)
    {
        IReadOnlyCollection<string> currencies;
        BrokerAuthResponse response;
        switch (request.Type)
        {
            case BrokerType.Coinbase:
                response = await coinbaseAuthService.Authenticate(request);
                break;
            case BrokerType.Freedom:
                response = await freedomAuthService.Authenticate(request.Username, request.Password);
                break;
            case BrokerType.Tradier:
                response = tradierAuthService.Authenticate("", "");
                break;
            case BrokerType.Alpaca:
                response = await alpacaAuthService.Authenticate(request);
                break;
            case BrokerType.Trading212:
                response = await trading212AuthService.Authenticate(request.Username, request.Password);
                break;
            default:
                logger.LogError("GetBrokerAuthLink not supported for the broker {BrokerType}", request.Type);
                throw new NotSupportedException(BrokerNotSupportedMessage); 
        }

        return response;
    }

    public async Task<BrokerAuthPromptResponse> GetBrokerAuthLink(BrokerType brokerType,
        BrokerGetAuthenticationLinkRequest request, string userId)
    {
        if (!string.IsNullOrEmpty(request.RedirectLink))
        {
            request.RedirectLink = request.RedirectLink.Trim();
            ValidateRedirectLink(request.RedirectLink);
        }

        BrokerAuthPromptResponse response;
        switch (brokerType)
        {
            case BrokerType.Coinbase:
                response = coinbaseAuthService.GetAuthLink(request.RedirectLink, request.EnableCryptocurrencyTransfers);
                break;
            case BrokerType.Alpaca:
                response = alpacaAuthService.GetLinkToken(request.RedirectLink);
                break;
            default:
                logger.LogError("GetBrokerAuthLink not supported for the broker {BrokerType}", brokerType);
                throw new NotSupportedException(BrokerNotSupportedMessage);
        }

        return response;
    }

    public string GetExternalBrokerAccountId(BrokerType requestType, string authToken)
    {
        throw new NotImplementedException();
    }

    public async Task<BrokerAuthResponse> RefreshToken(
        BrokerRefreshTokenRequest request)
    {
        BrokerAuthResponse response;
        var createBrokerageAccounts = false;

        switch (request.Type)
        {
            case BrokerType.Coinbase:
                response = await coinbaseAuthService.RefreshAccessToken(request.RefreshToken);
                break;
            default:
                logger.LogError("Refresh not supported for the broker {BrokerType}", request.Type);
                throw new NotSupportedException(BrokerNotSupportedMessage);
        }

        return response;
    }

    public Task<string> GetAccountName(string deviceId, BrokerType brokerType, string authToken)
    {
        throw new NotImplementedException();
    }
    
    //TODO: implement for each broker with account information details
    public Task<BrokerAccountDetails> GetBrokerAccountDetails(BrokerType requestType, string authToken,
        string accountId, string innerIdByBroker,
        string deviceId)
    {
        throw new NotImplementedException();
    }

    //TODO: refactor
    public async Task<BrokerPositions> GetPositions(BrokerBaseRequest request)
    {
        BrokerPositions response = null;
        switch (request.Type)
        {
            // case BrokerType.Coinbase:
            //     response = await coinbaseAuthService.GetPositions();
            //     break;
            // case BrokerType.InteractiveBrokers:
            //     response = await coinbaseAuthService.GetPositions();
            //     break;
            case BrokerType.Freedom:
                response = await freedomAuthService.GetPositions("e8a25afdaf2fafbcf099744f5f24640e", "85b4e174f7efce916b9fa1099022f2cf6c3412ad");
                break;
            case BrokerType.Tradier:
                response = await tradierAuthService.GetPositions(request);
                break;
            case BrokerType.Alpaca:
                response = await alpacaAuthService.GetPositions(request.AuthToken);
                break;
            case BrokerType.Trading212:
                response = await trading212AuthService.GetPositions(request);
                break;
            default:
                logger.LogError("Refresh not supported for the broker {BrokerType}", request.Type);
                throw new NotSupportedException(BrokerNotSupportedMessage);
        }

        return response;
    }

    public async Task<BrokerBalance> GetBalance(BrokerBaseRequest request)
    {
        BrokerBalance response;
        switch (request.Type)
        {
            // case BrokerType.Coinbase:
            //     response = await coinbaseAuthService.GetPositions();
            //     break;
            // case BrokerType.InteractiveBrokers:
            //     response = await coinbaseAuthService.GetPositions();
            //     break;
            //TODO: Add encryption to apiKey/secret on frontend, and decryption on the server (Request.Token) to allow sending such requests 
            case BrokerType.Freedom:    
                response = await freedomAuthService.GetBalance("e8a25afdaf2fafbcf099744f5f24640e", "85b4e174f7efce916b9fa1099022f2cf6c3412ad");
                break;
            case BrokerType.Tradier:
                response = await tradierAuthService.GetBalance(request);
                break;
            case BrokerType.Alpaca:
                response = await alpacaAuthService.GetBalance(request.AuthToken);
                break;
            case BrokerType.Trading212:
                response = await trading212AuthService.GetBalance(request);
                break;
            default:
                logger.LogError("Refresh not supported for the broker {BrokerType}", request.Type);
                throw new NotSupportedException(BrokerNotSupportedMessage);
        }

        return response;
    }

    private void ValidateRedirectLink(string redirectLink)
    {
        Uri redirectUri;
        try
        {
            redirectUri = new Uri(redirectLink);
        }
        catch (UriFormatException e)
        {
            var message = "Wrong redirect URL was passed.";
            logger.LogError(e, "{Message}: {RedirectUrl}", message, redirectLink);
            throw new InvalidOperationException(message);
        }
    }
}