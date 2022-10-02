using Developist.Extensions.Api.Exceptions;
using Developist.Extensions.Api.ProblemDetails;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

using System.Diagnostics;

namespace Developist.Extensions.Api.MvcFilters
{
    public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly ApiExceptionFilterOptions? options;
        private readonly IHostEnvironment? environment;

        public ApiExceptionFilterAttribute(IOptions<ApiExceptionFilterOptions>? options = null, IHostEnvironment? environment = null)
        {
            this.options = options?.Value;
            this.environment = environment;
        }

        public override void OnException(ExceptionContext exceptionContext)
        {
            if (exceptionContext.Exception is ApiException exception && ShouldHandleException(exception, exceptionContext.HttpContext))
            {
                var problemDetails = exception.ToProblemDetails(ShouldDiscloseExceptionDetails(exception, exceptionContext.HttpContext));
                AddTraceId(problemDetails, exceptionContext.HttpContext);

                exceptionContext.Result = new ObjectResult(problemDetails);
                exceptionContext.ExceptionHandled = true;
            }
            base.OnException(exceptionContext);
        }

        private bool ShouldHandleException(ApiException exception, HttpContext httpContext)
        {
            IHostEnvironment? environment = this.environment ?? httpContext.RequestServices.GetService<IHostEnvironment>();
            ApiExceptionFilterOptions? options = this.options ?? httpContext.RequestServices.GetService<IOptions<ApiExceptionFilterOptions>>()?.Value;

            return options?.ShouldHandleException(exception, environment) == true;
        }

        private bool ShouldDiscloseExceptionDetails(ApiException exception, HttpContext httpContext)
        {
            IHostEnvironment? environment = this.environment ?? httpContext.RequestServices.GetService<IHostEnvironment>();
            ApiExceptionFilterOptions? options = this.options ?? httpContext.RequestServices.GetService<IOptions<ApiExceptionFilterOptions>>()?.Value;

            return options?.ShouldDiscloseExceptionDetails(exception, environment) == true;
        }

        private static void AddTraceId(ApiProblemDetails problemDetails, HttpContext httpContext)
        {
            problemDetails.Extensions["traceId"] = Activity.Current?.Id ?? httpContext.TraceIdentifier;
        }
    }
}
