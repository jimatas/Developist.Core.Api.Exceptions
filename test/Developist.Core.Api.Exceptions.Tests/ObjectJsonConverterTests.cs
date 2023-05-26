using Developist.Core.Api.ProblemDetails.Serialization;
using System.Dynamic;
using System.Text.Json;

namespace Developist.Core.Api.Tests;

[TestClass]
public class ObjectJsonConverterTests
{
    [TestMethod]
    public void WithObjectConverter_ByDefault_AddsObjectConverter()
    {
        // Arrange
        var options = new JsonSerializerOptions();
        var anyConvertersBeforeAct = options.Converters.Any();

        // Act
        options = options.WithObjectConverter();

        // Assert
        Assert.IsFalse(anyConvertersBeforeAct);
        Assert.IsTrue(options.Converters.Any());
        Assert.IsInstanceOfType(options.Converters.First(), typeof(ObjectJsonConverter));
    }

    [TestMethod]
    public void WithObjectConverter_CalledTwice_AddsOnlyOne()
    {
        // Arrange
        var options = new JsonSerializerOptions();

        // Act
        options = options.WithObjectConverter();
        options = options.WithObjectConverter();

        // Assert
        Assert.AreEqual(1, options.Converters.Count);
    }

    [TestMethod]
    public void WithoutObjectConverter_ByDefault_RemovesObjectConverter()
    {
        // Arrange
        var options = new JsonSerializerOptions().WithObjectConverter();
        var anyConvertersBeforeAct = options.Converters.Any();

        // Act
        options = options.WithoutObjectConverter();

        // Assert
        Assert.IsTrue(anyConvertersBeforeAct);
        Assert.IsFalse(options.Converters.Any());
    }

    [TestMethod]
    public void WithoutObjectConverter_WhenTwoPresent_RemovesBoth()
    {
        // Arrange
        var options = new JsonSerializerOptions().WithObjectConverter();
        options.Converters.Add(new ObjectJsonConverter());

        var converterCountBeforeAct = options.Converters.Count;

        // Act
        options = options.WithoutObjectConverter();

        // Assert
        Assert.AreEqual(2, converterCountBeforeAct);
        Assert.IsFalse(options.Converters.Any());
    }

    [TestMethod]
    public void Serialize_GivenBooleanTrue_WritesTrue()
    {
        // Arrange
        var options = new JsonSerializerOptions().WithObjectConverter();
        var obj = new Dictionary<string, object?>
        {
            { "BooleanProperty", true }
        };

        // Act
        var jsonString = JsonSerializer.Serialize(obj, options);

        // Assert
        Assert.AreEqual("{\"BooleanProperty\":true}", jsonString);
    }

    [TestMethod]
    public void Serialize_GivenBooleanFalse_WritesFalse()
    {
        // Arrange
        var options = new JsonSerializerOptions().WithObjectConverter();
        var obj = new Dictionary<string, object?>
        {
            { "BooleanProperty", false }
        };

        // Act
        var jsonString = JsonSerializer.Serialize(obj, options);

        // Assert
        Assert.AreEqual("{\"BooleanProperty\":false}", jsonString);
    }

    [TestMethod]
    public void Serialize_GivenInt32Value_WritesIt()
    {
        // Arrange
        var options = new JsonSerializerOptions().WithObjectConverter();
        var obj = new Dictionary<string, object?>
        {
            { "Int32Property", 12 }
        };

        // Act
        var jsonString = JsonSerializer.Serialize(obj, options);

        // Assert
        Assert.AreEqual("{\"Int32Property\":12}", jsonString);
    }

    [TestMethod]
    public void Serialize_GivenInt64Value_WritesIt()
    {
        // Arrange
        var options = new JsonSerializerOptions().WithObjectConverter();
        var obj = new Dictionary<string, object?>
        {
            { "Int64Property", (long)int.MaxValue + 1 }
        };

        // Act
        var jsonString = JsonSerializer.Serialize(obj, options);

        // Assert
        Assert.AreEqual("{\"Int64Property\":2147483648}", jsonString);
    }

