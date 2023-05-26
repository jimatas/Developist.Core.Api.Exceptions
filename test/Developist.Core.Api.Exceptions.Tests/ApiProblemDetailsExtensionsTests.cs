using Developist.Core.Api.Exceptions;
using Developist.Core.Api.ProblemDetails;
using System.ComponentModel.DataAnnotations;

namespace Developist.Core.Api.Tests;

[TestClass]
public class ApiProblemDetailsExtensionsTests
{
    [TestMethod]
    public void ToProblemDetails_ByDefault_SetsAllPropertiesToExpectedValues()
    {
        // Arrange
        var exception = new ForbiddenException("You do not have permission to view this page.");

        // Act
        var problemDetails = exception.ToProblemDetails();

        // Assert
        Assert.AreEqual(exception.StatusCode, problemDetails.Status);
        Assert.AreEqual(exception.ReasonPhrase, problemDetails.Title);
        Assert.AreEqual(exception.HelpLink, problemDetails.Type);
        Assert.AreEqual(exception.Message, problemDetails.Detail);
    }

    [TestMethod]
    public void ToProblemDetails_GivenApiExceptionWithInnerException_DoesNotDiscloseDetailsByDefault()
    {
        // Arrange
        var exception = new BadRequestException("One or more validation errors occurred.", new ValidationException("The 'Id' property cannot be null or empty."));

        // Act
        var problemDetails = exception.ToProblemDetails();

        // Assert
        Assert.AreEqual(exception.Message, problemDetails.Detail);
    }

    [TestMethod]
    public void ToProblemDetails_GivenApiExceptionWithInnerException_DisclosesDetailsAsRequested()
    {
        // Arrange
        var exception = new BadRequestException("One or more validation errors occurred.", new ValidationException("The 'Id' property cannot be null or empty."));

        // Act
        var problemDetails = exception.ToProblemDetails(discloseExceptionDetails: true);

        // Assert
        Assert.AreEqual(exception.GetDetailMessage(), problemDetails.Detail);
    }
}
