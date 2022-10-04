using System.Net;

namespace Developist.Extensions.Api.Exceptions
{
    public class TooManyRequestsException : ApiException
    {
        public TooManyRequestsException()
            : base(HttpStatusCode.TooManyRequests) { }

        public TooManyRequestsException(string message)
            : base(HttpStatusCode.TooManyRequests, message) { }

        public TooManyRequestsException(string message, Exception innerException)
            : base(HttpStatusCode.TooManyRequests, message, innerException) { }

        public TimeSpan? RetryAfter { get; set; }
    }
}
