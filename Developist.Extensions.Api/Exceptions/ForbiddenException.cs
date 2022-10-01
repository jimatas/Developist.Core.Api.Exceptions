using System.Net;

namespace Developist.Extensions.Api.Exceptions
{
    public class ForbiddenException : ApiException
    {
        public ForbiddenException()
            : base(HttpStatusCode.Forbidden) { }

        public ForbiddenException(string message)
            : base(HttpStatusCode.Forbidden, message) { }

        public ForbiddenException(string message, Exception innerException)
            : base(HttpStatusCode.Forbidden, message, innerException) { }
    }
}
