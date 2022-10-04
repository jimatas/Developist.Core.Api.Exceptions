using System.Net;

namespace Developist.Extensions.Api.Exceptions
{
    public class ConflictException : ApiException
    {
        public ConflictException()
            : base(HttpStatusCode.Conflict) { }

        public ConflictException(string message)
            : base(HttpStatusCode.Conflict, message) { }

        public ConflictException(string message, Exception innerException)
            : base(HttpStatusCode.Conflict, message, innerException) { }
    }
}
