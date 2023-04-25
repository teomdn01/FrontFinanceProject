using System;

namespace Org.Front.Core.Contracts.Exceptions
{
    public class ApiCallException : Exception
    {
        public string DisplayMessage { get; set; }

        public ApiCallException()
        {
        }

        public ApiCallException(string message) : base(message)
        {
        }

        public ApiCallException(string message, string displayMessage) : base(message)
        {
            DisplayMessage = displayMessage;
        }

        public ApiCallException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
