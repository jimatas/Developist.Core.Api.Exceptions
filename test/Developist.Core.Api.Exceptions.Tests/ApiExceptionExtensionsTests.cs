using Developist.Core.Api.Exceptions;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Developist.Core.Api.Tests;

[TestClass]
public class ApiExceptionExtensionsTests
{
    [TestMethod]
    public void GetDetailMessage_ByDefault_ReturnsMessage()
    {
        // Arrange
        var exception = new ApiException(HttpStatusCode.InternalServerError, "There was a problem servicing your request.");

        // Act
        var detailMessage = exception.GetDetailMessage();

        // Assert
        StringAssert.Contains(detailMessage, "ApiException: There was a problem servicing your request.");
    }

    [TestMethod]
    public void GetDetailMessage_GivenApiExceptionWithInnerException_ReturnsInnerExceptionDetailsInMessage()
    {
        // Arrange
        var exception = new ApiException(HttpStatusCode.InternalServerError, "There was a problem servicing your request.",
            innerException: new ValidationException("A required property was empty."));

        // Act
        var detailMessage = exception.GetDetailMessage();

        // Assert
        StringAssert.Contains(detailMessage, "ValidationException: A required property was empty.");
    }

    [TestMethod]
    public void GetDetailMessage_GivenApiExceptionWithNestedInnerExceptions_ReturnsAllInnerExceptionDetailsInMessage()
    {
        // Arrange
        var exception = new ApiException(HttpStatusCode.InternalServerError, "There was a problem servicing your request.",
            innerException: new InvalidOperationException("One or more properties did not validate.",
                innerException: new ValidationException("A required property was empty.")));

        // Act
        var detailMessage = exception.GetDetailMessage();

        // Assert
        StringAssert.Contains(detailMessage, "InvalidOperationException: One or more properties did not validate.");
        StringAssert.Contains(detailMessage, "ValidationException: A required property was empty.");
    }

    [TestMethod]
    public void GetDetailMessage_GivenApiExceptionWithStackTrace_ReturnsItInMessage()
    {
        // Arrange
        var exception = new CustomStackTraceApiException(HttpStatusCode.InternalServerError,
            message: "There was a problem servicing your request.",
            stackTrace: "CustomStackTraceException: There was a problem servicing your request.\r\nat CustomStackTraceException...");

        // Act
        var detailMessage = exception.GetDetailMessage();

        // Assert
        StringAssert.Contains(detailMessage, "CustomStackTraceException: There was a problem servicing your request.\r\nat CustomStackTraceException...");
    }

    [TestMethod]
    public void GetDefaultReasonPhrase_GivenBadRequestException_ReturnsExpectedReasonPhrase()
    {
        // Arrange
        var exception = new BadRequestException();

        // Act
        var defaultReasonPhrase = exception.GetDefaultReasonPhrase();

        // Assert
        Assert.AreEqual("Bad Request", defaultReasonPhrase);
        Assert.AreEqual(defaultReasonPhrase, exception.ReasonPhrase);
    }

    [TestMethod]
    public void GetDefaultReasonPhrase_GivenUnauthorizedException_ReturnsExpectedReasonPhrase()
    {
        // Arrange
        var exception = new UnauthorizedException();

        // Act
        var defaultReasonPhrase = exception.GetDefaultReasonPhrase();

        // Assert
        Assert.AreEqual("Unauthorized", defaultReasonPhrase);
        Assert.AreEqual(defaultReasonPhrase, exception.ReasonPhrase);
    }

    [TestMethod]
    public void GetDefaultReasonPhrase_GivenForbiddenException_ReturnsExpectedReasonPhrase()
    {
        // Arrange
        var exception = new ForbiddenException();

        // Act
        var defaultReasonPhrase = exception.GetDefaultReasonPhrase();

        // Assert
        Assert.AreEqual("Forbidden", defaultReasonPhrase);
        Assert.AreEqual(defaultReasonPhrase, exception.ReasonPhrase);
    }

    [TestMethod]
    public void GetDefaultReasonPhrase_GivenNotFoundException_ReturnsExpectedReasonPhrase()
    {
        // Arrange
        var exception = new NotFoundException();

        // Act
        var defaultReasonPhrase = exception.GetDefaultReasonPhrase();

        // Assert
        Assert.AreEqual("Not Found", defaultReasonPhrase);
        Assert.AreEqual(defaultReasonPhrase, exception.ReasonPhrase);
    }

    [TestMethod]
    public void GetDefaultReasonPhrase_ForUndefinedErrorStatusCode_ReturnsEmptyReasonPhrase()
    {
        // Arrange
        var exception = new ApiException((HttpStatusCode)599);

        // Act
        var defaultReasonPhrase = exception.GetDefaultReasonPhrase();

        // Assert
        Assert.IsTrue(string.IsNullOrEmpty(defaultReasonPhrase));
    }

    [TestMethod]
    [DynamicData(nameof(ErrorStatusCodes))]
    public void GetDefaultReasonPhrase_ForAnyErrorStatusCode_ReturnsReasonPhrase(HttpStatusCode errorStatusCode)
    {
        // Arrange
        var exception = new ApiException(errorStatusCode);

        // Act
        var defaultReasonPhrase = exception.GetDefaultReasonPhrase();

        // Assert
        Assert.IsFalse(string.IsNullOrEmpty(defaultReasonPhrase));
    }

    [TestMethod]
    public void GetDefaultHelpLink_ForUndefinedErrorStatusCode_ReturnsEmptyHelpLink()
    {
        // Arrange
        var exception = new ApiException((HttpStatusCode)599);

        // Act
        var defaultHelpLink = exception.GetDefaultHelpLink();

        // Assert
        Assert.AreEqual(new Uri("about:blank"), defaultHelpLink);
    }

    [DataTestMethod]
    [DynamicData(nameof(ErrorStatusCodes))]
    public void GetDefaultHelpLink_ForAnyErrorStatusCode_ReturnsHelpLink(HttpStatusCode errorStatusCode)
    {
        // Arrange
        var exception = new ApiException(errorStatusCode);

        // Act
        var defaultHelpLink = exception.GetDefaultHelpLink();

        // Assert
        Assert.IsNotNull(defaultHelpLink);
        Assert.AreNotEqual(new Uri("about:blank"), defaultHelpLink);
    }

    private static IEnumerable<object[]> ErrorStatusCodes => Enumerable
        .Range(400, 200)
        .Where(statusCode => Enum.IsDefined(typeof(HttpStatusCode), statusCode))
        .Select(statusCode => new object[] { (HttpStatusCode)statusCode });

    private class CustomStackTraceApiException : ApiException
    {
        public CustomStackTraceApiException(HttpStatusCode statusCode, string message, string stackTrace)
            : base(statusCode, message) => StackTrace = stackTrace;

        public override string? StackTrace { get; }
    }
}
