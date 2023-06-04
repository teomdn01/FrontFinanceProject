using System.Net;
using Alpaca.Markets;
using Brokers.Alpaca.Configuration;
using Brokers.Alpaca.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Org.Front.Core.Contracts.Exceptions;
using Org.Front.Core.Contracts.Helpers;
using Org.Front.Core.Contracts.Models.Brokers;

namespace Brokers.Alpaca.Services;

public class BaseAlpacaService
{
    protected readonly AlpacaConfig AlpacaConfig;
    protected readonly ILogger Logger;

    public BaseAlpacaService(IOptions<AlpacaConfig> config, ILogger logger)
    {
        AlpacaConfig = config.Value ?? throw new ArgumentNullException(nameof(config));
        Logger = logger;
    }

    protected async Task<T> ExecuteAndHandleExceptions<T>(Func<Task<T>> action, string operationName)
    {
        try
        {
            return await action();
        }
        catch (RestClientErrorException e)
        {
            var statusCode = HttpStatusCode.BadRequest;
            if (Enum.IsDefined(typeof(HttpStatusCode), e.ErrorCode))
            {
                statusCode = (HttpStatusCode)e.ErrorCode;
            }

            throw BrokerErrorResponseHelper.CreateBrokerException(statusCode, e.Message, e.Message, operationName,
                "Alpaca", Logger);
        }
        catch (RequestValidationException e)
        {
            throw BrokerErrorResponseHelper.CreateBrokerException(
                HttpStatusCode.BadRequest,
                e.Message,
                e.Message,
                operationName,
                "Alpaca",
                Logger);
        }
        catch (Exception e)
        {
            if (e.Message.Contains("\"PAPER_ONLY\""))
            {
                var error = "Failed to connect your brokerage account. Paper accounts not supported.";
                Logger.LogInformation(e, error);
                throw new ApiCallDetailedException(error, error);
            }
            else
            {
                var genericMessage = string.Format(BrokerErrorMessages.GenericOperationName, operationName, "Alpaca");
                var detailedMessage = $"{genericMessage} Error message: {e.Message}";
                Logger.LogError(e, genericMessage);
                throw new ApiCallDetailedException(detailedMessage, genericMessage);
            }
        }
    }


    protected IAlpacaTradingClient CreateAlpacaClient(string token)
    {
        var environment = AlpacaConfig.IsLiveTradingEnvironment ? Environments.Live : Environments.Paper;
        return environment.GetAlpacaTradingClient(new OAuthKey(token));
    }
    
    protected IAlpacaDataClient CreateAlpacaDataClient(string keyToken)
    {
        //TODO: Split token to api key and secret
        var environment = AlpacaConfig.IsLiveTradingEnvironment ? Environments.Live : Environments.Paper;
        return environment.GetAlpacaDataClient(new SecretKey("AKRYX1QT8KS5542V3ZJT", "aMBacFhzkZVECXUZs5djugAuYGBVc4hNvywvsF8v"));
    }

    protected async Task<IAccount> GetAccount(string token)
    {
        return await ExecuteAndHandleExceptions(async () =>
        {
            using var client = CreateAlpacaClient(token);
            return await client.GetAccountAsync();
        }, BrokerOperationNames.GetAccount);
    }

    protected static string CreateTokenWithAccountId(TokenModel model)
    {
        if (string.IsNullOrEmpty(model.AccountId) || string.IsNullOrEmpty(model.Token))
        {
            throw new ApiCallException("Alpaca token data is corrupted");
        }

        return $"{model.Token}|{model.AccountId}";
    }

    protected static TokenModel ParseToken(string accessToken)
    {
        if (!accessToken.Contains("|"))
        {
            return new TokenModel()
            {
                Token = accessToken
            };
        }

        var tokenData = accessToken.Split("|");
        return new TokenModel()
        {
            Token = tokenData[0],
            AccountId = tokenData[1]
        };
    }
}