using Developist.Core.Api.Exceptions;
using Developist.Core.Api.MvcFilters;
using System.Net;

namespace Developist.Core.Api.Tests;

[TestClass]
public class GlobalExceptionFilterOptionsTests
{
    [TestMethod]
    public void MapExceptionToStatusCode_GivenBadRequestExceptionToUnprocessableEntityStatusCodeMapping_MapsIt()
    {
        // Arrange
        var options = new GlobalExceptionFilterOptions();

        // Act
        options.MapExceptionToStatusCode<BadRequestException>(HttpStatusCode.UnprocessableEntity);
        var statusCode = options.GetMappedStatusCodeOrDefault(new BadRequestException());

        // Assert
        Assert.AreEqual(HttpStatusCode.UnprocessableEntity, statusCode);
    }

    [TestMethod]
    public void MapExceptionToStatusCode_GivenArgumentExceptionToBadRequestStatusMapping_MapsIt()
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
    public void MapExceptionToStatusCode_GivenNonErrorStatusCode_ThrowsArgumentException()
    {
        // Arrange
        var options = new GlobalExceptionFilterOptions();

        // Act
        var action = () => options.MapExceptionToStatusCode<Exception>(HttpStatusCode.OK);

        // Assert
        var exception = Assert.ThrowsException<ArgumentOutOfRangeException>(action);
        Assert.AreEqual("statusCode", exception.ParamName);
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

        // Act
        options.MapExceptionToStatusCode<ArgumentException>(HttpStatusCode.BadRequest);

        var statusCode = options.GetMappedStatusCodeOrDefault(new ArgumentNullException());

        // Assert
        Assert.AreEqual(HttpStatusCode.BadRequest, statusCode);
    }

    [TestMethod]
    public void GetMappedStatusCodeOrDefault_GivenArgumentNullExceptionWithBothArgumentExceptionAndArgumentNullExceptionMapped_ReturnsBestMatch()
    {
        // Arrange
        var options = new GlobalExceptionFilterOptions();

        // Act
        options.MapExceptionToStatusCode<ArgumentException>(HttpStatusCode.BadRequest);
        options.MapExceptionToStatusCode<ArgumentNullException>(HttpStatusCode.UnprocessableEntity);

        var statusCode = options.GetMappedStatusCodeOrDefault(new ArgumentNullException());

        // Assert
        Assert.AreEqual(HttpStatusCode.UnprocessableEntity, statusCode);
    }

    [TestMethod]
    public void GetMappedStatusCodeOrDefault_GivenArgumentExceptionWithBothArgumentExceptionAndArgumentNullExceptionMapped_ReturnsBestMatch()
    {
        // Arrange
        var options = new GlobalExceptionFilterOptions();

        // Act
        options.MapExceptionToStatusCode<ArgumentException>(HttpStatusCode.BadRequest);
        options.MapExceptionToStatusCode<ArgumentNullException>(HttpStatusCode.UnprocessableEntity);

        var statusCode = options.GetMappedStatusCodeOrDefault(new ArgumentException());

        // Assert
        Assert.AreEqual(HttpStatusCode.BadRequest, statusCode);
    }
}
