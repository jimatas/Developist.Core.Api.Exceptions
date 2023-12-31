namespace Developist.Core.Api.Exceptions;

/// <summary>
/// Represents an HTTP 409 Conflict error that occurs during the execution of an API request.
/// </summary>
public class ConflictException : ApiException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ConflictException"/> class with a default error message.
    /// </summary>
    public ConflictException()
        : base(HttpStatusCode.Conflict) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ConflictException"/> class with the specified error message.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    public ConflictException(string message)
        : base(HttpStatusCode.Conflict, message) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ConflictException"/> class with the specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
    public ConflictException(string message, Exception innerException)
        : base(HttpStatusCode.Conflict, message, innerException) { }
}
