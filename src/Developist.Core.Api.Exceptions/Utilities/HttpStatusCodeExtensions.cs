using System.Net;
using System.Runtime.CompilerServices;

namespace Developist.Core.Api.Utilities;

/// <summary>
/// Provides extension methods for the <see cref="HttpStatusCode"/> enumeration.
/// </summary>
public static class HttpStatusCodeExtensions
{
    /// <summary>
    /// Determines whether the specified HTTP status code is an error status code.
    /// </summary>
    /// <param name="statusCode">The HTTP status code to check.</param>
    /// <returns><c>true</c> if the specified HTTP status code is an error status code; otherwise, <c>false</c>.</returns>
    public static bool IsErrorStatusCode(this HttpStatusCode statusCode)
    {
        return (int)statusCode is >= 400 and <= 599;
    }

    internal static HttpStatusCode EnsureErrorStatusCode(this HttpStatusCode statusCode,
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
