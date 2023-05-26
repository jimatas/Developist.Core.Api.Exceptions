using Developist.Core.Api.ProblemDetails.Serialization;
using System.Net;
using System.Text.Json.Serialization;
using ApiProblemDetailsJsonNetConverter = Developist.Core.Api.ProblemDetails.Serialization.Newtonsoft.ApiProblemDetailsJsonConverter;

namespace Developist.Core.Api.ProblemDetails;

/// <summary>
/// Represents a problem details object that provides additional information about an API error.
/// </summary>
[JsonConverter(typeof(ApiProblemDetailsJsonConverter))]
[JsonNet.JsonConverter(typeof(ApiProblemDetailsJsonNetConverter))]
public class ApiProblemDetails : MvcProblemDetails
{
    /// <summary>
    /// Gets or sets the type of the problem.
    /// </summary>
    public new Uri? Type
    {
        get => base.Type is null ? null : new Uri(base.Type);
        set => base.Type = value?.ToString();
    }

    /// <summary>
    /// Gets or sets the HTTP status code associated with the problem.
    /// </summary>
    public new HttpStatusCode? Status
    {
        get => base.Status is null ? null : (HttpStatusCode)base.Status;
        set => base.Status = value is null ? null : (int)value;
    }

    /// <summary>
    /// Gets or sets the URI that identifies the specific occurrence of the problem.
    /// </summary>
    public new Uri? Instance
    {
        get => base.Instance is null ? null : new Uri(base.Instance);
        set => base.Instance = value?.ToString();
    }
}
