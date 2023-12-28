using System.Net;
using System.Text;

namespace Developist.Core.Api.Exceptions;

/// <summary>
/// Provides extension methods for the <see cref="ApiException"/> class.
/// </summary>
public static class ApiExceptionExtensions
{
    private static readonly Dictionary<HttpStatusCode, (string ReasonPhrase, Uri HelpLink)> DefaultResponses = new()
    {
        { HttpStatusCode.BadRequest, ("Bad Request", new Uri("https://www.rfc-editor.org/rfc/rfc9110#section-15.5.1")) },
        { HttpStatusCode.Unauthorized, ("Unauthorized", new Uri("https://www.rfc-editor.org/rfc/rfc9110#section-15.5.2")) },
        { HttpStatusCode.PaymentRequired, ("Payment Required", new Uri("https://www.rfc-editor.org/rfc/rfc9110#section-15.5.3")) },
        { HttpStatusCode.Forbidden, ("Forbidden", new Uri("https://www.rfc-editor.org/rfc/rfc9110#section-15.5.4")) },
        { HttpStatusCode.NotFound, ("Not Found", new Uri("https://www.rfc-editor.org/rfc/rfc9110#section-15.5.5")) },
        { HttpStatusCode.MethodNotAllowed, ("Method Not Allowed", new Uri("https://www.rfc-editor.org/rfc/rfc9110#section-15.5.6")) },
        { HttpStatusCode.NotAcceptable, ("Not Acceptable", new Uri("https://www.rfc-editor.org/rfc/rfc9110#section-15.5.7")) },
        { HttpStatusCode.ProxyAuthenticationRequired, ("Proxy Authentication Required", new Uri("https://www.rfc-editor.org/rfc/rfc9110#section-15.5.8")) },
        { HttpStatusCode.RequestTimeout, ("Request Timeout", new Uri("https://www.rfc-editor.org/rfc/rfc9110#section-15.5.9")) },
        { HttpStatusCode.Conflict, ("Conflict", new Uri("https://www.rfc-editor.org/rfc/rfc9110#section-15.5.10")) },
        { HttpStatusCode.Gone, ("Gone", new Uri("https://www.rfc-editor.org/rfc/rfc9110#section-15.5.11")) },
        { HttpStatusCode.LengthRequired, ("Length Required", new Uri("https://www.rfc-editor.org/rfc/rfc9110#section-15.5.12")) },
        { HttpStatusCode.PreconditionFailed, ("Precondition Failed", new Uri("https://www.rfc-editor.org/rfc/rfc9110#section-15.5.13")) },
        { HttpStatusCode.RequestEntityTooLarge, ("Payload Too Large", new Uri("https://www.rfc-editor.org/rfc/rfc9110#section-15.5.14")) },
        { HttpStatusCode.RequestUriTooLong, ("URI Too Long", new Uri("https://www.rfc-editor.org/rfc/rfc9110#section-15.5.15")) },
        { HttpStatusCode.UnsupportedMediaType, ("Unsupported Media Type", new Uri("https://www.rfc-editor.org/rfc/rfc9110#section-15.5.16")) },
        { HttpStatusCode.RequestedRangeNotSatisfiable, ("Range Not Satisfiable", new Uri("https://www.rfc-editor.org/rfc/rfc9110#section-15.5.17")) },
        { HttpStatusCode.ExpectationFailed, ("Expectation Failed", new Uri("https://www.rfc-editor.org/rfc/rfc9110#section-15.5.18")) },
        { HttpStatusCode.MisdirectedRequest, ("Misdirected Request", new Uri("https://www.rfc-editor.org/rfc/rfc9110#section-15.5.20")) },
        { HttpStatusCode.UnprocessableEntity, ("Unprocessable Entity", new Uri("https://www.rfc-editor.org/rfc/rfc9110#section-15.5.21")) },
        { HttpStatusCode.Locked, ("Locked", new Uri("https://www.rfc-editor.org/rfc/rfc4918.html#section-11.3")) },
        { HttpStatusCode.FailedDependency, ("Failed Dependency", new Uri("https://www.rfc-editor.org/rfc/rfc4918.html#section-11.4")) },
        { HttpStatusCode.UpgradeRequired, ("Upgrade Required", new Uri("https://www.rfc-editor.org/rfc/rfc9110#section-15.5.22")) },
        { HttpStatusCode.PreconditionRequired, ("Precondition Required", new Uri("https://www.rfc-editor.org/rfc/rfc6585.html#section-3")) },
        { HttpStatusCode.TooManyRequests, ("Too Many Requests", new Uri("https://www.rfc-editor.org/rfc/rfc6585.html#section-4")) },
        { HttpStatusCode.RequestHeaderFieldsTooLarge, ("Request Header Fields Too Large", new Uri("https://www.rfc-editor.org/rfc/rfc6585.html#section-5")) },
        { HttpStatusCode.UnavailableForLegalReasons, ("Unavailable For Legal Reasons", new Uri("https://www.rfc-editor.org/rfc/rfc7725.html#section-3")) },
        { HttpStatusCode.InternalServerError, ("Internal Server Error", new Uri("https://www.rfc-editor.org/rfc/rfc9110#section-15.6.1")) },
        { HttpStatusCode.NotImplemented, ("Not Implemented", new Uri("https://www.rfc-editor.org/rfc/rfc9110#section-15.6.2")) },
        { HttpStatusCode.BadGateway, ("Bad Gateway", new Uri("https://www.rfc-editor.org/rfc/rfc9110#section-15.6.3")) },
        { HttpStatusCode.ServiceUnavailable, ("Service Unavailable", new Uri("https://www.rfc-editor.org/rfc/rfc9110#section-15.6.4")) },
        { HttpStatusCode.GatewayTimeout, ("Gateway Timeout", new Uri("https://www.rfc-editor.org/rfc/rfc9110#section-15.6.5")) },
        { HttpStatusCode.HttpVersionNotSupported, ("HTTP Version Not Supported", new Uri("https://www.rfc-editor.org/rfc/rfc9110#section-15.6.6")) },
        { HttpStatusCode.VariantAlsoNegotiates, ("Variant Also Negotiates", new Uri("https://www.rfc-editor.org/rfc/rfc2295.html#section-8.1")) },
        { HttpStatusCode.InsufficientStorage, ("Insufficient Storage", new Uri("https://www.rfc-editor.org/rfc/rfc4918.html#section-11.5")) },
        { HttpStatusCode.LoopDetected, ("Loop Detected", new Uri("https://www.rfc-editor.org/rfc/rfc5842.html#section-7.2")) },
        { HttpStatusCode.NotExtended, ("Not Extended", new Uri("https://www.rfc-editor.org/rfc/rfc2774.html#section-7")) },
        { HttpStatusCode.NetworkAuthenticationRequired, ("Network Authentication Required", new Uri("https://www.rfc-editor.org/rfc/rfc6585.html#section-6")) }
    };

