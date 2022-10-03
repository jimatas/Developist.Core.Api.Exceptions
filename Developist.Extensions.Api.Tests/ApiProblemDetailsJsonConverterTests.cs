using Developist.Extensions.Api.ProblemDetails;

using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Developist.Extensions.Api.Tests
{
    [TestClass]
    public class ApiProblemDetailsJsonConverterTests
    {
        [TestMethod]
        public void Serialize_GivenDefaultProblemDetailsInstance_WritesEmptyObject()
        {
            ApiProblemDetails problemDetails = new();

            var jsonString = JsonSerializer.Serialize(problemDetails);

            Assert.IsNotNull(jsonString);
            Assert.AreEqual("{}", jsonString);
        }

        [TestMethod]
        public void Deserialize_GivenEmptyObject_ReadsDefaultProblemDetailsInstance()
        {
            string jsonString = "{}";

            var problemDetails = JsonSerializer.Deserialize<ApiProblemDetails>(jsonString);

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
            var problemDetails = new ApiProblemDetails
            {
                Type = new Uri("https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/500"),
                Title = "Internal Server Error",
                Status = HttpStatusCode.InternalServerError,
                Detail = "There was a problem servicing your request.",
                Instance = new Uri("https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/500")
            };

            var jsonString = JsonSerializer.Serialize(problemDetails);

            Assert.IsNotNull(jsonString);
            Assert.AreEqual("{\"type\":\"https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/500\",\"title\":\"Internal Server Error\",\"status\":500,\"detail\":\"There was a problem servicing your request.\",\"instance\":\"https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/500\"}", jsonString);
        }

        [TestMethod]
        public void Deserialize_GivenJsonObjectWithProperties_ReadsThemIntoProblemDetails()
        {
            string jsonString = "{\"type\":\"https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/500\",\"title\":\"Internal Server Error\",\"status\":500,\"detail\":\"There was a problem servicing your request.\",\"instance\":\"https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/500\"}";

            var problemDetails = JsonSerializer.Deserialize<ApiProblemDetails>(jsonString)!;

            Assert.AreEqual(new Uri("https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/500"), problemDetails.Type);
            Assert.AreEqual("Internal Server Error", problemDetails.Title);
            Assert.AreEqual(HttpStatusCode.InternalServerError, problemDetails.Status);
            Assert.AreEqual("There was a problem servicing your request.", problemDetails.Detail);
            Assert.AreEqual(new Uri("https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/500"), problemDetails.Instance);
        }

        [TestMethod]
        public void Serialize_GivenProblemDetailsWithSingleSimpleExtensionProperty_WritesThatProperty()
        {
            ApiProblemDetails problemDetails = new();
            problemDetails.Extensions["customErrorData"] = "Additional information about the error.";

            var jsonString = JsonSerializer.Serialize(problemDetails);

            Assert.IsNotNull(jsonString);
            Assert.AreEqual("{\"customErrorData\":\"Additional information about the error.\"}", jsonString);
        }

        [TestMethod]
        public void Serialize_GivenProblemDetailsWithComplexExtensionProperty_WritesThatProperty()
        {
            ApiProblemDetails problemDetails = new();
            problemDetails.Extensions["customErrorData"] = new CustomErrorData1
            {
                ErrorNumber = 100,
                ErrorInformation = "Additional information about the error."
            };

            var jsonString = JsonSerializer.Serialize(problemDetails);

            Assert.IsNotNull(jsonString);
            Assert.AreEqual("{\"customErrorData\":{\"ErrorNumber\":100,\"ErrorInformation\":\"Additional information about the error.\"}}", jsonString);
        }

        class CustomErrorData1
        {
            public int ErrorNumber { get; set; }
            public string? ErrorInformation { get; set; }
        }

        [TestMethod]
        public void Serialize_GivenProblemDetailsWithComplexExtensionPropertyWithCustomNaming_WritesThatProperty()
        {
            ApiProblemDetails problemDetails = new();
            problemDetails.Extensions["customErrorData"] = new CustomErrorData2
            {
                ErrorNumber = 100,
                ErrorInformation = "Additional information about the error."
            };

            var jsonString = JsonSerializer.Serialize(problemDetails);

            Assert.IsNotNull(jsonString);
            Assert.AreEqual("{\"customErrorData\":{\"error_no\":100,\"error_info\":\"Additional information about the error.\"}}", jsonString);
        }

        class CustomErrorData2
        {
            [JsonPropertyName("error_no")]
            public int ErrorNumber { get; set; }

            [JsonPropertyName("error_info")]
            public string? ErrorInformation { get; set; }
        }
    }
}
