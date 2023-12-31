namespace Developist.Core.Api.Exceptions;

/// <summary>
/// Represents an HTTP 422 Unprocessable Entity error that occurs during the execution of an API request.
/// </summary>
public class UnprocessableEntityException : ApiException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UnprocessableEntityException"/> class with a default error message.
    /// </summary>
    public UnprocessableEntityException()
        : base(HttpStatusCode.UnprocessableEntity) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="UnprocessableEntityException"/> class with the specified error message.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    public UnprocessableEntityException(string message)
        : base(HttpStatusCode.UnprocessableEntity, message) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="UnprocessableEntityException"/> class with the specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
    public UnprocessableEntityException(string message, Exception innerException)
        : base(HttpStatusCode.UnprocessableEntity, message, innerException) { }
}
