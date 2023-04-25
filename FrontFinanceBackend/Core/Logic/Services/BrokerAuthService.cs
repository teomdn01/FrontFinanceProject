using Brokers.Coinbase;
using Core.Contracts;
using Core.Models;
using Microsoft.Extensions.Logging;
using Org.Front.Core.Contracts.Models.Brokers;

namespace Core.Logic.Services;

public class BrokerAuthService : IBrokerAuthService
{
    private const string BrokerNotSupportedMessage = "Broker not supported.";

    private readonly ILogger<IBrokerAuthService> logger;
    private readonly ICoinbaseAuthService coinbaseAuthService;

    public BrokerAuthService(ILogger<BrokerAuthService> logger,
        ICoinbaseAuthService coinbaseAuthService)
    {
        this.coinbaseAuthService = coinbaseAuthService;
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

    public Task<BrokerAccountDetails> GetBrokerAccountDetails(BrokerType requestType, string authToken,
        string accountId, string innerIdByBroker,
        string deviceId)
    {
        throw new NotImplementedException();
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