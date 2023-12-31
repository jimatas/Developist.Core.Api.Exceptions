using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Developist.Core.Api.Exceptions.Filters;

/// <summary>
/// Represents the options that can be used to configure a <see cref="GlobalExceptionFilterAttribute"/>.
/// </summary>
public class GlobalExceptionFilterOptions
{
    private readonly Dictionary<Type, HttpStatusCode> _exceptionToStatusCodeMappings = new();
    private readonly Lazy<IEnumerable<Type>> _exceptionTypesByInheritanceDepthInitializer;

    /// <summary>
    /// Initializes a new instance of the <see cref="GlobalExceptionFilterOptions"/> class.
    /// </summary>
    public GlobalExceptionFilterOptions()
    {
        _exceptionTypesByInheritanceDepthInitializer = new Lazy<IEnumerable<Type>>(() =>
            _exceptionToStatusCodeMappings.Keys.OrderByDescending(type => type, new DepthOfInheritanceComparer()));
    }

    /// <summary>
    /// Gets or sets a delegate that determines whether to disclose exception details in the problem details response.
    /// </summary>
    /// <remarks>
    /// The default function discloses exception details in the development environment only.
    /// </remarks>
    public Func<Exception, HttpContext, bool> ShouldDiscloseExceptionDetails { get; set; } = (_, ctx) =>
    {
        return ctx.RequestServices.GetRequiredService<IHostEnvironment>().IsDevelopment();
    };

    /// <summary>
    /// Maps the specified exception type to the specified HTTP status code.
    /// </summary>
    /// <typeparam name="TException">The type of the exception to map.</typeparam>
    /// <param name="statusCode">The HTTP status code to map to the specified exception type.</param>
    public void MapExceptionToStatusCode<TException>(HttpStatusCode statusCode)
        where TException : Exception
    {
        _exceptionToStatusCodeMappings[typeof(TException)] = statusCode.EnsureErrorStatusCode();
    }

    /// <summary>
    /// Gets the mapped HTTP status code for a given exception, or a default status code if no mapping is found.
    /// </summary>
    /// <param name="exception">The exception for which to retrieve the mapped status code.</param>
    /// <param name="defaultStatusCode">The default HTTP status code to use if no mapping is found.</param>
    /// <returns>The mapped HTTP status code associated with the provided exception type,
    /// or the default status code if no specific mapping is defined.</returns>
    public HttpStatusCode GetMappedStatusCodeOrDefault(Exception exception,
        HttpStatusCode defaultStatusCode = HttpStatusCode.InternalServerError)
    {
        foreach (var exceptionType in _exceptionTypesByInheritanceDepthInitializer.Value)
        {
            if (exceptionType.IsAssignableFrom(exception.GetType()))
            {
                return _exceptionToStatusCodeMappings[exceptionType];
            }
        }

        return defaultStatusCode;
    }

    private class DepthOfInheritanceComparer : IComparer<Type>
    {
        public int Compare(Type? x, Type? y)
        {
            var xDepth = 0;
            while (x != typeof(object))
            {
                x = x!.BaseType;
                xDepth++;
            }

            var yDepth = 0;
            while (y != typeof(object))
            {
                y = y!.BaseType;
                yDepth++;
            }

            return Math.Clamp(xDepth - yDepth,
                min: -1,
                max: 1);
        }
    }
}
