using System.Net;

namespace Developist.Extensions.Api.Exceptions
{
    public class UnauthorizedException : ApiException
    {
        public UnauthorizedException()
            : base(HttpStatusCode.Unauthorized) { }

        public UnauthorizedException(string message)
            : base(HttpStatusCode.Unauthorized, message) { }

        public UnauthorizedException(string message, Exception innerException)
            : base(HttpStatusCode.Unauthorized, message, innerException) { }
    }
}
