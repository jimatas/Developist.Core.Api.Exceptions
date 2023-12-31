namespace Developist.Core.Api.Exceptions;

/// <summary>
/// Provides extension methods for the <see cref="ApiException"/> class.
/// </summary>
public static class ApiExceptionExtensions
{
    private static readonly Dictionary<HttpStatusCode, (string ReasonPhrase, Uri HelpLink)> DefaultResponses = new()
    {
        { HttpStatusCode.BadRequest, ("Bad Request", new("https://www.rfc-editor.org/rfc/rfc9110#section-15.5.1")) },
        { HttpStatusCode.Unauthorized, ("Unauthorized", new("https://www.rfc-editor.org/rfc/rfc9110#section-15.5.2")) },
        { HttpStatusCode.PaymentRequired, ("Payment Required", new("https://www.rfc-editor.org/rfc/rfc9110#section-15.5.3")) },
        { HttpStatusCode.Forbidden, ("Forbidden", new("https://www.rfc-editor.org/rfc/rfc9110#section-15.5.4")) },
        { HttpStatusCode.NotFound, ("Not Found", new("https://www.rfc-editor.org/rfc/rfc9110#section-15.5.5")) },
        { HttpStatusCode.MethodNotAllowed, ("Method Not Allowed", new("https://www.rfc-editor.org/rfc/rfc9110#section-15.5.6")) },
        { HttpStatusCode.NotAcceptable, ("Not Acceptable", new("https://www.rfc-editor.org/rfc/rfc9110#section-15.5.7")) },
        { HttpStatusCode.ProxyAuthenticationRequired, ("Proxy Authentication Required", new("https://www.rfc-editor.org/rfc/rfc9110#section-15.5.8")) },
        { HttpStatusCode.RequestTimeout, ("Request Timeout", new("https://www.rfc-editor.org/rfc/rfc9110#section-15.5.9")) },
        { HttpStatusCode.Conflict, ("Conflict", new("https://www.rfc-editor.org/rfc/rfc9110#section-15.5.10")) },
        { HttpStatusCode.Gone, ("Gone", new("https://www.rfc-editor.org/rfc/rfc9110#section-15.5.11")) },
        { HttpStatusCode.LengthRequired, ("Length Required", new("https://www.rfc-editor.org/rfc/rfc9110#section-15.5.12")) },
        { HttpStatusCode.PreconditionFailed, ("Precondition Failed", new("https://www.rfc-editor.org/rfc/rfc9110#section-15.5.13")) },
        { HttpStatusCode.RequestEntityTooLarge, ("Payload Too Large", new("https://www.rfc-editor.org/rfc/rfc9110#section-15.5.14")) },
        { HttpStatusCode.RequestUriTooLong, ("URI Too Long", new("https://www.rfc-editor.org/rfc/rfc9110#section-15.5.15")) },
        { HttpStatusCode.UnsupportedMediaType, ("Unsupported Media Type", new("https://www.rfc-editor.org/rfc/rfc9110#section-15.5.16")) },
        { HttpStatusCode.RequestedRangeNotSatisfiable, ("Range Not Satisfiable", new("https://www.rfc-editor.org/rfc/rfc9110#section-15.5.17")) },
        { HttpStatusCode.ExpectationFailed, ("Expectation Failed", new("https://www.rfc-editor.org/rfc/rfc9110#section-15.5.18")) },
        { HttpStatusCode.MisdirectedRequest, ("Misdirected Request", new("https://www.rfc-editor.org/rfc/rfc9110#section-15.5.20")) },
        { HttpStatusCode.UnprocessableEntity, ("Unprocessable Entity", new("https://www.rfc-editor.org/rfc/rfc9110#section-15.5.21")) },
        { HttpStatusCode.Locked, ("Locked", new("https://www.rfc-editor.org/rfc/rfc4918.html#section-11.3")) },
        { HttpStatusCode.FailedDependency, ("Failed Dependency", new("https://www.rfc-editor.org/rfc/rfc4918.html#section-11.4")) },
        { HttpStatusCode.UpgradeRequired, ("Upgrade Required", new("https://www.rfc-editor.org/rfc/rfc9110#section-15.5.22")) },
        { HttpStatusCode.PreconditionRequired, ("Precondition Required", new("https://www.rfc-editor.org/rfc/rfc6585.html#section-3")) },
        { HttpStatusCode.TooManyRequests, ("Too Many Requests", new("https://www.rfc-editor.org/rfc/rfc6585.html#section-4")) },
        { HttpStatusCode.RequestHeaderFieldsTooLarge, ("Request Header Fields Too Large", new("https://www.rfc-editor.org/rfc/rfc6585.html#section-5")) },
        { HttpStatusCode.UnavailableForLegalReasons, ("Unavailable For Legal Reasons", new("https://www.rfc-editor.org/rfc/rfc7725.html#section-3")) },
        { HttpStatusCode.InternalServerError, ("Internal Server Error", new("https://www.rfc-editor.org/rfc/rfc9110#section-15.6.1")) },
        { HttpStatusCode.NotImplemented, ("Not Implemented", new("https://www.rfc-editor.org/rfc/rfc9110#section-15.6.2")) },
        { HttpStatusCode.BadGateway, ("Bad Gateway", new("https://www.rfc-editor.org/rfc/rfc9110#section-15.6.3")) },
        { HttpStatusCode.ServiceUnavailable, ("Service Unavailable", new("https://www.rfc-editor.org/rfc/rfc9110#section-15.6.4")) },
        { HttpStatusCode.GatewayTimeout, ("Gateway Timeout", new("https://www.rfc-editor.org/rfc/rfc9110#section-15.6.5")) },
        { HttpStatusCode.HttpVersionNotSupported, ("HTTP Version Not Supported", new("https://www.rfc-editor.org/rfc/rfc9110#section-15.6.6")) },
        { HttpStatusCode.VariantAlsoNegotiates, ("Variant Also Negotiates", new("https://www.rfc-editor.org/rfc/rfc2295.html#section-8.1")) },
        { HttpStatusCode.InsufficientStorage, ("Insufficient Storage", new("https://www.rfc-editor.org/rfc/rfc4918.html#section-11.5")) },
        { HttpStatusCode.LoopDetected, ("Loop Detected", new("https://www.rfc-editor.org/rfc/rfc5842.html#section-7.2")) },
        { HttpStatusCode.NotExtended, ("Not Extended", new("https://www.rfc-editor.org/rfc/rfc2774.html#section-7")) },
        { HttpStatusCode.NetworkAuthenticationRequired, ("Network Authentication Required", new("https://www.rfc-editor.org/rfc/rfc6585.html#section-6")) }
    };

