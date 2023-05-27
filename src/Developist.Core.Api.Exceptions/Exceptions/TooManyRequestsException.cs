using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace Developist.Core.Api.Exceptions;

/// <summary>
/// Represents an HTTP 429 Too Many Requests error that occurs during the execution of an API request.
/// </summary>
public class TooManyRequestsException : ApiException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TooManyRequestsException"/> class with a default error message.
    /// </summary>
    public TooManyRequestsException()
        : base(HttpStatusCode.TooManyRequests) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="TooManyRequestsException"/> class with the specified error message.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    public TooManyRequestsException(string message)
        : base(HttpStatusCode.TooManyRequests, message) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="TooManyRequestsException"/> class with the specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
    public TooManyRequestsException(string message, Exception innerException)
        : base(HttpStatusCode.TooManyRequests, message, innerException) { }

    /// <summary>
    /// Gets or sets the duration after which the client can retry the request.
    /// </summary>
    [ExcludeFromCodeCoverage(Justification = "Just a simple, automatic property.")]
    public TimeSpan? RetryAfter { get; set; }
}
