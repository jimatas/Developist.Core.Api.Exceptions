using Developist.Extensions.Api.ProblemDetails.Serialization;

using System.Net;
using System.Text.Json.Serialization;

namespace Developist.Extensions.Api.ProblemDetails
{
    [JsonConverter(typeof(ApiProblemDetailsJsonConverter))]
    public class ApiProblemDetails
    {
        public Uri? Type { get; internal set; }
        public string? Title { get; internal set; }
        public HttpStatusCode? Status { get; internal set; }
        public string? Detail { get; internal set; }
        public Uri? Instance { get; internal set; }

        [JsonExtensionData]
        public IDictionary<string, object?> Extensions { get; } = new Dictionary<string, object?>();
    }
}
