using System;

namespace Org.Front.Core.Contracts.Exceptions
{
    public class UnauthorizedApiCallException : ApiCallException
    {
        public bool TokenExpired { get; set; }
        public UnauthorizedApiCallException()
        {
        }

        public UnauthorizedApiCallException(string message) : base(message)
        {
        }

        public UnauthorizedApiCallException(string message, string displayMessage) : base(message, displayMessage)
        {
        }

        public UnauthorizedApiCallException(string message, bool tokenExpired) : base(message)
        {
            TokenExpired = tokenExpired;
        }

        public UnauthorizedApiCallException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
