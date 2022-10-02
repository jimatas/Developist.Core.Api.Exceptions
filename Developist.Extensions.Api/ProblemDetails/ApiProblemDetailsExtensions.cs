using Developist.Extensions.Api.Exceptions;

namespace Developist.Extensions.Api.ProblemDetails
{
    public static class ApiProblemDetailsExtensions
    {
        public static ApiProblemDetails ToProblemDetails(this ApiException exception, bool discloseExceptionDetails = false)
        {
            var problemDetails = new ApiProblemDetails
            {
                Status = exception.StatusCode,
                Title = exception.ReasonPhrase,
                Type = exception.HelpLink,
                Detail = discloseExceptionDetails ? exception.DetailMessage() : exception.Message
            };

            return problemDetails;
        }
    }
}