    [TestMethod]
    public void Serialize_GivenDoubleValue_WritesIt()
    {
        // Arrange
        var options = new JsonSerializerOptions().WithObjectConverter();
        var obj = new Dictionary<string, object?>
        {
            { "DoubleProperty", 123.456 }
        };

        // Act
        var jsonString = JsonSerializer.Serialize(obj, options);

        // Assert
        Assert.AreEqual("{\"DoubleProperty\":123.456}", jsonString);
    }

    [TestMethod]
    public void Serialize_GivenStringValue_WritesIt()
    {
        // Arrange
        var options = new JsonSerializerOptions().WithObjectConverter();
        var obj = new Dictionary<string, object?>
        {
            { "StringProperty", "Hello, world!" }
        };

        // Act
        var jsonString = JsonSerializer.Serialize(obj, options);

        // Assert
        Assert.AreEqual("{\"StringProperty\":\"Hello, world!\"}", jsonString);
    }

    [TestMethod]
    public void Serialize_GivenObjectValue_WritesIt()
    {
        // Arrange
        var options = new JsonSerializerOptions().WithObjectConverter();
        var obj = new Dictionary<string, object?>
        {
            { "ObjectProperty", new object() }
        };

        // Act
        var jsonString = JsonSerializer.Serialize(obj, options);

        // Assert
        Assert.AreEqual("{\"ObjectProperty\":{}}", jsonString);
    }

    [DataTestMethod]
    [DataRow("true", true)]
    [DataRow("false", false)]
    public void Deserialize_GivenJsonBoolean_ReadsIt(string jsonString, bool expectedValue)
    {
        // Arrange
        var options = new JsonSerializerOptions().WithObjectConverter();

        // Act
        var obj = JsonSerializer.Deserialize<object>(jsonString, options);

        // Assert
        Assert.IsInstanceOfType(obj, expectedValue.GetType());
        Assert.AreEqual(expectedValue, obj);
    }

    [DataTestMethod]
    [DataRow("32", 32)]
    [DataRow("2147483647", int.MaxValue)]
    public void Deserialize_GivenJsonInt32_ReadsIt(string jsonString, int expectedValue)
    {
        // Arrange
        var options = new JsonSerializerOptions().WithObjectConverter();

        // Act
        var obj = JsonSerializer.Deserialize<object>(jsonString, options);

        // Assert
        Assert.IsInstanceOfType(obj, expectedValue.GetType());
        Assert.AreEqual(expectedValue, obj);
    }

    [DataTestMethod]
    [DataRow("2147483648", (long)int.MaxValue + 1)]
    [DataRow("-9223372036854775808", long.MinValue)]
    public void Deserialize_GivenJsonInt64_ReadsIt(string jsonString, long expectedValue)
    {
        // Arrange
        var options = new JsonSerializerOptions().WithObjectConverter();

        // Act
        var obj = JsonSerializer.Deserialize<object>(jsonString, options);

        // Assert
        Assert.IsInstanceOfType(obj, expectedValue.GetType());
        Assert.AreEqual(expectedValue, obj);
    }

    [DataTestMethod]
    [DataRow("0.0", 0.0f)]
    [DataRow("-123.456", -123.456)]
    public void Deserialize_GivenJsonDouble_ReadsIt(string jsonString, double expectedValue)
    {
        // Arrange
        var options = new JsonSerializerOptions().WithObjectConverter();

        // Act
        var obj = JsonSerializer.Deserialize<object>(jsonString, options);

        // Assert
        Assert.IsInstanceOfType(obj, expectedValue.GetType());
        Assert.AreEqual(expectedValue, obj);
    }

