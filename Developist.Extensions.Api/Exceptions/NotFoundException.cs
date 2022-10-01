using System.Net;

namespace Developist.Extensions.Api.Exceptions
{
    public class NotFoundException : ApiException
    {
        public NotFoundException()
            : base(HttpStatusCode.NotFound) { }

        public NotFoundException(string message)
            : base(HttpStatusCode.NotFound, message) { }

        public NotFoundException(string message, Exception innerException)
            : base(HttpStatusCode.NotFound, message, innerException) { }
    }
}
