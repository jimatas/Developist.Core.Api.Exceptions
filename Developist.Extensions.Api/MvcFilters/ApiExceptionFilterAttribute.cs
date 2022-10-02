using Developist.Extensions.Api.Exceptions;
using Developist.Extensions.Api.ProblemDetails;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Developist.Extensions.Api.MvcFilters
{
    public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly ApiExceptionFilterOptions? options;
        private readonly IHostEnvironment? environment;

        public ApiExceptionFilterAttribute() { }
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
                OnSerializingProblemDetails(problemDetails, exceptionContext.HttpContext);

                exceptionContext.Result = new ObjectResult(problemDetails);
                exceptionContext.ExceptionHandled = true;
            }
            base.OnException(exceptionContext);
        }

        private bool ShouldHandleException(ApiException exception, HttpContext httpContext)
        {
            IHostEnvironment? environment = this.environment ?? httpContext.RequestServices.GetService<IHostEnvironment>();
            ApiExceptionFilterOptions? options = this.options ?? httpContext.RequestServices.GetService<IOptions<ApiExceptionFilterOptions>>()?.Value;

            return options?.ShouldHandleException(exception, environment) is true;
        }

        private bool ShouldDiscloseExceptionDetails(ApiException exception, HttpContext httpContext)
        {
            IHostEnvironment? environment = this.environment ?? httpContext.RequestServices.GetService<IHostEnvironment>();
            ApiExceptionFilterOptions? options = this.options ?? httpContext.RequestServices.GetService<IOptions<ApiExceptionFilterOptions>>()?.Value;

            return options?.ShouldDiscloseExceptionDetails(exception, environment) is true;
        }

        private void OnSerializingProblemDetails(ApiProblemDetails problemDetails, HttpContext httpContext)
        {
            ApiExceptionFilterOptions? options = this.options ?? httpContext.RequestServices.GetService<IOptions<ApiExceptionFilterOptions>>()?.Value;
            options?.OnSerializingProblemDetails(problemDetails, httpContext);
        }
    }
}
