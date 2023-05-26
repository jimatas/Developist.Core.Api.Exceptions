using System.Net;

namespace Developist.Core.Api.Exceptions;

/// <summary>
/// Represents an HTTP 404 Not Found error that occurs during the execution of an API request.
/// </summary>
public class NotFoundException : ApiException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NotFoundException"/> class with a default error message.
    /// </summary>
    public NotFoundException()
        : base(HttpStatusCode.NotFound) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="NotFoundException"/> class with the specified error message.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    public NotFoundException(string message)
        : base(HttpStatusCode.NotFound, message) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="NotFoundException"/> class with the specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
    public NotFoundException(string message, Exception innerException)
        : base(HttpStatusCode.NotFound, message, innerException) { }
}
