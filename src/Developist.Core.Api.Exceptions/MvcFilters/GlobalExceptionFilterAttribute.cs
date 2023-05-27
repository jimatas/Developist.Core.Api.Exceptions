using Developist.Core.Api.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace Developist.Core.Api.MvcFilters;

/// <summary>
/// An exception filter attribute that handles unhandled exceptions by returning an <see cref="MvcProblemDetails"/> object as the response.
/// </summary>
public class GlobalExceptionFilterAttribute : ExceptionFilterAttribute, IFilterFactory
{
    private GlobalExceptionFilterOptions? _options;
    private ProblemDetailsFactory? _problemDetailsFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="GlobalExceptionFilterAttribute"/> class.
    /// </summary>
    public GlobalExceptionFilterAttribute() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="GlobalExceptionFilterAttribute"/> class using the specified options and problem details factory.
    /// </summary>
    /// <param name="options">The options used to configure the filter.</param>
    /// <param name="problemDetailsFactory">The factory used to create problem details objects.</param>
    [ActivatorUtilitiesConstructor]
    public GlobalExceptionFilterAttribute(
        IOptions<GlobalExceptionFilterOptions> options,
        ProblemDetailsFactory problemDetailsFactory)
    {
        _options = options?.Value;
        _problemDetailsFactory = problemDetailsFactory;
    }

    /// <inheritdoc/>
    [ExcludeFromCodeCoverage(Justification = "Just a simple, hard-coded property.")]
    public bool IsReusable => true;

    /// <inheritdoc/>
    public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
    {
        return serviceProvider.GetService<GlobalExceptionFilterAttribute>() ?? new();
    }

    /// <inheritdoc/>
    public override void OnException(ExceptionContext exceptionContext)
    {
        if (!exceptionContext.ExceptionHandled &&
            // Ensure that either the exception is not an ApiException or there is no registered ApiExceptionFilterAttribute to handle it.
            (exceptionContext.Exception is not ApiException || exceptionContext.FindEffectivePolicy<ApiExceptionFilterAttribute>() is null))
        {
            var httpContext = exceptionContext.HttpContext;
            var getOptions = httpContext.RequestServices.GetRequiredService<IOptions<GlobalExceptionFilterOptions>>;
            var getProblemDetailsFactory = httpContext.RequestServices.GetRequiredService<ProblemDetailsFactory>;

            _options ??= getOptions().Value;
            _problemDetailsFactory ??= getProblemDetailsFactory();

            MvcProblemDetails problemDetails;
            if (exceptionContext.Exception is ValidationException validationException)
            {
                problemDetails = _problemDetailsFactory.CreateValidationProblemDetails(httpContext,
                    modelStateDictionary: exceptionContext.ModelState,
                    statusCode: (int)_options.GetMappedStatusCodeOrDefault(validationException, HttpStatusCode.BadRequest),
                    detail: validationException.ValidationResult.ErrorMessage ?? validationException.Message);
            }
            else
            {
                var exception = exceptionContext.Exception;
                problemDetails = _problemDetailsFactory.CreateProblemDetails(httpContext,
                    statusCode: (int)_options.GetMappedStatusCodeOrDefault(exception),
                    detail: exception.Message);
            }

            exceptionContext.Result = new ObjectResult(problemDetails);
            exceptionContext.ExceptionHandled = true;
        }

        base.OnException(exceptionContext);
    }
}
