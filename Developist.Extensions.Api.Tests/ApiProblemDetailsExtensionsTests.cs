using Developist.Extensions.Api.Exceptions;
using Developist.Extensions.Api.ProblemDetails;

using System.ComponentModel.DataAnnotations;

namespace Developist.Extensions.Api.Tests
{
    [TestClass]
    public class ApiProblemDetailsExtensionsTests
    {
        [TestMethod]
        public void ToProblemDetails_ByDefault_SetsAllPropertiesToExpectedValues()
        {
            var exception = new ForbiddenException("You do not have permission to view this page.");

            var problemDetails = exception.ToProblemDetails();

            Assert.AreEqual(exception.StatusCode, problemDetails.Status);
            Assert.AreEqual(exception.ReasonPhrase, problemDetails.Title);
            Assert.AreEqual(exception.HelpLink, problemDetails.Type);
            Assert.AreEqual(exception.Message, problemDetails.Detail);
        }

        [TestMethod]
        public void ToProblemDetails_GivenApiExceptionWithInnerException_DoesNotDiscloseDetailsByDefault()
        {
            var exception = new BadRequestException("One or more validation errors occurred.", new ValidationException("The 'Id' property cannot be null or empty."));

            var problemDetails = exception.ToProblemDetails();

            Assert.AreEqual(exception.Message, problemDetails.Detail);
        }

        [TestMethod]
        public void ToProblemDetails_GivenApiExceptionWithInnerException_DisclosesDetailsAsRequested()
        {
            var exception = new BadRequestException("One or more validation errors occurred.", new ValidationException("The 'Id' property cannot be null or empty."));

            var problemDetails = exception.ToProblemDetails(discloseExceptionDetails: true);

            Assert.AreEqual(exception.DetailMessage(), problemDetails.Detail);
        }
    }
}
