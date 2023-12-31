namespace Developist.Core.Api.Exceptions;

/// <summary>
/// Represents an HTTP 403 Forbidden error that occurs during the execution of an API request.
/// </summary>
public class ForbiddenException : ApiException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ForbiddenException"/> class with a default error message.
    /// </summary>
    public ForbiddenException()
        : base(HttpStatusCode.Forbidden) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ForbiddenException"/> class with the specified error message.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    public ForbiddenException(string message)
        : base(HttpStatusCode.Forbidden, message) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ForbiddenException"/> class with the specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
    public ForbiddenException(string message, Exception innerException)
        : base(HttpStatusCode.Forbidden, message, innerException) { }
}
