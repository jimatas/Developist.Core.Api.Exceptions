using System.Net;
using System.Text;

namespace Developist.Extensions.Api.Exceptions
{
    public static class ApiExceptionExtensions
    {
        public static string GetDefaultReasonPhrase(this ApiException exception)
        {
            return exception.StatusCode switch
            {
                HttpStatusCode.BadRequest => "Bad Request",
                HttpStatusCode.Unauthorized => "Unauthorized",
                HttpStatusCode.PaymentRequired => "Payment Required",
                HttpStatusCode.Forbidden => "Forbidden",
                HttpStatusCode.NotFound => "Not Found",
                HttpStatusCode.MethodNotAllowed => "Method Not Allowed",
                HttpStatusCode.NotAcceptable => "Not Acceptable",
                HttpStatusCode.ProxyAuthenticationRequired => "Proxy Authentication Required",
                HttpStatusCode.RequestTimeout => "Request Timeout",
                HttpStatusCode.Conflict => "Conflict",
                HttpStatusCode.Gone => "Gone",
                HttpStatusCode.LengthRequired => "Length Required",
                HttpStatusCode.PreconditionFailed => "Precondition Failed",
                HttpStatusCode.RequestEntityTooLarge => "Payload Too Large",
                HttpStatusCode.RequestUriTooLong => "URI Too Long",
                HttpStatusCode.UnsupportedMediaType => "Unsupported Media Type",
                HttpStatusCode.RequestedRangeNotSatisfiable => "Range Not Satisfiable",
                HttpStatusCode.ExpectationFailed => "Expectation Failed",
                HttpStatusCode.MisdirectedRequest => "Misdirected Request",
                HttpStatusCode.UnprocessableEntity => "Unprocessable Entity",
                HttpStatusCode.Locked => "Locked",
                HttpStatusCode.FailedDependency => "Failed Dependency",
                HttpStatusCode.UpgradeRequired => "Upgrade Required",
                HttpStatusCode.PreconditionRequired => "Precondition Required",
                HttpStatusCode.TooManyRequests => "Too Many Requests",
                HttpStatusCode.RequestHeaderFieldsTooLarge => "Request Header Fields Too Large",
                HttpStatusCode.UnavailableForLegalReasons => "Unavailable For Legal Reasons",
                HttpStatusCode.InternalServerError => "Internal Server Error",
                HttpStatusCode.NotImplemented => "Not Implemented",
                HttpStatusCode.BadGateway => "Bad Gateway",
                HttpStatusCode.ServiceUnavailable => "Service Unavailable",
                HttpStatusCode.GatewayTimeout => "Gateway Timeout",
                HttpStatusCode.HttpVersionNotSupported => "HTTP Version Not Supported",
                HttpStatusCode.VariantAlsoNegotiates => "Variant Also Negotiates",
                HttpStatusCode.InsufficientStorage => "Insufficient Storage",
                HttpStatusCode.LoopDetected => "Loop Detected",
                HttpStatusCode.NotExtended => "Not Extended",
                HttpStatusCode.NetworkAuthenticationRequired => "Network Authentication Required",
                _ => string.Empty
            };
        }

