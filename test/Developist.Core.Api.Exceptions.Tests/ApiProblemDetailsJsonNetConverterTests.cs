using Developist.Core.Api.ProblemDetails;
using System.Dynamic;
using System.Net;

namespace Developist.Core.Api.Tests;

[TestClass]
public class ApiProblemDetailsJsonNetConverterTests
{
    [TestMethod]
    public void Serialize_GivenDefaultProblemDetailsInstance_WritesEmptyObject()
    {
        // Arrange
        var problemDetails = new ApiProblemDetails();

        // Act
        var jsonString = JsonNet.JsonConvert.SerializeObject(problemDetails);

        // Assert
        Assert.IsNotNull(jsonString);
        Assert.AreEqual("{}", jsonString);
    }

    [TestMethod]
    public void Deserialize_GivenEmptyObject_ReadsDefaultProblemDetailsInstance()
    {
        // Arrange
        var jsonString = "{}";

        // Act
        var problemDetails = JsonNet.JsonConvert.DeserializeObject<ApiProblemDetails>(jsonString);

        // Assert
        Assert.IsNotNull(problemDetails);
        Assert.IsNull(problemDetails.Type);
        Assert.IsNull(problemDetails.Title);
        Assert.IsNull(problemDetails.Status);
        Assert.IsNull(problemDetails.Detail);
        Assert.IsNull(problemDetails.Instance);
        Assert.IsFalse(problemDetails.Extensions.Any());
    }

    [TestMethod]
    public void Serialize_GivenProblemDetailsWithAllPropertiesSet_WritesAllProperties()
    {
        // Arrange
        var problemDetails = new ApiProblemDetails
        {
            Type = new Uri("https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/500"),
            Title = "Internal Server Error",
            Status = HttpStatusCode.InternalServerError,
            Detail = "There was a problem servicing your request.",
            Instance = new Uri("https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/500")
        };

        // Act
        var jsonString = JsonNet.JsonConvert.SerializeObject(problemDetails);

        // Assert
        Assert.AreEqual("{\"type\":\"https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/500\",\"title\":\"Internal Server Error\",\"status\":500,\"detail\":\"There was a problem servicing your request.\",\"instance\":\"https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/500\"}", jsonString);
    }

    [TestMethod]
    public void Deserialize_GivenJsonObjectWithMultipleProperties_ReadsThemAllIntoProblemDetails()
    {
        // Arrange
        var jsonString = "{\"type\":\"https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/500\",\"title\":\"Internal Server Error\",\"status\":500,\"detail\":\"There was a problem servicing your request.\",\"instance\":\"https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/500\"}";

        // Act
        var problemDetails = JsonNet.JsonConvert.DeserializeObject<ApiProblemDetails>(jsonString)!;

        // Assert
        Assert.AreEqual(new Uri("https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/500"), problemDetails.Type);
        Assert.AreEqual("Internal Server Error", problemDetails.Title);
        Assert.AreEqual(HttpStatusCode.InternalServerError, problemDetails.Status);
        Assert.AreEqual("There was a problem servicing your request.", problemDetails.Detail);
        Assert.AreEqual(new Uri("https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/500"), problemDetails.Instance);
    }

    [TestMethod]
    public void Serialize_GivenProblemDetailsWithSimpleExtensionProperty_WritesThatProperty()
    {
        // Arrange
        var problemDetails = new ApiProblemDetails();
        problemDetails.Extensions["customErrorData"] = "Additional information about the error.";

        // Act
        var jsonString = JsonNet.JsonConvert.SerializeObject(problemDetails);

        // Assert
        Assert.AreEqual("{\"customErrorData\":\"Additional information about the error.\"}", jsonString);
    }

    [TestMethod]
    public void Deserialize_GivenJsonObjectWithSimpleExtensionProperty_ReadsThatProperty()
    {
        // Arrange
        var jsonString = "{\"customErrorData\":\"Additional information about the error.\"}";

        // Act
        var problemDetails = JsonNet.JsonConvert.DeserializeObject<ApiProblemDetails>(jsonString)!;

        // Assert
        Assert.IsTrue(problemDetails.Extensions.Any());
        Assert.IsTrue(problemDetails.Extensions.ContainsKey("customErrorData"));
        Assert.AreEqual("Additional information about the error.", problemDetails.Extensions["customErrorData"]);
    }

    [TestMethod]
    public void Serialize_GivenProblemDetailsWithComplexExtensionProperty_WritesThatProperty()
    {
        // Arrange
        var problemDetails = new ApiProblemDetails();
        problemDetails.Extensions["customErrorData"] = new CustomErrorData1
        {
            ErrorNumber = 100,
            ErrorInformation = "Additional information about the error."
        };

        // Act
        var jsonString = JsonNet.JsonConvert.SerializeObject(problemDetails);

        // Assert
        Assert.AreEqual("{\"customErrorData\":{\"ErrorNumber\":100,\"ErrorInformation\":\"Additional information about the error.\"}}", jsonString);
    }

    [TestMethod]
    public void Deserialize_GivenJsonObjectWithComplexExtensionProperty_ReadsThatProperty()
    {
        // Arrange
        var jsonString = "{\"customErrorData\":{\"ErrorNumber\":100,\"ErrorInformation\":\"Additional information about the error.\"}}";

        // Act
        var problemDetails = JsonNet.JsonConvert.DeserializeObject<ApiProblemDetails>(jsonString)!;

        // Assert
        Assert.IsTrue(problemDetails.Extensions.Any());
        Assert.IsTrue(problemDetails.Extensions.ContainsKey("customErrorData"));
        Assert.IsInstanceOfType(problemDetails.Extensions["customErrorData"], typeof(ExpandoObject));

        dynamic customErrorData = problemDetails.Extensions["customErrorData"]!;
        Assert.AreEqual(100, customErrorData.ErrorNumber);
        Assert.AreEqual("Additional information about the error.", customErrorData.ErrorInformation);
    }

    [TestMethod]
    public void Serialize_GivenProblemDetailsWithComplexExtensionPropertyUsingCustomNaming_WritesThatProperty()
    {
        // Arrange
        var problemDetails = new ApiProblemDetails();
        problemDetails.Extensions["customErrorData"] = new CustomErrorData2
        {
            ErrorNumber = 100,
            ErrorInformation = "Additional information about the error."
        };

        // Act
        var jsonString = JsonNet.JsonConvert.SerializeObject(problemDetails);

        // Assert
        Assert.AreEqual("{\"customErrorData\":{\"error_no\":100,\"error_info\":\"Additional information about the error.\"}}", jsonString);
    }

    #region Fixture
    class CustomErrorData1
    {
        public int ErrorNumber { get; set; }
        public string? ErrorInformation { get; set; }
    }

    class CustomErrorData2
    {
        [JsonNet.JsonProperty("error_no")]
        public int ErrorNumber { get; set; }

        [JsonNet.JsonProperty("error_info")]
        public string? ErrorInformation { get; set; }
    }
    #endregion
}
