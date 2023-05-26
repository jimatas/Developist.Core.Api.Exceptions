using Developist.Core.Api.Exceptions;
using System.Net;

namespace Developist.Core.Api.Tests;

[TestClass]
public class ApiExceptionTests
{
    [DataTestMethod]
    [DataRow(typeof(BadRequestException), HttpStatusCode.BadRequest)]
    [DataRow(typeof(ForbiddenException), HttpStatusCode.Forbidden)]
    [DataRow(typeof(NotFoundException), HttpStatusCode.NotFound)]
    [DataRow(typeof(UnauthorizedException), HttpStatusCode.Unauthorized)]
    public void ApiException_DefaultConstructor_SetsAllProperties(Type apiExceptionType, HttpStatusCode expectedStatusCode)
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
    [DataRow(typeof(ForbiddenException), "Forbidden", HttpStatusCode.Forbidden)]
    [DataRow(typeof(NotFoundException), "Not Found", HttpStatusCode.NotFound)]
    [DataRow(typeof(UnauthorizedException), "Unauthorized", HttpStatusCode.Unauthorized)]
    public void ApiException_MessageConstructor_SetsAllProperties(Type apiExceptionType, string message, HttpStatusCode expectedStatusCode)
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
    [DataRow(typeof(ForbiddenException), "Forbidden", HttpStatusCode.Forbidden)]
    [DataRow(typeof(NotFoundException), "Not Found", HttpStatusCode.NotFound)]
    [DataRow(typeof(UnauthorizedException), "Unauthorized", HttpStatusCode.Unauthorized)]
    public void ApiException_MessageAndInnerExceptionConstructor_SetsAllProperties(Type apiExceptionType, string message, HttpStatusCode expectedStatusCode)
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
}