        public static Uri GetDefaultHelpLink(this ApiException exception)
        {
            return exception.StatusCode switch
            {
                // https://www.iana.org/assignments/http-status-codes/http-status-codes.xhtml
                //
                HttpStatusCode.BadRequest => new Uri("https://www.rfc-editor.org/rfc/rfc9110#section-15.5.1"),
                HttpStatusCode.Unauthorized => new Uri("https://www.rfc-editor.org/rfc/rfc9110#section-15.5.2"),
                HttpStatusCode.PaymentRequired => new Uri("https://www.rfc-editor.org/rfc/rfc9110#section-15.5.3"),
                HttpStatusCode.Forbidden => new Uri("https://www.rfc-editor.org/rfc/rfc9110#section-15.5.4"),
                HttpStatusCode.NotFound => new Uri("https://www.rfc-editor.org/rfc/rfc9110#section-15.5.5"),
                HttpStatusCode.MethodNotAllowed => new Uri("https://www.rfc-editor.org/rfc/rfc9110#section-15.5.6"),
                HttpStatusCode.NotAcceptable => new Uri("https://www.rfc-editor.org/rfc/rfc9110#section-15.5.7"),
                HttpStatusCode.ProxyAuthenticationRequired => new Uri("https://www.rfc-editor.org/rfc/rfc9110#section-15.5.8"),
                HttpStatusCode.RequestTimeout => new Uri("https://www.rfc-editor.org/rfc/rfc9110#section-15.5.9"),
                HttpStatusCode.Conflict => new Uri("https://www.rfc-editor.org/rfc/rfc9110#section-15.5.10"),
                HttpStatusCode.Gone => new Uri("https://www.rfc-editor.org/rfc/rfc9110#section-15.5.11"),
                HttpStatusCode.LengthRequired => new Uri("https://www.rfc-editor.org/rfc/rfc9110#section-15.5.12"),
                HttpStatusCode.PreconditionFailed => new Uri("https://www.rfc-editor.org/rfc/rfc9110#section-15.5.13"),
                HttpStatusCode.RequestEntityTooLarge => new Uri("https://www.rfc-editor.org/rfc/rfc9110#section-15.5.14"),
                HttpStatusCode.RequestUriTooLong => new Uri("https://www.rfc-editor.org/rfc/rfc9110#section-15.5.15"),
                HttpStatusCode.UnsupportedMediaType => new Uri("https://www.rfc-editor.org/rfc/rfc9110#section-15.5.16"),
                HttpStatusCode.RequestedRangeNotSatisfiable => new Uri("https://www.rfc-editor.org/rfc/rfc9110#section-15.5.17"),
                HttpStatusCode.ExpectationFailed => new Uri("https://www.rfc-editor.org/rfc/rfc9110#section-15.5.18"),
                HttpStatusCode.MisdirectedRequest => new Uri("https://www.rfc-editor.org/rfc/rfc9110#section-15.5.20"),
                HttpStatusCode.UnprocessableEntity => new Uri("https://www.rfc-editor.org/rfc/rfc9110#section-15.5.21"),
                HttpStatusCode.Locked => new Uri("https://www.rfc-editor.org/rfc/rfc4918.html#section-11.3"),
                HttpStatusCode.FailedDependency => new Uri("https://www.rfc-editor.org/rfc/rfc4918.html#section-11.4"),
                HttpStatusCode.UpgradeRequired => new Uri("https://www.rfc-editor.org/rfc/rfc9110#section-15.5.22"),
                HttpStatusCode.PreconditionRequired => new Uri("https://www.rfc-editor.org/rfc/rfc6585.html#section-3"),
                HttpStatusCode.TooManyRequests => new Uri("https://www.rfc-editor.org/rfc/rfc6585.html#section-4"),
                HttpStatusCode.RequestHeaderFieldsTooLarge => new Uri("https://www.rfc-editor.org/rfc/rfc6585.html#section-5"),
                HttpStatusCode.UnavailableForLegalReasons => new Uri("https://www.rfc-editor.org/rfc/rfc7725.html#section-3"),
                HttpStatusCode.InternalServerError => new Uri("https://www.rfc-editor.org/rfc/rfc9110#section-15.6.1"),
                HttpStatusCode.NotImplemented => new Uri("https://www.rfc-editor.org/rfc/rfc9110#section-15.6.2"),
                HttpStatusCode.BadGateway => new Uri("https://www.rfc-editor.org/rfc/rfc9110#section-15.6.3"),
                HttpStatusCode.ServiceUnavailable => new Uri("https://www.rfc-editor.org/rfc/rfc9110#section-15.6.4"),
                HttpStatusCode.GatewayTimeout => new Uri("https://www.rfc-editor.org/rfc/rfc9110#section-15.6.5"),
                HttpStatusCode.HttpVersionNotSupported => new Uri("https://www.rfc-editor.org/rfc/rfc9110#section-15.6.6"),
                HttpStatusCode.VariantAlsoNegotiates => new Uri("https://www.rfc-editor.org/rfc/rfc2295.html#section-8.1s"),
                HttpStatusCode.InsufficientStorage => new Uri("https://www.rfc-editor.org/rfc/rfc4918.html#section-11.5"),
                HttpStatusCode.LoopDetected => new Uri("https://www.rfc-editor.org/rfc/rfc5842.html#section-7.2"),
                HttpStatusCode.NotExtended => new Uri("https://www.rfc-editor.org/rfc/rfc2774.html#section-7"),
                HttpStatusCode.NetworkAuthenticationRequired => new Uri("https://www.rfc-editor.org/rfc/rfc6585.html#section-6"),
                _ => new Uri("about:blank")
            };
        }

        public static string GetDetailMessage(this ApiException exception)
        {
            StringBuilder detailMessageBuilder = new(GetDetailMessageFor(exception));

            Exception? innerException = exception.InnerException;
            for (int level = 1; innerException is not null; level++)
            {
                detailMessageBuilder.Append($" [InnerException ({level}): {GetDetailMessageFor(innerException)}]");
                innerException = innerException.InnerException;
            }

            return detailMessageBuilder.ToString();

            static string GetDetailMessageFor(Exception exception)
            {
                string detailMessage = $"{exception.GetType().FullName}: {exception.Message}";
                if (!string.IsNullOrEmpty(exception.StackTrace))
                {
                    detailMessage += Environment.NewLine + exception.StackTrace;
                }
                return detailMessage;
            }
        }
    }
}
