using Developist.Extensions.Api.Exceptions;
using Developist.Extensions.Api.ProblemDetails;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using System.Diagnostics;

namespace Developist.Extensions.Api.MvcFilters
{
    public class ApiExceptionFilterOptions
    {
        public Func<ApiException, HttpContext, bool> ShouldHandleException { get; set; } = (_, _) => true;
        public Func<ApiException, HttpContext, bool> ShouldDiscloseExceptionDetails { get; set; } = (_, ctx) =>
        {
            return ctx.RequestServices.GetRequiredService<IHostEnvironment>().IsDevelopment();
        };

        public Action<ApiProblemDetails, ApiException, HttpContext> OnSerializingProblemDetails { get; set; } = (prob, _, ctx) =>
        {
            prob.Extensions["traceId"] = Activity.Current?.Id ?? ctx.TraceIdentifier;
        };
    }
}
