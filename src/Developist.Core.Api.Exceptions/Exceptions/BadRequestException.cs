using System.Net;

namespace Developist.Core.Api.Exceptions;

/// <summary>
/// Represents an HTTP 400 Bad Request error that occurs during the execution of an API request.
/// </summary>
public class BadRequestException : ApiException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BadRequestException"/> class with a default error message.
    /// </summary>
    public BadRequestException()
        : base(HttpStatusCode.BadRequest) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="BadRequestException"/> class with the specified error message.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    public BadRequestException(string message)
        : base(HttpStatusCode.BadRequest, message) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="BadRequestException"/> class with the specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
    public BadRequestException(string message, Exception innerException)
        : base(HttpStatusCode.BadRequest, message, innerException) { }
}
