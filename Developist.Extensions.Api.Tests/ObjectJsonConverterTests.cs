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

        [TestMethod]
        public void Serialize_GivenBooleanTrue_WritesTrue()
        {
            var options = new JsonSerializerOptions().WithObjectConverter();

            IDictionary<string, object?> obj = new Dictionary<string, object?>
            {
                { "BooleanProperty", true }
            };

            var jsonString = JsonSerializer.Serialize(obj, options);

            Assert.AreEqual("{\"BooleanProperty\":true}", jsonString);
        }

        [TestMethod]
        public void Serialize_GivenBooleanFalse_WritesFalse()
        {
            var options = new JsonSerializerOptions().WithObjectConverter();

            IDictionary<string, object?> obj = new Dictionary<string, object?>
            {
                { "BooleanProperty", false }
            };

            var jsonString = JsonSerializer.Serialize(obj, options);

            Assert.AreEqual("{\"BooleanProperty\":false}", jsonString);
        }

        [TestMethod]
        public void Serialize_GivenInt32Value_WritesIt()
        {
            var options = new JsonSerializerOptions().WithObjectConverter();

            IDictionary<string, object?> obj = new Dictionary<string, object?>
            {
                { "Int32Property", 12 }
            };

            var jsonString = JsonSerializer.Serialize(obj, options);

            Assert.AreEqual("{\"Int32Property\":12}", jsonString);
        }

        [TestMethod]
        public void Serialize_GivenInt64Value_WritesIt()
        {
            var options = new JsonSerializerOptions().WithObjectConverter();

            IDictionary<string, object?> obj = new Dictionary<string, object?>
            {
                { "Int64Property", (long)int.MaxValue + 1 }
            };

            var jsonString = JsonSerializer.Serialize(obj, options);

            Assert.AreEqual("{\"Int64Property\":2147483648}", jsonString);
        }

        [TestMethod]
        public void Serialize_GivenDoubleValue_WritesIt()
        {
            var options = new JsonSerializerOptions().WithObjectConverter();

            IDictionary<string, object?> obj = new Dictionary<string, object?>
            {
                { "DoubleProperty", 123.456 }
            };

            var jsonString = JsonSerializer.Serialize(obj, options);

            Assert.AreEqual("{\"DoubleProperty\":123.456}", jsonString);
        }

        [TestMethod]
        public void Serialize_GivenStringValue_WritesIt()
        {
            var options = new JsonSerializerOptions().WithObjectConverter();

            IDictionary<string, object?> obj = new Dictionary<string, object?>
            {
                { "StringProperty", "Hello, World!" }
            };

            var jsonString = JsonSerializer.Serialize(obj, options);

            Assert.AreEqual("{\"StringProperty\":\"Hello, World!\"}", jsonString);
        }

        [TestMethod]
        public void Serialize_GivenObjectValue_WritesIt()
        {
            var options = new JsonSerializerOptions().WithObjectConverter();

            IDictionary<string, object?> obj = new Dictionary<string, object?>
            {
                { "ObjectProperty", new object() }
            };

            var jsonString = JsonSerializer.Serialize(obj, options);

            Assert.AreEqual("{\"ObjectProperty\":{}}", jsonString);
        }

        [DataTestMethod]
        [DataRow("true", true)]
        [DataRow("false", false)]
        public void Deserialize_GivenJsonBoolean_ReadsIt(string jsonString, bool expectedValue)
        {
            var options = new JsonSerializerOptions().WithObjectConverter();

            var obj = JsonSerializer.Deserialize<object>(jsonString, options);

            Assert.IsInstanceOfType(obj, typeof(bool));
            Assert.AreEqual(expectedValue, obj);
        }

        [DataTestMethod]
        [DataRow("32", 32)]
        [DataRow("2147483647", int.MaxValue)]
        public void Deserialize_GivenJsonInt32_ReadsIt(string jsonString, int expectedValue)
        {
            var options = new JsonSerializerOptions().WithObjectConverter();

            var obj = JsonSerializer.Deserialize<object>(jsonString, options);

            Assert.IsInstanceOfType(obj, expectedValue.GetType());
            Assert.AreEqual(expectedValue, obj);
        }

        [DataTestMethod]
        [DataRow("2147483648", (long)int.MaxValue + 1)]
        [DataRow("-9223372036854775808", long.MinValue)]
        public void Deserialize_GivenJsonInt64_ReadsIt(string jsonString, long expectedValue)
        {
            var options = new JsonSerializerOptions().WithObjectConverter();

            var obj = JsonSerializer.Deserialize<object>(jsonString, options);

            Assert.IsInstanceOfType(obj, expectedValue.GetType());
            Assert.AreEqual(expectedValue, obj);
        }

        [DataTestMethod]
        [DataRow("0.0", 0.0f)]
        [DataRow("-123.456", -123.456)]
        public void Deserialize_GivenJsonDouble_ReadsIt(string jsonString, double expectedValue)
        {
            var options = new JsonSerializerOptions().WithObjectConverter();

            var obj = JsonSerializer.Deserialize<object>(jsonString, options);

            Assert.IsInstanceOfType(obj, expectedValue.GetType());
            Assert.AreEqual(expectedValue, obj);
        }

        [DataTestMethod]
        [DataRow("\"2022-10-02T10:57:06.0937503+02:00\"", "2022-10-02T10:57:06.0937503+02:00")]
        [DataRow("\"2022-10-02T08:57:06Z\"", "2022-10-02T08:57:06Z")]
        public void Deserialize_GivenJsonDateTimeOffset_ReadsIt(string jsonString, string expectedValueBeforeParsing)
        {
            DateTimeOffset expectedValue = DateTimeOffset.Parse(expectedValueBeforeParsing);

            var options = new JsonSerializerOptions().WithObjectConverter();

            var obj = JsonSerializer.Deserialize<object>(jsonString, options);

            Assert.IsInstanceOfType(obj, expectedValue.GetType());
            Assert.AreEqual(expectedValue, obj);
        }

        [DataTestMethod]
        [DataRow("\"53fc6500-e338-42a2-8b20-34f885209490\"", "53fc6500-e338-42a2-8b20-34f885209490")]
        [DataRow("\"00000000-0000-0000-0000-000000000000\"", "00000000-0000-0000-0000-000000000000")]
        public void Deserialize_GivenJsonGuid_ReadsIt(string jsonString, string expectedValueBeforeParsing)
        {
            Guid expectedValue = Guid.Parse(expectedValueBeforeParsing);

            var options = new JsonSerializerOptions().WithObjectConverter();

            var obj = JsonSerializer.Deserialize<object>(jsonString, options);

            Assert.IsInstanceOfType(obj, expectedValue.GetType());
            Assert.AreEqual(expectedValue, obj);
        }

        [DataTestMethod]
        [DataRow("\"Hello, world!\"", "Hello, world!")]
        [DataRow("\"2022-01-1A\"", "2022-01-1A")]
        [DataRow("\"\"", "")]
        public void Deserialize_GivenJsonString_ReadsIt(string jsonString, string expectedValue)
        {
            var options = new JsonSerializerOptions().WithObjectConverter();

            var obj = JsonSerializer.Deserialize<object>(jsonString, options);

            Assert.IsInstanceOfType(obj, expectedValue.GetType());
            Assert.AreEqual(expectedValue, obj);
        }
    }
}
