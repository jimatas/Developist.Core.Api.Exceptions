using Developist.Extensions.Api.Exceptions;
using Developist.Extensions.Api.ProblemDetails;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Developist.Extensions.Api.MvcFilters
{
    public class ApiExceptionFilterAttribute : ExceptionFilterAttribute, IFilterFactory
    {
        private ApiExceptionFilterOptions? options;

        public ApiExceptionFilterAttribute() { }

        [ActivatorUtilitiesConstructor]
        public ApiExceptionFilterAttribute(IOptions<ApiExceptionFilterOptions> options) => this.options = options?.Value;

        public virtual bool IsReusable => true;

        public virtual IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
            => serviceProvider.GetService<ApiExceptionFilterAttribute>() ?? new();

        public override void OnException(ExceptionContext exceptionContext)
        {
            if (exceptionContext.Exception is ApiException apiException && !exceptionContext.ExceptionHandled)
            {
                var httpContext = exceptionContext.HttpContext;
                var getOptions = httpContext.RequestServices.GetRequiredService<IOptions<ApiExceptionFilterOptions>>;

                options ??= getOptions().Value;

                if (options.ShouldHandleException(apiException, httpContext))
                {
                    var problemDetails = apiException.ToProblemDetails(
                        options.ShouldDiscloseExceptionDetails(apiException, httpContext));

                    options.OnSerializingProblemDetails(problemDetails, apiException, httpContext);

                    exceptionContext.Result = new ObjectResult(problemDetails);
                    exceptionContext.ExceptionHandled = true;
                }
            }
            base.OnException(exceptionContext);
        }
    }
}