    [DataTestMethod]
    [DataRow("\"2022-10-02T10:57:06.0937503+02:00\"", "2022-10-02T10:57:06.0937503+02:00")]
    [DataRow("\"2022-10-02T08:57:06Z\"", "2022-10-02T08:57:06Z")]
    public void Deserialize_GivenJsonDateTimeOffset_ReadsIt(string jsonString, string expectedValueBeforeParsing)
    {
        // Arrange
        var expectedValue = DateTimeOffset.Parse(expectedValueBeforeParsing);
        var options = new JsonSerializerOptions().WithObjectConverter();

        // Act
        var obj = JsonSerializer.Deserialize<object>(jsonString, options);

        // Assert
        Assert.IsInstanceOfType(obj, expectedValue.GetType());
        Assert.AreEqual(expectedValue, obj);
    }

    [DataTestMethod]
    [DataRow("\"53fc6500-e338-42a2-8b20-34f885209490\"", "53fc6500-e338-42a2-8b20-34f885209490")]
    [DataRow("\"00000000-0000-0000-0000-000000000000\"", "00000000-0000-0000-0000-000000000000")]
    public void Deserialize_GivenJsonGuid_ReadsIt(string jsonString, string expectedValueBeforeParsing)
    {
        // Arrange
        var expectedValue = Guid.Parse(expectedValueBeforeParsing);
        var options = new JsonSerializerOptions().WithObjectConverter();

        // Act
        var obj = JsonSerializer.Deserialize<object>(jsonString, options);

        // Assert
        Assert.IsInstanceOfType(obj, expectedValue.GetType());
        Assert.AreEqual(expectedValue, obj);
    }

    [DataTestMethod]
    [DataRow("\"Hello, world!\"", "Hello, world!")]
    [DataRow("\"2022-01-1A\"", "2022-01-1A")]
    [DataRow("\"\"", "")]
    public void Deserialize_GivenJsonString_ReadsIt(string jsonString, string expectedValue)
    {
        // Arrange
        var options = new JsonSerializerOptions().WithObjectConverter();

        // Act
        var obj = JsonSerializer.Deserialize<object>(jsonString, options);

        // Assert
        Assert.IsInstanceOfType(obj, expectedValue.GetType());
        Assert.AreEqual(expectedValue, obj);
    }

    [TestMethod]
    public void Deserialize_GivenJsonArray_ReadsIt()
    {
        // Arrange
        var jsonString = "[\"Hello, world!\", 32, -123.456, \"2022-01-01T23:59:59Z\"]";
        var expectedValue = new object[] { "Hello, world!", 32, -123.456, new DateTimeOffset(2022, 1, 1, 23, 59, 59, TimeSpan.Zero) };
        var options = new JsonSerializerOptions().WithObjectConverter();

        // Act
        var obj = JsonSerializer.Deserialize<object>(jsonString, options)!;

        // Assert
        Assert.IsInstanceOfType(obj, expectedValue.GetType());
        Assert.IsTrue(expectedValue.SequenceEqual((object[])obj));
    }

    [TestMethod]
    public void Deserialize_GivenJsonObject_ReadsIt()
    {
        // Arrange
        var jsonString = "{\"StringProperty\": \"Hello, world!\", \"Int32Property\": 32, \"DoubleProperty\": -123.456, \"DateTimeOffsetProperty\": \"2022-01-01T23:59:59Z\"}";

        dynamic expectedValue = new ExpandoObject();
        expectedValue.StringProperty = "Hello, world!";
        expectedValue.Int32Property = 32;
        expectedValue.DoubleProperty = -123.456;
        expectedValue.DateTimeOffsetProperty = new DateTimeOffset(2022, 1, 1, 23, 59, 59, TimeSpan.Zero);

        var options = new JsonSerializerOptions().WithObjectConverter();

        // Act
        var obj = JsonSerializer.Deserialize<object>(jsonString, options)!;

        // Assert
        Assert.IsInstanceOfType(obj, expectedValue.GetType());
        Assert.AreEqual(expectedValue.StringProperty, ((dynamic)obj).StringProperty);
        Assert.AreEqual(expectedValue.Int32Property, ((dynamic)obj).Int32Property);
        Assert.AreEqual(expectedValue.DoubleProperty, ((dynamic)obj).DoubleProperty);
        Assert.AreEqual(expectedValue.DateTimeOffsetProperty, ((dynamic)obj).DateTimeOffsetProperty);
    }
}
