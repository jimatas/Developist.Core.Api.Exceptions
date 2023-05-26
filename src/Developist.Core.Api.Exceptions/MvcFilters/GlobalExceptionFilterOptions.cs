using Developist.Core.Api.Utilities;
using System.Net;

namespace Developist.Core.Api.MvcFilters;

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
        _exceptionTypesByInheritanceDepthInitializer = new Lazy<IEnumerable<Type>>(
            () => _exceptionToStatusCodeMappings.Keys.OrderByDescending(type => type, new DepthOfInheritanceComparer()));
    }

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

    internal HttpStatusCode GetMappedStatusCodeOrDefault(Exception exception,
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
