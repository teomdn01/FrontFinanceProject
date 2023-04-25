using System;
using System.Net;
using Microsoft.Extensions.Logging;
using Org.Front.Core.Contracts.Exceptions;
using Org.Front.Core.Contracts.Models.Brokers;

namespace Org.Front.Core.Contracts.Helpers
{
    public static class BrokerErrorResponseHelper
    {
        // Created in order to follow the same logic when handling unsuccessful responses from all brokers.
        // This method always logs error(including content returned from broker),
        // processes

        public static Exception CreateBrokerException(
            HttpStatusCode statusCode,
            string parsedMessage,
            string content,
            string operationName,
            string brokerName,
            ILogger logger)
        {
            if (!string.IsNullOrEmpty(parsedMessage))
            {
                parsedMessage = parsedMessage.Replace("_", " ").FirstCharToUpper();
            }

            var genericErrorMessage =
                string.Format(BrokerErrorMessages.GenericOperationName, operationName, brokerName);

            //Make sure all erroneous responses are logged including content:
            logger.LogError(
                "Could not execute broker request. Broker name: {BrokerName}. Status = {StatusCode}. Content = {Content}",
                brokerName,
                statusCode,
                content);

            var displayMessage = string.IsNullOrEmpty(parsedMessage)
                ? genericErrorMessage
                : $"{genericErrorMessage} {parsedMessage}";

            return statusCode switch
            {
                HttpStatusCode.Unauthorized => new UnauthorizedApiCallException(
                    $"Unauthorized api call. Status: {statusCode}. Error message: {parsedMessage ?? content}", displayMessage),
                HttpStatusCode.BadRequest when !string.IsNullOrEmpty(parsedMessage) =>
                    new ApiCallDetailedException(parsedMessage, displayMessage),
                HttpStatusCode.NotFound when !string.IsNullOrEmpty(parsedMessage) =>
                    new ApiCallNotFoundException(parsedMessage, displayMessage),
                HttpStatusCode.Forbidden when !string.IsNullOrEmpty(parsedMessage) =>
                    new ApiCallException(parsedMessage, displayMessage),
                _ => new ApiCallException(
                    $"Failed to execute request. Status: {statusCode} Response: {content}", displayMessage)
            };
        }
    }
}
