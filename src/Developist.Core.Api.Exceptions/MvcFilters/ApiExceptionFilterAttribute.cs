using Developist.Core.Api.Exceptions;
using Developist.Core.Api.ProblemDetails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;

namespace Developist.Core.Api.MvcFilters;

/// <summary>
/// An exception filter attribute that handles <see cref="ApiException"/> objects by returning an <see cref="ApiProblemDetails"/> object as the response.
/// </summary>
public class ApiExceptionFilterAttribute : ExceptionFilterAttribute, IFilterFactory
{
    private ApiExceptionFilterOptions? _options;

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiExceptionFilterAttribute"/> class.
    /// </summary>
    public ApiExceptionFilterAttribute() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiExceptionFilterAttribute"/> class using the specified options.
    /// </summary>
    /// <param name="options">The options used to configure the filter.</param>
    [ActivatorUtilitiesConstructor]
    public ApiExceptionFilterAttribute(IOptions<ApiExceptionFilterOptions> options)
    {
        _options = options?.Value;
    }

    /// <inheritdoc/>
    [ExcludeFromCodeCoverage(Justification = "Just a simple, hard-coded property.")]
    public bool IsReusable => true;

    /// <inheritdoc/>
    public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
    {
        return serviceProvider.GetService<ApiExceptionFilterAttribute>() ?? new();
    }

    /// <inheritdoc/>
    public override void OnException(ExceptionContext exceptionContext)
    {
        if (!exceptionContext.ExceptionHandled && exceptionContext.Exception is ApiException apiException)
        {
            var httpContext = exceptionContext.HttpContext;
            var getOptions = httpContext.RequestServices.GetRequiredService<IOptions<ApiExceptionFilterOptions>>;

            _options ??= getOptions().Value;

            if (_options.ShouldHandleException(apiException, httpContext))
            {
                var problemDetails = apiException.ToProblemDetails(
                    _options.ShouldDiscloseExceptionDetails(apiException, httpContext));

                _options.OnSerializingProblemDetails(problemDetails, apiException, httpContext);

                exceptionContext.Result = new ObjectResult(problemDetails);
                exceptionContext.ExceptionHandled = true;
            }
        }

        base.OnException(exceptionContext);
    }
}
