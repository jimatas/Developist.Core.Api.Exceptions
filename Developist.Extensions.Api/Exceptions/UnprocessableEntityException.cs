using System.Net;

namespace Developist.Extensions.Api.Exceptions
{
    public class UnprocessableEntityException : ApiException
    {
        public UnprocessableEntityException()
            : base(HttpStatusCode.UnprocessableEntity) { }

        public UnprocessableEntityException(string message)
            : base(HttpStatusCode.UnprocessableEntity, message) { }

        public UnprocessableEntityException(string message, Exception innerException)
            : base(HttpStatusCode.UnprocessableEntity, message, innerException) { }
    }
}
