using Microsoft.Extensions.Hosting;

using System.Diagnostics;

namespace Developist.Extensions.Api.MvcFilters
{
    public class ApiExceptionFilterOptions
    {
        public Func<ApiExceptionContext, bool> ShouldHandleException { get; set; } = _ => true;
        public Func<ApiExceptionContext, bool> ShouldDiscloseExceptionDetails { get; set; } = ctx => ctx.Environment.IsDevelopment();
        public Action<ApiProblemDetailsContext> OnSerializingProblemDetails { get; set; } = ctx =>
        {
            ctx.ProblemDetails.Extensions["traceId"] = Activity.Current?.Id ?? ctx.HttpContext.TraceIdentifier;
        };
    }
}
