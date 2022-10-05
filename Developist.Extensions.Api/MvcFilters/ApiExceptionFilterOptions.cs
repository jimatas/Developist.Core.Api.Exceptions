using Developist.Extensions.Api.Exceptions;
using Developist.Extensions.Api.ProblemDetails;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

using System.Diagnostics;

namespace Developist.Extensions.Api.MvcFilters
{
    public class ApiExceptionFilterOptions
    {
        public Func<ApiException, IHostEnvironment, bool> ShouldHandleException { get; set; } = (_, _) => true;
        public Func<ApiException, IHostEnvironment, bool> ShouldDiscloseExceptionDetails { get; set; } = (_, env) => env.IsDevelopment();
        public Action<ApiProblemDetails, HttpContext> OnSerializingProblemDetails { get; set; } = (prob, ctx) =>
        {
            prob.Extensions["traceId"] = Activity.Current?.Id ?? ctx.TraceIdentifier;
        };
    }
}
