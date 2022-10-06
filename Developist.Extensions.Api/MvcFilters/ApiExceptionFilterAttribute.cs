﻿using Developist.Extensions.Api.Exceptions;
using Developist.Extensions.Api.ProblemDetails;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

using System.Diagnostics.CodeAnalysis;

namespace Developist.Extensions.Api.MvcFilters
{
    public class ApiExceptionFilterAttribute : ExceptionFilterAttribute, IFilterFactory
    {
        private ApiExceptionFilterOptions? options;
        private IHostEnvironment? environment;

        public ApiExceptionFilterAttribute() { }

        [ActivatorUtilitiesConstructor]
        public ApiExceptionFilterAttribute(IOptions<ApiExceptionFilterOptions> options, IHostEnvironment environment)
        {
            this.options = options?.Value;
            this.environment = environment;
        }

        public virtual bool IsReusable => true;

        public virtual IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
            => serviceProvider.GetService<ApiExceptionFilterAttribute>() ?? new();

        public override void OnException(ExceptionContext exceptionContext)
        {
            if (exceptionContext.Exception is ApiException apiException)
            {
                var httpContext = exceptionContext.HttpContext;
                
                EnsureServicesInitialized(httpContext.RequestServices);
                if (options.ShouldHandleException(apiException, environment))
                {
                    var problemDetails = apiException.ToProblemDetails(
                        options.ShouldDiscloseExceptionDetails(apiException, environment));
                    
                    options.OnSerializingProblemDetails(problemDetails, httpContext);

                    exceptionContext.Result = new ObjectResult(problemDetails);
                    exceptionContext.ExceptionHandled = true;
                }
            }
            base.OnException(exceptionContext);
        }

        [MemberNotNull(nameof(options), nameof(environment))]
        private void EnsureServicesInitialized(IServiceProvider serviceProvider)
        {
            options ??= serviceProvider.GetRequiredService<IOptions<ApiExceptionFilterOptions>>().Value;
            environment ??= serviceProvider.GetRequiredService<IHostEnvironment>();
        }
    }
}