    /// <summary>
    /// Gets the default reason phrase associated with the current <see cref="ApiException"/> instance.
    /// </summary>
    /// <param name="exception">The current <see cref="ApiException"/> instance.</param>
    /// <returns>The default reason phrase associated with the specified HTTP status code.</returns>
    public static string GetDefaultReasonPhrase(this ApiException exception)
    {
        return DefaultResponses.TryGetValue(exception.StatusCode, out var defaultResponse)
            ? defaultResponse.ReasonPhrase
            : string.Empty;
    }

    /// <summary>
    /// Gets the default help link associated with the current <see cref="ApiException"/> instance.
    /// </summary>
    /// <param name="exception">The current <see cref="ApiException"/> instance.</param>
    /// <returns>The default help link associated with the specified HTTP status code.</returns>
    public static Uri GetDefaultHelpLink(this ApiException exception)
    {
        return DefaultResponses.TryGetValue(exception.StatusCode, out var defaultResponse)
            ? defaultResponse.HelpLink
            : new("about:blank");
    }

    /// <summary>
    /// Gets a detailed error message for the current <see cref="Exception"/> instance, including any inner exceptions.
    /// </summary>
    /// <param name="exception">The current <see cref="Exception"/> instance.</param>
    /// <returns>A detailed error message for the current <see cref="Exception"/> instance.</returns>
    public static string GetDetailMessage(this Exception exception)
    {
        var detailMessageBuilder = new StringBuilder(GetDetailMessageCore(exception));
        var innerException = exception.InnerException;

        for (var level = 1; innerException is not null; level++)
        {
            detailMessageBuilder.Append($" [InnerException ({level}): {GetDetailMessageCore(innerException)}]");
            innerException = innerException.InnerException;
        }

        return detailMessageBuilder.ToString();
    }

    private static string GetDetailMessageCore(Exception exception)
    {
        var detailMessage = $"{exception.GetType().FullName}: {exception.Message}";
        if (!string.IsNullOrEmpty(exception.StackTrace))
        {
            detailMessage += Environment.NewLine + exception.StackTrace;
        }

        return detailMessage;
    }
}
