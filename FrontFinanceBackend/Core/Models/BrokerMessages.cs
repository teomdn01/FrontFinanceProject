namespace Org.Front.Core.Contracts.Models.Brokers
{
    public static class BrokerErrorMessages
    {
        public const string BrokerNotSupportedMessage = "Integration is not supported.";
        public const string GenericOperationName = "Could not {0} using {1}.";
        public const string CouldNotConnectBrokerTryLater = "Could not connect to {0}. Please try again later.";
        public const string CouldNotConnectNoAccount = "Could not connect to {0}. No account was found.";
        public const string CouldNotUpdatePortfolio = "Could not get portfolio from {0}.";
        public const string CouldNotFindAccount = "Could not find {0} account.";
        public const string NotAuthorized = "Not authorized.";
        public const string NoStockAvailable = "No assets available to sell using {0}.";
        public const string CouldNotSell = "Could not place sell order using {0}.";
        public const string CouldNotPlaceOrder = "Could not place order using {0}.";
        public const string CouldNotSellNoPrice = "Could not validate sell order. Please try again later.";
        public const string CouldNotUpdateAfterConnecting = "Could not update a broker's portfolio after connecting the broker.";
        public const string BalanceNotEnoughMessage = "Could not place buy order - buying power is not enough.";
        public const string ExpenseNotEnoughMessage = "Could not place buy order - target expense amount cannot be less than {0}";
        public const string NegativeAmountMessage = "Amount can not be less or equals zero.";
        public const string NotValidSymbol = "Executing order for provided symbol {0} is not possible.";
        public const string NotValidOrderType = "Cannot execute order with type: {0}.";
        public const string BuyOrderOnlyOneBrokerAllowed = "Purchase can be done when one broker provided only.";
        public const string OnlyOneBrokerAllowed = "Operation can be done when one broker provided only.";
        public const string CouldNotSellNotEnoughStock = "Could not execute sell order - portfolio doesn't have sufficient amount of {0}";
        public const string FractionalQuantitiesNotSupported = "{0} does not support fractional quantities. Make sure that the requested amount is a whole number.";
        public const string CouldNotInitiateCanceling = "Could not initiate {0} order canceling.";
        public const string MfaCodeNotCorrect = "Provided code is not correct.";
        public const string CouldNotGetAccountDetails = "Could not get additional account details.";
        public const string BrokerOrderPlaceTypeNotProvided = "Order Place Type not provided.";
        public const string BrokerOrderPlaceTypeNotSupported = "Order Place Type not supported.";
        public const string BrokerOrderTimeInForceTypeNotSupported = "Order Time In Force Type not supported.";
        public const string BrokerMarginOrderNotSupported = "Margin Order not supported.";
        public const string BrokerIsolatedMarginOrderNotSupported = "Isolated Margin Order not supported.";
        public const string BrokerCrossMarginOrderNotSupported = "Cross Margin Order not supported.";
        public const string BrokerFractionalSharesOrderNotSupported = "Fractional Shares Order not supported.";
        public const string BrokerCryptocurrencyPaymentOrderNotSupported = "Cryptocurrency Payment Order not supported.";
        public const string BrokerFiatPaymentOrderNotSupported = "Fiat Payment Order not supported.";
        public const string BrokerStockTradingNotSupported = "Stock Trading not supported.";
        public const string BrokerCryptoTradingNotSupported = "Cryptocurrency Trading not supported.";
        public const string BrokerPaymentSymbolNotSupported = "Payment symbol {0} is not supported.";
        public const string ExtendedTradingHoursNotSupported = "Extended trading hours are not supported for the selected order type.";
        public const string PlacingInPaymentAmountNotSupported = "Placing orders in payment amount is not supported for the {0} order type.";
        public const string AmountInPaymentNotProvided = "Amount in payment is not provided.";
        public const string AmountNotProvided = "Amount in shares is not provided.";
        public const string CryptoTransferFailed = "Crypto transfer failed {0}.";
        public const string TransactionNotFound = "Could not find transaction for coin = {0} with id = {1}";
        public const string AmountIsTooLow = "Amount is too low.";
        public const string PriceIsTooLow = "Price is too low.";
        public const string DepositAddressNotFound = "Could not find deposit address for symbol {0}.";
        public const string DepositAddressWithChainNotFound = "Could not find deposit address for symbol {0} with chain {1}.";
        public const string OnlyOnePaymentTypeCanBeUsed = "Cannot use both AmountIsFiat and AmountIsInPaymentSymbol.";
    }

    // Used to return explicit display messages to the users when certain operations fail
    // E.g. instead of 'Failed to execute request', the API returns 'Failed to get balance' (for example, it can happen in buy/sell request)
    public static class BrokerOperationNames
    {
        public const string Authenticate = "authenticate";
        public const string GetAccount = "get account";
        public const string GetAccounts = "get accounts";
        public const string PlaceOrder = "place {0} order";
        public const string GetPortfolio = "get portfolio";
        public const string CancelOrder = "cancel order";
        public const string CancelOrders = "cancel orders";
        public const string GetSymbolInfo = "get symbol info";
        public const string GetBalance = "get balance";
        public const string GetOrder = "get order";
        public const string GetCryptoConverts = "get crypto converts";
        public const string GetSplits = "get splits";
        public const string GetOrders = "get orders";
        public const string GetTransactions = "get transactions";
        public const string GetTransaction = "get transaction";
        public const string GetUserInfo = "get user info";
        public const string GetPaymentMethods = "get payment method";
        public const string PreviewOrder = "preview order";
        public const string GetDepositAddress = "get deposit address";
        public const string GeneratePrivateKey = "generate private key";
        public const string InitiateCryptoTransaction = "initiate cryptocurrency transaction";
        public const string RequestMfaCode = "request mfa code";
        public const string GenerateCryptoDepositAddress = "generate cryptocurrency address";
        public const string GetTransactionFee = "get transaction fee";
        public const string GetTransactionQuota = "get transaction quota";
        public const string GetCryptocurrencyChain = "get cryptocurrency chain";
        public const string GetNftPositions = "get NFT positions";
        public const string GetOptionsPositions = "get options";
        public const string GetPriceInfo = "get price info";
    }
}
