using Developist.Extensions.Api.ProblemDetails.Serialization;

using System.Net;
using System.Text.Json.Serialization;

using MvcProblemDetails = Microsoft.AspNetCore.Mvc.ProblemDetails;

namespace Developist.Extensions.Api.ProblemDetails
{
    [JsonConverter(typeof(ApiProblemDetailsJsonConverter))]
    public class ApiProblemDetails : MvcProblemDetails
    {
        public new Uri? Type
        {
            get => base.Type is null ? null : new Uri(base.Type);
            set => base.Type = value?.ToString();
        }

        public new HttpStatusCode? Status
        {
            get => base.Status is null ? null : (HttpStatusCode)base.Status;
            set => base.Status = value is null ? null : (int)value;
        }

        public new Uri? Instance
        {
            get => base.Instance is null ? null : new Uri(base.Instance);
            set => base.Instance = value?.ToString();
        }
    }
}
