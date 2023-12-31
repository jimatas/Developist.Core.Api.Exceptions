using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;

namespace Developist.Core.Api.Exceptions.Filters.Tests;

[TestClass]
public class GlobalExceptionFilterAttributeTests
{
    [TestMethod]
    public void IsReusable_ByDefault_ReturnsTrue()
    {
        var globalExceptionFilter = new GlobalExceptionFilterAttribute();

        // Act
        var result = globalExceptionFilter.IsReusable;

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void OnException_InitializedUsingDefaultConstructor_ResolvesRequiredServices()
    {
        var problemDetailsFactoryMock = new Mock<ProblemDetailsFactory>();
        problemDetailsFactoryMock.Setup(factory => factory.CreateProblemDetails(
            It.IsAny<HttpContext>(),
            It.IsAny<int?>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>())).Verifiable();

        var globalExceptionFilterOptions = new GlobalExceptionFilterOptions
        {
            ShouldDiscloseExceptionDetails = (_, _) => true
        };

        var serviceProviderMock = new Mock<IServiceProvider>();
        serviceProviderMock.Setup(provider => provider.GetService(typeof(ProblemDetailsFactory)))
            .Returns(problemDetailsFactoryMock.Object).Verifiable();

        serviceProviderMock.Setup(provider => provider.GetService(typeof(IOptions<GlobalExceptionFilterOptions>)))
            .Returns(Options.Create(globalExceptionFilterOptions)).Verifiable();

        var actionContext = new ActionContext(
            new DefaultHttpContext
            {
                RequestServices = serviceProviderMock.Object,
            },
            new RouteData(),
            new ActionDescriptor());

        var exceptionContext = new ExceptionContext(actionContext, new List<IFilterMetadata>())
        {
            Exception = new ArgumentNullException()
        };

        var globalExceptionFilter = new GlobalExceptionFilterAttribute();

        // Act
        globalExceptionFilter.OnException(exceptionContext);

        // Assert
        Assert.IsTrue(exceptionContext.ExceptionHandled);
        problemDetailsFactoryMock.Verify();
        serviceProviderMock.Verify();
    }

    [DataTestMethod]
    [DataRow(false)]
    [DataRow(true)]
    public void OnException_GivenApiException_HandlesException(bool discloseExceptionDetails)
    {
        // Arrange
        var problemDetailsFactoryMock = new Mock<ProblemDetailsFactory>();
        problemDetailsFactoryMock.Setup(factory => factory.CreateProblemDetails(
            It.IsAny<HttpContext>(),
            It.IsAny<int?>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>())).Verifiable();

        var actionContext = new ActionContext(
            new DefaultHttpContext(),
            new RouteData(),
            new ActionDescriptor());

        var exceptionContext = new ExceptionContext(actionContext, new List<IFilterMetadata>())
        {
            Exception = new ApiException((HttpStatusCode)599)
        };

        var globalExceptionFilterOptions = new GlobalExceptionFilterOptions
        {
            ShouldDiscloseExceptionDetails = (_, _) => discloseExceptionDetails
        };

        var globalExceptionFilter = new GlobalExceptionFilterAttribute(
            Options.Create(globalExceptionFilterOptions),
            problemDetailsFactoryMock.Object);

        // Act
        globalExceptionFilter.OnException(exceptionContext);

        // Assert
        Assert.IsTrue(exceptionContext.ExceptionHandled);
        problemDetailsFactoryMock.Verify();
    }

    [DataTestMethod]
    [DataRow(typeof(ApplicationException), false)]
    [DataRow(typeof(ApplicationException), true)]
    [DataRow(typeof(ArgumentException), false)]
    [DataRow(typeof(ArgumentException), true)]
    [DataRow(typeof(NotSupportedException), false)]
    [DataRow(typeof(NotSupportedException), true)]
    public void OnException_GivenNonApiException_HandlesException(Type nonApiExceptionType, bool discloseExceptionDetails)
    {
        // Arrange
        var problemDetailsFactoryMock = new Mock<ProblemDetailsFactory>();
        problemDetailsFactoryMock.Setup(factory => factory.CreateProblemDetails(
            It.IsAny<HttpContext>(),
            It.IsAny<int?>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>())).Verifiable();

        var actionContext = new ActionContext(
            new DefaultHttpContext(),
            new RouteData(),
            new ActionDescriptor());

        var exceptionContext = new ExceptionContext(actionContext, new List<IFilterMetadata>())
        {
            Exception = (Exception)Activator.CreateInstance(nonApiExceptionType)!
        };

        var globalExceptionFilterOptions = new GlobalExceptionFilterOptions
        {
            ShouldDiscloseExceptionDetails = (_, _) => discloseExceptionDetails
        };

        var globalExceptionFilter = new GlobalExceptionFilterAttribute(
            Options.Create(globalExceptionFilterOptions),
            problemDetailsFactoryMock.Object);

        // Act
        globalExceptionFilter.OnException(exceptionContext);

        // Assert
        Assert.IsTrue(exceptionContext.ExceptionHandled);
        problemDetailsFactoryMock.Verify();
    }

    [DataTestMethod]
    [DataRow(false)]
    [DataRow(true)]
    public void OnException_GivenValidationException_HandlesException(bool discloseExceptionDetails)
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
            It.IsAny<string>())).Verifiable();

        var actionContext = new ActionContext(
            new DefaultHttpContext(),
            new RouteData(),
            new ActionDescriptor());

        var exceptionContext = new ExceptionContext(actionContext, new List<IFilterMetadata>())
        {
            Exception = new ValidationException("A required property was empty.")
        };

        var globalExceptionFilterOptions = new GlobalExceptionFilterOptions
        {
            ShouldDiscloseExceptionDetails = (_, _) => discloseExceptionDetails
        };

        var globalExceptionFilter = new GlobalExceptionFilterAttribute(
            Options.Create(globalExceptionFilterOptions),
            problemDetailsFactoryMock.Object);

        // Act
        globalExceptionFilter.OnException(exceptionContext);

        // Assert
        Assert.IsTrue(exceptionContext.ExceptionHandled);
        problemDetailsFactoryMock.Verify();
    }

    [DataTestMethod]
    [DataRow(false)]
    [DataRow(true)]
    public void OnException_GivenValidationExceptionWithValidationResult_HandlesException(bool discloseExceptionDetails)
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
            It.IsAny<string>())).Verifiable();

        var actionContext = new ActionContext(
            new DefaultHttpContext(),
            new RouteData(),
            new ActionDescriptor());

        var exceptionContext = new ExceptionContext(actionContext, new List<IFilterMetadata>())
        {
            Exception = new ValidationException(new ValidationResult("A required property was empty."), validatingAttribute: null, value: null)
        };

        var globalExceptionFilterOptions = new GlobalExceptionFilterOptions
        {
            ShouldDiscloseExceptionDetails = (_, _) => discloseExceptionDetails
        };

        var globalExceptionFilter = new GlobalExceptionFilterAttribute(
            Options.Create(globalExceptionFilterOptions),
            problemDetailsFactoryMock.Object);

        // Act
        globalExceptionFilter.OnException(exceptionContext);

        // Assert
        Assert.IsTrue(exceptionContext.ExceptionHandled);
        problemDetailsFactoryMock.Verify();
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
