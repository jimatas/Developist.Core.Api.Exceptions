using Developist.Core.Api.Utilities;
using System.Net;

namespace Developist.Core.Api.Exceptions;

/// <summary>
/// Represents a custom exception that is used to signal errors in the processing of HTTP requests made to an API.
/// </summary>
public class ApiException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ApiException"/> class with the specified HTTP status code.
    /// </summary>
    /// <param name="statusCode">The HTTP status code.</param>
    public ApiException(HttpStatusCode statusCode)
    {
        StatusCode = statusCode.EnsureErrorStatusCode();
        ReasonPhrase = this.GetDefaultReasonPhrase();
        HelpLink = this.GetDefaultHelpLink();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiException"/> class with the specified HTTP status code and error message.
    /// </summary>
    /// <param name="statusCode">The HTTP status code.</param>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    public ApiException(HttpStatusCode statusCode, string message)
        : base(message)
    {
        StatusCode = statusCode.EnsureErrorStatusCode();
        ReasonPhrase = this.GetDefaultReasonPhrase();
        HelpLink = this.GetDefaultHelpLink();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiException"/> class with the specified HTTP status code, error message, and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="statusCode">The HTTP status code.</param>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
    public ApiException(HttpStatusCode statusCode, string message, Exception innerException)
        : base(message, innerException)
    {
        StatusCode = statusCode.EnsureErrorStatusCode();
        ReasonPhrase = this.GetDefaultReasonPhrase();
        HelpLink = this.GetDefaultHelpLink();
    }

    /// <summary>
    /// Gets the HTTP status code associated with this exception.
    /// </summary>
    public HttpStatusCode StatusCode { get; }

    /// <summary>
    /// Gets the reason phrase that describes the status code associated with this exception.
    /// </summary>
    public string ReasonPhrase { get; protected set; }

    /// <summary>
    /// Gets the URI that contains information about the exception.
    /// </summary>
    public new Uri HelpLink
    {
        get => new(base.HelpLink ?? "about:blank");
        protected set => base.HelpLink = value.ToString();
    }
}
