namespace Org.Front.Core.Contracts.Exceptions
{
    public class ApiCallNotFoundException : ApiCallException
    {
        public ApiCallNotFoundException() : base()
        {
        }

        public ApiCallNotFoundException(string message) : base(message)
        {
        }

        public ApiCallNotFoundException(string message, string displayMessage) : base(message, displayMessage)
        {
        }

        public ApiCallNotFoundException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}
