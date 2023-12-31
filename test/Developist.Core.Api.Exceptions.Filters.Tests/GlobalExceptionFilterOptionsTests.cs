using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace Developist.Core.Api.Exceptions.Filters.Tests;

[TestClass]
public class GlobalExceptionFilterOptionsTests
{
    [DataTestMethod]
    [DataRow(false)]
    [DataRow(true)]
    public void ShouldDiscloseExceptionDetails_ByDefault_ChecksHostingEnvironment(bool isDevelopment)
    {
        // Arrange
        var hostEnvironmentMock = new Mock<IHostEnvironment>();
        hostEnvironmentMock.SetupGet(host => host.EnvironmentName)
            .Returns(isDevelopment ? Environments.Development : Environments.Production);

        var serviceProviderMock = new Mock<IServiceProvider>();
        serviceProviderMock.Setup(provider => provider.GetService(typeof(IHostEnvironment)))
            .Returns(hostEnvironmentMock.Object);

        var options = new GlobalExceptionFilterOptions();
        var httpContext = new DefaultHttpContext
        {
            RequestServices = serviceProviderMock.Object
        };

        var exception = new Exception();

        // Act
        var result = options.ShouldDiscloseExceptionDetails(exception, httpContext);

        // Assert
        Assert.AreEqual(isDevelopment, result);
    }

    [TestMethod]
    public void MapExceptionToStatusCode_GivenValidationExceptionToBadRequestStatusCodeMapping_MapsException()
    {
        // Arrange
        var options = new GlobalExceptionFilterOptions();

        // Act
        options.MapExceptionToStatusCode<ValidationException>(HttpStatusCode.BadRequest);
        var statusCode = options.GetMappedStatusCodeOrDefault(new ValidationException("Bad Request"));

        // Assert
        Assert.AreEqual(HttpStatusCode.BadRequest, statusCode);
    }

    [TestMethod]
    public void MapExceptionToStatusCode_GivenArgumentExceptionToBadRequestStatusMapping_MapsException()
    {
        // Arrange
        var options = new GlobalExceptionFilterOptions();

        // Act
        options.MapExceptionToStatusCode<ArgumentException>(HttpStatusCode.BadRequest);
        var statusCode = options.GetMappedStatusCodeOrDefault(new ArgumentException());

        // Assert
        Assert.AreEqual(HttpStatusCode.BadRequest, statusCode);
    }

    [TestMethod]
    public void MapExceptionToStatusCode_GivenNonErrorStatusCode_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var options = new GlobalExceptionFilterOptions();

        // Act
        void action() => options.MapExceptionToStatusCode<Exception>(HttpStatusCode.OK);

        // Assert
        var exception = Assert.ThrowsException<ArgumentOutOfRangeException>(action);
        StringAssert.Contains(exception.Message, "Value does not indicate an HTTP error status.");
    }

    [TestMethod]
    public void GetMappedStatusCodeOrDefault_GivenArgumentNullExceptionWithNoMappings_ReturnsDefaultStatusCode()
    {
        // Arrange
        var options = new GlobalExceptionFilterOptions();

        // Act
        var statusCode = options.GetMappedStatusCodeOrDefault(new ArgumentNullException());

        // Assert
        Assert.AreEqual(HttpStatusCode.InternalServerError, statusCode);
    }

    [TestMethod]
    public void GetMappedStatusCodeOrDefault_GivenArgumentNullExceptionWithOnlyArgumentExceptionsMapped_ReturnsMappedStatusCode()
    {
        // Arrange
        var options = new GlobalExceptionFilterOptions();
        options.MapExceptionToStatusCode<ArgumentException>(HttpStatusCode.BadRequest);

        // Act
        var statusCode = options.GetMappedStatusCodeOrDefault(new ArgumentNullException());

        // Assert
        Assert.AreEqual(HttpStatusCode.BadRequest, statusCode);
    }

    [TestMethod]
    public void GetMappedStatusCodeOrDefault_GivenArgumentNullExceptionWithBothArgumentExceptionAndArgumentNullExceptionMapped_ReturnsBestMatch()
    {
        // Arrange
        var options = new GlobalExceptionFilterOptions();
        options.MapExceptionToStatusCode<ArgumentException>(HttpStatusCode.BadRequest);
        options.MapExceptionToStatusCode<ArgumentNullException>(HttpStatusCode.UnprocessableEntity);

        // Act
        var statusCode = options.GetMappedStatusCodeOrDefault(new ArgumentNullException());

        // Assert
        Assert.AreEqual(HttpStatusCode.UnprocessableEntity, statusCode);
    }

    [TestMethod]
    public void GetMappedStatusCodeOrDefault_GivenArgumentExceptionWithBothArgumentExceptionAndArgumentNullExceptionMapped_ReturnsBestMatch()
    {
        // Arrange
        var options = new GlobalExceptionFilterOptions();
        options.MapExceptionToStatusCode<ArgumentException>(HttpStatusCode.BadRequest);
        options.MapExceptionToStatusCode<ArgumentNullException>(HttpStatusCode.UnprocessableEntity);

        // Act
        var statusCode = options.GetMappedStatusCodeOrDefault(new ArgumentException());

        // Assert
        Assert.AreEqual(HttpStatusCode.BadRequest, statusCode);
    }
}
