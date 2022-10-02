using Developist.Extensions.Api.Exceptions;

using Microsoft.Extensions.Hosting;

namespace Developist.Extensions.Api.MvcFilters
{
    public class ApiExceptionFilterOptions
    {
        public Func<ApiException, IHostEnvironment?, bool> ShouldHandleException { get; set; } = (_, _) => true;
        public Func<ApiException, IHostEnvironment?, bool> ShouldDiscloseExceptionDetails { get; set; } = (_, env) => env?.IsDevelopment() == true;
    }
}