    /// <summary>
    /// Gets the default reason phrase associated with the specified <see cref="ApiException"/>.
    /// </summary>
    /// <param name="exception">The <see cref="ApiException"/> instance.</param>
    /// <returns>The default reason phrase associated with the specified HTTP status code.</returns>
    public static string GetDefaultReasonPhrase(this ApiException exception)
    {
        return DefaultResponses.TryGetValue(exception.StatusCode, out var defaultResponse)
            ? defaultResponse.ReasonPhrase
            : string.Empty;
    }

    /// <summary>
    /// Gets the default help link associated with the specified <see cref="ApiException"/>.
    /// </summary>
    /// <param name="exception">The <see cref="ApiException"/> instance.</param>
    /// <returns>The default help link associated with the specified HTTP status code.</returns>
    public static Uri GetDefaultHelpLink(this ApiException exception)
    {
        return DefaultResponses.TryGetValue(exception.StatusCode, out var defaultResponse)
            ? defaultResponse.HelpLink
            : new Uri("about:blank");
    }

    /// <summary>
    /// Gets a detailed error message for the specified <see cref="ApiException"/>, including any inner exceptions.
    /// </summary>
    /// <param name="exception">The <see cref="ApiException"/> instance.</param>
    /// <returns>A detailed error message for the specified <see cref="ApiException"/>.</returns>
    public static string GetDetailMessage(this ApiException exception)
    {
        var detailMessageBuilder = new StringBuilder(GetDetailMessageFor(exception));
        var innerException = exception.InnerException;
        
        for (var level = 1; innerException is not null; level++)
        {
            detailMessageBuilder.Append($" [InnerException ({level}): {GetDetailMessageFor(innerException)}]");
            innerException = innerException.InnerException;
        }

        return detailMessageBuilder.ToString();

        static string GetDetailMessageFor(Exception exception)
        {
            var detailMessage = $"{exception.GetType().FullName}: {exception.Message}";
            if (!string.IsNullOrEmpty(exception.StackTrace))
            {
                detailMessage += Environment.NewLine + exception.StackTrace;
            }

            return detailMessage;
        }
    }
}
