using Developist.Core.ArgumentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Developist.Core.Api.Exceptions.Filters;

/// <summary>
/// An exception filter attribute that handles unhandled exceptions by returning a <see cref="ProblemDetails"/> object as the response.
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
        _options = Ensure.Argument.NotNull(options).Value;
        _problemDetailsFactory = Ensure.Argument.NotNull(problemDetailsFactory);
    }

    /// <inheritdoc/>
    public bool IsReusable => true;

    /// <inheritdoc/>
    public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
    {
        return serviceProvider.GetService<GlobalExceptionFilterAttribute>() ?? new();
    }

    /// <inheritdoc/>
    public override void OnException(ExceptionContext exceptionContext)
    {
        var httpContext = exceptionContext.HttpContext;
        EnsureRequiredServices(httpContext.RequestServices);

        var problemDetails = exceptionContext.Exception switch
        {
            ValidationException exception => CreateValidationProblemDetails(httpContext, exceptionContext.ModelState, exception),
            ApiException exception => CreateApiProblemDetails(httpContext, exception),
            _ => CreateDefaultProblemDetails(httpContext, exceptionContext.Exception),
        };

        exceptionContext.Result = new ObjectResult(problemDetails);
        exceptionContext.ExceptionHandled = true;

        base.OnException(exceptionContext);
    }

    private void EnsureRequiredServices(IServiceProvider serviceProvider)
    {
        _options ??= serviceProvider.GetRequiredService<IOptions<GlobalExceptionFilterOptions>>().Value;
        _problemDetailsFactory ??= serviceProvider.GetRequiredService<ProblemDetailsFactory>();
    }

    private ProblemDetails CreateValidationProblemDetails(
        HttpContext httpContext,
        ModelStateDictionary modelState,
        ValidationException exception)
    {
        return _problemDetailsFactory!.CreateValidationProblemDetails(httpContext,
            modelStateDictionary: modelState,
            statusCode: (int)_options!.GetMappedStatusCodeOrDefault(exception, HttpStatusCode.UnprocessableEntity),
            detail: _options.ShouldDiscloseExceptionDetails(exception, httpContext)
                ? exception.GetDetailMessage()
                : exception.Message);
    }

    private ProblemDetails CreateApiProblemDetails(HttpContext httpContext, ApiException exception)
    {
        return _problemDetailsFactory!.CreateProblemDetails(httpContext,
            statusCode: (int)_options!.GetMappedStatusCodeOrDefault(exception, exception.StatusCode),
            title: exception.ReasonPhrase,
            type: exception.HelpLink.ToString(),
            detail: _options.ShouldDiscloseExceptionDetails(exception, httpContext)
                ? exception.GetDetailMessage()
                : exception.Message);
    }

    private ProblemDetails CreateDefaultProblemDetails(HttpContext httpContext, Exception exception)
    {
        return _problemDetailsFactory!.CreateProblemDetails(httpContext,
            statusCode: (int)_options!.GetMappedStatusCodeOrDefault(exception),
            detail: _options.ShouldDiscloseExceptionDetails(exception, httpContext)
                ? exception.GetDetailMessage()
                : exception.Message);
    }
}
