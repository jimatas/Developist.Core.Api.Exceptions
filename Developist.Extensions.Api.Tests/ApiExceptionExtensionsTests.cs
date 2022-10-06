using Developist.Extensions.Api.Exceptions;

using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Developist.Extensions.Api.Tests
{
    [TestClass]
    public class ApiExceptionExtensionsTests
    {
        [TestMethod]
        public void GetDetailMessage_ByDefault_ReturnsMessage()
        {
            var exception = new ApiException(HttpStatusCode.InternalServerError, "There was a problem servicing your request.");

            var detailMessage = exception.GetDetailMessage();

            Assert.IsTrue(detailMessage.Contains("ApiException: There was a problem servicing your request."));
        }

        [TestMethod]
        public void GetDetailMessage_GivenApiExceptionWithInnerException_ReturnsInnerExceptionDetailsInMessage()
        {
            var exception = new ApiException(HttpStatusCode.InternalServerError, "There was a problem servicing your request.",
                innerException: new ValidationException("A required property was empty."));

            var detailMessage = exception.GetDetailMessage();

            Assert.IsTrue(detailMessage.Contains("ValidationException: A required property was empty."));
        }

        [TestMethod]
        public void GetDetailMessage_GivenApiExceptionWithNestedInnerExceptions_ReturnsAllInnerExceptionDetailsInMessage()
        {
            var exception = new ApiException(HttpStatusCode.InternalServerError, "There was a problem servicing your request.",
                innerException: new InvalidOperationException("One or more properties did not validate.",
                    innerException: new ValidationException("A required property was empty.")));

            var detailMessage = exception.GetDetailMessage();

            Assert.IsTrue(detailMessage.Contains("InvalidOperationException: One or more properties did not validate.") && detailMessage.Contains("ValidationException: A required property was empty."));
        }

        [TestMethod]
        public void GetDefaultReasonPhrase_GivenBadRequestException_ReturnsExpectedReasonPhrase()
        {
            var exception = new BadRequestException();

            var defaultReasonPhrase = exception.GetDefaultReasonPhrase();

            Assert.AreEqual("Bad Request", defaultReasonPhrase);
            Assert.AreEqual(exception.ReasonPhrase, defaultReasonPhrase);
        }

        [TestMethod]
        public void GetDefaultReasonPhrase_GivenUnauthorizedException_ReturnsExpectedReasonPhrase()
        {
            var exception = new UnauthorizedException();

            var defaultReasonPhrase = exception.GetDefaultReasonPhrase();

            Assert.AreEqual("Unauthorized", defaultReasonPhrase);
            Assert.AreEqual(exception.ReasonPhrase, defaultReasonPhrase);
        }

        [TestMethod]
        public void GetDefaultReasonPhrase_GivenForbiddenException_ReturnsExpectedReasonPhrase()
        {
            var exception = new ForbiddenException();

            var defaultReasonPhrase = exception.GetDefaultReasonPhrase();

            Assert.AreEqual("Forbidden", defaultReasonPhrase);
            Assert.AreEqual(exception.ReasonPhrase, defaultReasonPhrase);
        }

        [TestMethod]
        public void GetDefaultReasonPhrase_GivenNotFoundException_ReturnsExpectedReasonPhrase()
        {
            var exception = new NotFoundException();

            var defaultReasonPhrase = exception.GetDefaultReasonPhrase();

            Assert.AreEqual("Not Found", defaultReasonPhrase);
            Assert.AreEqual(exception.ReasonPhrase, defaultReasonPhrase);
        }
    }
}
