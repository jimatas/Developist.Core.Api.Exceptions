using Developist.Core.Api.Exceptions;
using Developist.Core.Api.ProblemDetails;
using Microsoft.AspNetCore.Http;

namespace Developist.Core.Api.MvcFilters;

/// <summary>
/// Represents the options that can be used to configure an <see cref="ApiExceptionFilterAttribute"/>.
/// </summary>
public class ApiExceptionFilterOptions
{
    /// <summary>
    /// Gets or sets a delegate that determines whether to handle the specified <see cref="ApiException"/>.
    /// </summary>
    public Func<ApiException, HttpContext, bool> ShouldHandleException { get; set; } = ApiExceptionFilterOptionsDefaults.ShouldHandleException;

    /// <summary>
    /// Gets or sets a delegate that determines whether to disclose exception details in the problem details response.
    /// </summary>
    public Func<ApiException, HttpContext, bool> ShouldDiscloseExceptionDetails { get; set; } = ApiExceptionFilterOptionsDefaults.ShouldDiscloseExceptionDetails;

    /// <summary>
    /// Gets or sets a delegate that executes when serializing an <see cref="ApiProblemDetails"/> object.
    /// </summary>
    public Action<ApiProblemDetails, ApiException, HttpContext> OnSerializingProblemDetails { get; set; } = ApiExceptionFilterOptionsDefaults.OnSerializingProblemDetails;
}
