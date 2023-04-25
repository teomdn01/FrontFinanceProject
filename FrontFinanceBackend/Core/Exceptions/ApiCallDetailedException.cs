namespace Org.Front.Core.Contracts.Exceptions
{
    public class ApiCallDetailedException : ApiCallException
    {
        public ApiCallDetailedException()
        {
        }

        public ApiCallDetailedException(string message) : base(message)
        {
        }

        public ApiCallDetailedException(string message, string displayMessage) : base(message, displayMessage)
        {
        }

        public ApiCallDetailedException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}
