using System.Net;

namespace Developist.Extensions.Api.Exceptions
{
    public class ApiException : ApplicationException
    {
        public ApiException(HttpStatusCode statusCode)
        {
            StatusCode = EnsureErrorStatusCode(statusCode);
            ReasonPhrase = this.GetDefaultReasonPhrase();
            HelpLink = this.GetDefaultHelpLink();
        }

        public ApiException(HttpStatusCode statusCode, string message)
            : base(message)
        {
            StatusCode = EnsureErrorStatusCode(statusCode);
            ReasonPhrase = this.GetDefaultReasonPhrase();
            HelpLink = this.GetDefaultHelpLink();
        }

        public ApiException(HttpStatusCode statusCode, string message, Exception innerException)
            : base(message, innerException)
        {
            StatusCode = EnsureErrorStatusCode(statusCode);
            ReasonPhrase = this.GetDefaultReasonPhrase();
            HelpLink = this.GetDefaultHelpLink();
        }

        public HttpStatusCode StatusCode { get; }
        public string ReasonPhrase { get; protected set; }
        public new Uri HelpLink
        {
            get => new(base.HelpLink ?? "about:blank");
            protected set => base.HelpLink = value.ToString();
        }

        private static HttpStatusCode EnsureErrorStatusCode(HttpStatusCode statusCode)
        {
            if ((int)statusCode is < 400 or > 599)
            {
                throw new ArgumentOutOfRangeException(
                    paramName: nameof(statusCode),
                    actualValue: (int)statusCode,
                    message: "Value does not indicate an HTTP error.");
            }
            return statusCode;
        }
    }
}
