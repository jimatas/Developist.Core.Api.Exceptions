using Developist.Core.Api.Exceptions;

namespace Developist.Core.Api.ProblemDetails;

/// <summary>
/// Provides extension methods to convert an <see cref="ApiException"/> object to an <see cref="ApiProblemDetails"/> object.
/// </summary>
public static class ApiProblemDetailsExtensions
{
    /// <summary>
    /// Converts an <see cref="ApiException"/> object to an <see cref="ApiProblemDetails"/> object.
    /// </summary>
    /// <param name="exception">The <see cref="ApiException"/> object to convert.</param>
    /// <param name="discloseExceptionDetails">A boolean flag indicating whether to include the exception details in the problem details.</param>
    /// <returns>An <see cref="ApiProblemDetails"/> object representing the specified <see cref="ApiException"/> object.</returns>
    public static ApiProblemDetails ToProblemDetails(this ApiException exception, bool discloseExceptionDetails = false)
    {
        return new ApiProblemDetails
        {
            Status = exception.StatusCode,
            Title = exception.ReasonPhrase,
            Type = exception.HelpLink,
            Detail = discloseExceptionDetails ? exception.GetDetailMessage() : exception.Message
        };
    }
}
