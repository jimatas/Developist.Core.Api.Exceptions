using System.Dynamic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Developist.Core.Api.ProblemDetails.Serialization;

internal class ObjectJsonConverter : JsonConverter<object>
{
    public override object? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType switch
        {
            JsonTokenType.Null => null,
            JsonTokenType.True => true,
            JsonTokenType.False => false,
            JsonTokenType.Number when reader.TryGetInt32(out int result) => result,
            JsonTokenType.Number when reader.TryGetInt64(out long result) => result,
            JsonTokenType.Number => reader.GetDouble(),
            JsonTokenType.String when reader.TryGetDateTimeOffset(out DateTimeOffset result) => result,
            JsonTokenType.String when reader.TryGetGuid(out Guid result) => result,
            JsonTokenType.String => reader.GetString(),
            JsonTokenType.StartObject => ReadObject(ref reader, options),
            JsonTokenType.StartArray => ReadArray(ref reader, options),
            _ => throw new JsonException($"Invalid or unsupported JSON token: '{reader.TokenType}'.")
        };
    }

    private object ReadObject(ref Utf8JsonReader reader, JsonSerializerOptions options)
    {
        IDictionary<string, object?> result = new ExpandoObject();
        
        while (reader.Read() && reader.TokenType == JsonTokenType.PropertyName)
        {
            var key = reader.GetString()!;
            reader.Read();
            result.Add(key, Read(ref reader, typeof(object), options));
        }

        return result;
    }

    private object?[] ReadArray(ref Utf8JsonReader reader, JsonSerializerOptions options)
    {
        var result = new List<object?>();
        
        while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
        {
            result.Add(Read(ref reader, typeof(object), options));
        }

        return result.ToArray();
    }

    public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
    {
        if (value.GetType() == typeof(object))
        {
            writer.WriteStartObject();
            writer.WriteEndObject();
        }
        else
        {
            JsonSerializer.Serialize(writer, value, value.GetType(), options);
        }
    }
}
