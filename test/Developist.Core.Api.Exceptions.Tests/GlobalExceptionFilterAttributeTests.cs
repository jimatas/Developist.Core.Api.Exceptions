using Developist.Core.Api.Exceptions;
using Developist.Core.Api.MvcFilters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using Moq;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Developist.Core.Api.Tests;

[TestClass]
public class GlobalExceptionFilterAttributeTests
{
    [TestMethod]
    public void OnException_GivenApiExceptionAndApiExceptionFilterRegistered_DoesNotHandleIt()
    {
        // Arrange
        var problemDetailsFactoryMock = new Mock<ProblemDetailsFactory>();

        var actionContext = new ActionContext(
            new DefaultHttpContext(),
            new RouteData(),
            new ActionDescriptor());

        var filters = new List<IFilterMetadata>
        {
            new ApiExceptionFilterAttribute()
        };

        var exceptionContext = new ExceptionContext(actionContext, filters)
        {
            Exception = new ApiException((HttpStatusCode)599)
        };

        var globalExceptionFilterOptions = new GlobalExceptionFilterOptions();

        var globalExceptionFilter = new GlobalExceptionFilterAttribute(
            Options.Create(globalExceptionFilterOptions),
            problemDetailsFactoryMock.Object);

        // Act
        globalExceptionFilter.OnException(exceptionContext);

        // Assert
        Assert.IsFalse(exceptionContext.ExceptionHandled);
        problemDetailsFactoryMock.VerifyNoOtherCalls();
    }

    [TestMethod]
    public void OnException_GivenApiExceptionAndNoApiExceptionFilterRegistered_HandlesIt()
    {
        // Arrange
        var problemDetailsFactoryMock = new Mock<ProblemDetailsFactory>();
        problemDetailsFactoryMock.Setup(factory => factory.CreateProblemDetails(
            It.IsAny<HttpContext>(),
            It.IsAny<int?>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>()));

        var actionContext = new ActionContext(
            new DefaultHttpContext(),
            new RouteData(),
            new ActionDescriptor());

        var exceptionContext = new ExceptionContext(actionContext, new List<IFilterMetadata>())
        {
            Exception = new ApiException((HttpStatusCode)599)
        };

        var globalExceptionFilterOptions = new GlobalExceptionFilterOptions();

        var globalExceptionFilter = new GlobalExceptionFilterAttribute(
            Options.Create(globalExceptionFilterOptions),
            problemDetailsFactoryMock.Object);

        // Act
        globalExceptionFilter.OnException(exceptionContext);

        // Assert
        Assert.IsTrue(exceptionContext.ExceptionHandled);
        problemDetailsFactoryMock.Verify(factory => factory.CreateProblemDetails(
            It.IsAny<HttpContext>(),
            It.IsAny<int?>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>()));
    }

    [DataTestMethod]
    [DataRow(typeof(ApplicationException))]
    [DataRow(typeof(ArgumentException))]
    [DataRow(typeof(NotSupportedException))]
    public void OnException_GivenNonApiException_HandlesIt(Type nonApiExceptionType)
    {
        // Arrange
        var problemDetailsFactoryMock = new Mock<ProblemDetailsFactory>();
        problemDetailsFactoryMock.Setup(factory => factory.CreateProblemDetails(
            It.IsAny<HttpContext>(),
            It.IsAny<int?>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>()));

        var actionContext = new ActionContext(
            new DefaultHttpContext(),
            new RouteData(),
            new ActionDescriptor());

        var exceptionContext = new ExceptionContext(actionContext, new List<IFilterMetadata>())
        {
            Exception = (Exception)Activator.CreateInstance(nonApiExceptionType)!
        };

        var globalExceptionFilterOptions = new GlobalExceptionFilterOptions();

        var globalExceptionFilter = new GlobalExceptionFilterAttribute(
            Options.Create(globalExceptionFilterOptions),
            problemDetailsFactoryMock.Object);

        // Act
        globalExceptionFilter.OnException(exceptionContext);

        // Assert
        Assert.IsTrue(exceptionContext.ExceptionHandled);
        problemDetailsFactoryMock.Verify(factory => factory.CreateProblemDetails(
            It.IsAny<HttpContext>(),
            It.IsAny<int?>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>()));
    }

    [TestMethod]
    public void OnException_GivenValidationException_HandlesIt()
    {
        // Arrange
        var problemDetailsFactoryMock = new Mock<ProblemDetailsFactory>();
        problemDetailsFactoryMock.Setup(factory => factory.CreateValidationProblemDetails(
            It.IsAny<HttpContext>(),
            It.IsAny<ModelStateDictionary>(),
            It.IsAny<int?>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>()));

        var actionContext = new ActionContext(
            new DefaultHttpContext(),
            new RouteData(),
            new ActionDescriptor());

        var exceptionContext = new ExceptionContext(actionContext, new List<IFilterMetadata>())
        {
            Exception = new ValidationException()
        };

        var globalExceptionFilterOptions = new GlobalExceptionFilterOptions();

        var globalExceptionFilter = new GlobalExceptionFilterAttribute(
            Options.Create(globalExceptionFilterOptions),
            problemDetailsFactoryMock.Object);

        // Act
        globalExceptionFilter.OnException(exceptionContext);

        // Assert
        Assert.IsTrue(exceptionContext.ExceptionHandled);
        problemDetailsFactoryMock.Verify(factory => factory.CreateValidationProblemDetails(
            It.IsAny<HttpContext>(),
            It.IsAny<ModelStateDictionary>(),
            It.IsAny<int?>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>()));
    }

    [TestMethod]
    public void CreateInstance_WithFilterConfigured_ReturnsThatFilter()
    {
        // Arrange
        var globalExceptionFilter = new GlobalExceptionFilterAttribute();

        var serviceProviderMock = new Mock<IServiceProvider>();
        serviceProviderMock.Setup(serviceProvider => serviceProvider.GetService(typeof(GlobalExceptionFilterAttribute)))
            .Returns(globalExceptionFilter);

        // Act
        var filterMetadata = globalExceptionFilter.CreateInstance(serviceProviderMock.Object);

        // Assert
        Assert.AreEqual(globalExceptionFilter, filterMetadata);
    }

    [TestMethod]
    public void CreateInstance_WithNoFilterConfigured_CreatesNewFilter()
    {
        // Arrange
        var globalExceptionFilter = new GlobalExceptionFilterAttribute();
        var serviceProviderMock = new Mock<IServiceProvider>();

        // Act
        var filterMetadata = globalExceptionFilter.CreateInstance(serviceProviderMock.Object);

        // Assert
        Assert.IsNotNull(filterMetadata);
        Assert.AreNotSame(globalExceptionFilter, filterMetadata);
    }
}
