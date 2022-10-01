using System.Net;

namespace Developist.Extensions.Api.Exceptions
{
    public class BadRequestException : ApiException
    {
        public BadRequestException()
            : base(HttpStatusCode.BadRequest) { }

        public BadRequestException(string message)
            : base(HttpStatusCode.BadRequest, message) { }

        public BadRequestException(string message, Exception innerException)
            : base(HttpStatusCode.BadRequest, message, innerException) { }
    }
}
