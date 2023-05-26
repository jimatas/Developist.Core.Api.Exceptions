using Developist.Core.Api.Exceptions;
using Developist.Core.Api.ProblemDetails;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;

namespace Developist.Core.Api.MvcFilters;

/// <summary>
/// Provides default values for the properties of an <see cref="ApiExceptionFilterOptions"/> instance.
/// </summary>
public static class ApiExceptionFilterOptionsDefaults
{
    /// <summary>
    /// The default function that determines whether an <see cref="ApiException"/> should be handled by the <see cref="ApiExceptionFilterAttribute"/> instance.
    /// </summary>
    public static readonly Func<ApiException, HttpContext, bool> ShouldHandleException = (_, _) => true;

    /// <summary>
    /// The default function that determines whether the details of an <see cref="ApiException"/> should be disclosed in the <see cref="ApiProblemDetails"/> response.
    /// </summary>
    public static readonly Func<ApiException, HttpContext, bool> ShouldDiscloseExceptionDetails = (_, ctx) =>
    {
        return ctx.RequestServices.GetRequiredService<IHostEnvironment>().IsDevelopment();
    };

    /// <summary>
    /// The default action that is executed during serialization of the <see cref="ApiProblemDetails"/>.
    /// </summary>
    public static readonly Action<ApiProblemDetails, ApiException, HttpContext> OnSerializingProblemDetails = (prob, _, ctx) =>
    {
        prob.Extensions["traceId"] = Activity.Current?.Id ?? ctx.TraceIdentifier;
    };
}
