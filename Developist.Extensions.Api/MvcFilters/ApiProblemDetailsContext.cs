﻿using Developist.Extensions.Api.Exceptions;
using Developist.Extensions.Api.ProblemDetails;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace Developist.Extensions.Api.MvcFilters
{
    public class ApiProblemDetailsContext : ApiExceptionContext
    {
        internal ApiProblemDetailsContext(
            ApiProblemDetails problemDetails,
            ApiException exception,
            IHostEnvironment environment,
            HttpContext httpContext) : base(exception, environment, httpContext)
        {
            ProblemDetails = problemDetails;
        }

        public ApiProblemDetails ProblemDetails { get; }
    }
}
