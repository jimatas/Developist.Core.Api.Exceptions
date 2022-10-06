using Developist.Extensions.Api.Exceptions;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace Developist.Extensions.Api.MvcFilters
{
    public class ApiExceptionContext
    {
        internal ApiExceptionContext(
            ApiException exception,
            IHostEnvironment environment,
            HttpContext httpContext)
        {
            Exception = exception;
            Environment = environment;
            HttpContext = httpContext;
        }

        public ApiException Exception { get; }
        public IHostEnvironment Environment { get; }
        public HttpContext HttpContext { get; }
    }
}
