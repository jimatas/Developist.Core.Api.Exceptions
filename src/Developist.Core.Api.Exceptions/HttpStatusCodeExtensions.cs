namespace Developist.Core.Api.Exceptions;

/// <summary>
/// Provides extension methods for the <see cref="HttpStatusCode"/> enumeration.
/// </summary>
public static class HttpStatusCodeExtensions
{
    /// <summary>
    /// Determines whether the HTTP status code represents an error status code.
    /// </summary>
    /// <param name="statusCode">The HTTP status code to check.</param>
    /// <returns><see langword="true"/> if the specified HTTP status code is an error status code; otherwise, <see langword="false"/>.</returns>
    public static bool IsErrorStatusCode(this HttpStatusCode statusCode)
    {
        return (int)statusCode is >= 400 and <= 599;
    }

    /// <summary>
    /// Ensures that the HTTP status code represents an error status code and throws an <see cref="ArgumentOutOfRangeException"/> if not.
    /// </summary>
    /// <param name="statusCode">The HTTP status code to validate.</param>
    /// <param name="paramName">The name of the parameter being validated (automatically provided).</param>
    /// <returns>The validated HTTP status code.</returns>
    /// <exception cref="ArgumentOutOfRangeException"/>
    public static HttpStatusCode EnsureErrorStatusCode(this HttpStatusCode statusCode,
        [CallerArgumentExpression("statusCode")] string? paramName = default)
    {
        if (!statusCode.IsErrorStatusCode())
        {
            throw new ArgumentOutOfRangeException(
                paramName,
                actualValue: (int)statusCode,
                message: "Value does not indicate an HTTP error status.");
        }

        return statusCode;
    }
}
