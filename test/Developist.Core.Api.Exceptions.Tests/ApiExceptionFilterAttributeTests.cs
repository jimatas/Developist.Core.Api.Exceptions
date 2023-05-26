using Developist.Core.Api.Exceptions;
using Developist.Core.Api.MvcFilters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using Moq;

namespace Developist.Core.Api.Tests;

[TestClass]
public class ApiExceptionFilterAttributeTests
{
    [TestMethod]
    public void OnException_GivenNonApiException_DoesNothing()
    {
        // Arrange
        var actionContext = new ActionContext(
            new DefaultHttpContext(),
            new RouteData(),
            new ActionDescriptor());

        var exceptionContext = new ExceptionContext(actionContext, new List<IFilterMetadata>())
        {
            Exception = new SystemException()
        };

        var apiExceptionFilter = new ApiExceptionFilterAttribute();

        // Act
        apiExceptionFilter.OnException(exceptionContext);

        // Assert
        Assert.IsFalse(exceptionContext.ExceptionHandled);
    }

    [DataTestMethod]
    [DataRow(typeof(BadRequestException))]
    [DataRow(typeof(ForbiddenException))]
    [DataRow(typeof(NotFoundException))]
    [DataRow(typeof(UnauthorizedException))]
    public void OnException_GiveApiException_HandlesIt(Type apiExceptionType)
    {
        // Arrange
        var actionContext = new ActionContext(
            new DefaultHttpContext(),
            new RouteData(),
            new ActionDescriptor());

        var exceptionContext = new ExceptionContext(actionContext, new List<IFilterMetadata>())
        {
            Exception = (ApiException)Activator.CreateInstance(apiExceptionType)!
        };

        var apiExceptionFilterOptions = new ApiExceptionFilterOptions
        {
            ShouldDiscloseExceptionDetails = (_, _) => true
        };

        var apiExceptionFilter = new ApiExceptionFilterAttribute(Options.Create(apiExceptionFilterOptions));

        // Act
        apiExceptionFilter.OnException(exceptionContext);

        // Assert
        Assert.IsTrue(exceptionContext.ExceptionHandled);
    }

    [TestMethod]
    public void CreateInstance_WithFilterConfigured_ReturnsThatFilter()
    {
        // Arrange
        var apiExceptionFilter = new ApiExceptionFilterAttribute();

        var serviceProviderMock = new Mock<IServiceProvider>();
        serviceProviderMock.Setup(serviceProvider => serviceProvider.GetService(typeof(ApiExceptionFilterAttribute)))
            .Returns(apiExceptionFilter);

        // Act
        var filterMetadata = apiExceptionFilter.CreateInstance(serviceProviderMock.Object);

        // Assert
        Assert.AreEqual(apiExceptionFilter, filterMetadata);
    }

    [TestMethod]
    public void CreateInstance_WithNoFilterConfigured_CreatesNewFilter()
    {
        // Arrange
        var apiExceptionFilter = new ApiExceptionFilterAttribute();
        var serviceProviderMock = new Mock<IServiceProvider>();

        // Act
        var filterMetadata = apiExceptionFilter.CreateInstance(serviceProviderMock.Object);

        // Assert
        Assert.IsNotNull(filterMetadata);
        Assert.AreNotSame(apiExceptionFilter, filterMetadata);
    }
}
