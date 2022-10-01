using Developist.Extensions.Api.ProblemDetails.Serialization;

using System.Text.Json;

namespace Developist.Extensions.Api.Tests
{
    [TestClass]
    public class ObjectJsonConverterTests
    {
        [TestMethod]
        public void WithObjectConverter_ByDefault_AddsObjectConverter()
        {
            JsonSerializerOptions options = new();

            Assert.IsFalse(options.Converters.Any());

            options = options.WithObjectConverter();

            Assert.IsTrue(options.Converters.Any());
            Assert.IsInstanceOfType(options.Converters.First(), typeof(ObjectJsonConverter));
        }

        [TestMethod]
        public void WithObjectConverter_CalledTwice_AddsOnlyOne()
        {
            JsonSerializerOptions options = new();

            options = options.WithObjectConverter();
            options = options.WithObjectConverter();

            Assert.AreEqual(1, options.Converters.Count);
        }

        [TestMethod]
        public void WithoutObjectConverter_ByDefault_RemovesObjectConverter()
        {
            var options = new JsonSerializerOptions().WithObjectConverter();

            Assert.IsTrue(options.Converters.Any());

            options = options.WithoutObjectConverter();

            Assert.IsFalse(options.Converters.Any());
        }

        [TestMethod]
        public void WithoutObjectConverter_WhenTwoPresent_RemovesBoth()
        {
            var options = new JsonSerializerOptions().WithObjectConverter();
            options.Converters.Add(new ObjectJsonConverter());

            Assert.AreEqual(2, options.Converters.Count);

            options = options.WithoutObjectConverter();

            Assert.IsFalse(options.Converters.Any());
        }
    }
}
