using System.Collections.Generic;

namespace Brokers.Coinbase.Models
{
    public class ErrorResponse
    {
        public List<Error> Errors { get; set; }
    }

    public class Error
    {
        public string Id { get; set; }
        public string Message { get; set; }
    }
}
