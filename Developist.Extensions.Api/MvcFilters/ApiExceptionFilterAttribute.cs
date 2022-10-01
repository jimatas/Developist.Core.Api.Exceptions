using Developist.Extensions.Api.Exceptions;
using Developist.Extensions.Api.ProblemDetails;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using System.Diagnostics;

namespace Developist.Extensions.Api.MvcFilters
{
    public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext exceptionContext)
        {
            if (exceptionContext.Exception is ApiException apiException)
            {
                var problemDetails = apiException.ToProblemDetails(DiscloseExceptionDetails(exceptionContext.HttpContext));
                AddTraceId(problemDetails, exceptionContext.HttpContext);

                exceptionContext.Result = new ObjectResult(problemDetails);
                exceptionContext.ExceptionHandled = true;
            }
            base.OnException(exceptionContext);
        }

        private static bool DiscloseExceptionDetails(HttpContext httpContext)
        {
            IConfiguration configuration = httpContext.RequestServices.GetRequiredService<IConfiguration>();
            return bool.TryParse(configuration[nameof(DiscloseExceptionDetails)], out bool discloseExceptionDetails) && discloseExceptionDetails;
        }

        private static void AddTraceId(ApiProblemDetails problemDetails, HttpContext httpContext)
        {
            problemDetails.Extensions["traceId"] = Activity.Current?.Id ?? httpContext.TraceIdentifier;
        }
    }
}
