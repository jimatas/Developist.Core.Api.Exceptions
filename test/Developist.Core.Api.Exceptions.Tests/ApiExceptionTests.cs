namespace Developist.Core.Api.Exceptions.Tests;

[TestClass]
public class ApiExceptionTests
{
    [DataTestMethod]
    [DataRow((HttpStatusCode)int.MinValue)]
    [DataRow((HttpStatusCode)int.MaxValue)]
    public void ApiException_InitializedWithNonErrorStatusCode_ThrowsArgumentOutOfRangeException(HttpStatusCode invalidStatusCode)
    {
        // Arrange

        // Act
        void action() => _ = new ApiException(invalidStatusCode);

        // Assert
        var exception = Assert.ThrowsException<ArgumentOutOfRangeException>(action);
        StringAssert.Contains(exception.Message, "Value does not indicate an HTTP error status.");
    }

    [DataTestMethod]
    [DataRow(typeof(BadRequestException), HttpStatusCode.BadRequest)]
    [DataRow(typeof(ConflictException), HttpStatusCode.Conflict)]
    [DataRow(typeof(ForbiddenException), HttpStatusCode.Forbidden)]
    [DataRow(typeof(NotFoundException), HttpStatusCode.NotFound)]
    [DataRow(typeof(TooManyRequestsException), HttpStatusCode.TooManyRequests)]
    [DataRow(typeof(UnauthorizedException), HttpStatusCode.Unauthorized)]
    [DataRow(typeof(UnprocessableEntityException), HttpStatusCode.UnprocessableEntity)]
    public void ApiExceptionTypes_InitializedWithDefaultConstructor_SetsExpectedStatusCodeAndProperties(Type apiExceptionType, HttpStatusCode expectedStatusCode)
    {
        // Arrange
        
        // Act
        var exception = (ApiException)Activator.CreateInstance(apiExceptionType)!;

        // Assert
        Assert.AreEqual(expectedStatusCode, exception.StatusCode);
        Assert.IsFalse(string.IsNullOrEmpty(exception.ReasonPhrase));
        Assert.IsNotNull(exception.HelpLink);
        Assert.AreNotEqual(new Uri("about:blank"), exception.HelpLink);
    }

    [DataTestMethod]
    [DataRow(typeof(BadRequestException), "Bad Request", HttpStatusCode.BadRequest)]
    [DataRow(typeof(ConflictException), "Conflict", HttpStatusCode.Conflict)]
    [DataRow(typeof(ForbiddenException), "Forbidden", HttpStatusCode.Forbidden)]
    [DataRow(typeof(NotFoundException), "Not Found", HttpStatusCode.NotFound)]
    [DataRow(typeof(TooManyRequestsException), "Too Many Requests", HttpStatusCode.TooManyRequests)]
    [DataRow(typeof(UnauthorizedException), "Unauthorized", HttpStatusCode.Unauthorized)]
    [DataRow(typeof(UnprocessableEntityException), "Unprocessable Entity", HttpStatusCode.UnprocessableEntity)]
    public void ApiExceptionTypes_InitializedWithCustomMessage_SetsMessageAndStatusCodeCorrectly(Type apiExceptionType, string message, HttpStatusCode expectedStatusCode)
    {
        // Arrange

        // Act
        var exception = (ApiException)Activator.CreateInstance(apiExceptionType, message)!;

        // Assert
        Assert.AreEqual(message, exception.Message);
        Assert.AreEqual(expectedStatusCode, exception.StatusCode);
        Assert.IsFalse(string.IsNullOrEmpty(exception.ReasonPhrase));
        Assert.IsNotNull(exception.HelpLink);
        Assert.AreNotEqual(new Uri("about:blank"), exception.HelpLink);
    }

    [DataTestMethod]
    [DataRow(typeof(BadRequestException), "Bad Request", HttpStatusCode.BadRequest)]
    [DataRow(typeof(ConflictException), "Conflict", HttpStatusCode.Conflict)]
    [DataRow(typeof(ForbiddenException), "Forbidden", HttpStatusCode.Forbidden)]
    [DataRow(typeof(NotFoundException), "Not Found", HttpStatusCode.NotFound)]
    [DataRow(typeof(TooManyRequestsException), "Too Many Requests", HttpStatusCode.TooManyRequests)]
    [DataRow(typeof(UnauthorizedException), "Unauthorized", HttpStatusCode.Unauthorized)]
    [DataRow(typeof(UnprocessableEntityException), "Unprocessable Entity", HttpStatusCode.UnprocessableEntity)]
    public void ApiExceptionTypes_InitializedWithMessageAndInnerException_SetsPropertiesCorrectly(Type apiExceptionType, string message, HttpStatusCode expectedStatusCode)
    {
        // Arrange

        // Act
        var exception = (ApiException)Activator.CreateInstance(apiExceptionType, message, new Exception("Inner Exception"))!;

        // Assert
        Assert.AreEqual(message, exception.Message);
        Assert.IsNotNull(exception.InnerException);
        Assert.AreEqual(expectedStatusCode, exception.StatusCode);
        Assert.IsFalse(string.IsNullOrEmpty(exception.ReasonPhrase));
        Assert.IsNotNull(exception.HelpLink);
        Assert.AreNotEqual(new Uri("about:blank"), exception.HelpLink);
    }

    [DataTestMethod]
    [DataRow(0)]
    [DataRow(3)]
    [DataRow(5)]
    public void TooManyRequestsException_InitializedWithRetryAfter_SetsRetryAfterCorrectly(int seconds)
    {
        // Arrange
        var retryAfter = TimeSpan.FromSeconds(seconds);

        // Act
        var exception = new TooManyRequestsException { RetryAfter = retryAfter };

        // Assert
        Assert.AreEqual(retryAfter, exception.RetryAfter);
    }

    [TestMethod]
    public void TooManyRequestsException_InitializedWithoutRetryAfter_RetryAfterRemainsNull()
    {
        // Arrange

        // Act
        var exception = new TooManyRequestsException();

        // Assert
        Assert.IsNull(exception.RetryAfter);
    }
}
