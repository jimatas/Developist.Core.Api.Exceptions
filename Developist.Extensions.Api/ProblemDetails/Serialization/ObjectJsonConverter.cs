using System.Collections;
using System.Dynamic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Developist.Extensions.Api.ProblemDetails.Serialization
{
    internal class ObjectJsonConverter : JsonConverter<object>
    {
        public override object? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.TokenType switch
            {
                JsonTokenType.Null => null,
                JsonTokenType.True => true,
                JsonTokenType.False => false,
                JsonTokenType.Number when reader.TryGetInt32(out int value) => value,
                JsonTokenType.Number when reader.TryGetInt64(out long value) => value,
                JsonTokenType.Number => reader.GetDouble(),
                JsonTokenType.String when reader.TryGetDateTimeOffset(out DateTimeOffset value) => value,
                JsonTokenType.String when reader.TryGetGuid(out Guid value) => value,
                JsonTokenType.String => reader.GetString(),
                JsonTokenType.StartObject => ReadObject(ref reader, options),
                JsonTokenType.StartArray => ReadArray(ref reader, options),
                _ => throw new JsonException($"Unsupported JSON token '{reader.TokenType}'.")
            };
        }

        private object ReadObject(ref Utf8JsonReader reader, JsonSerializerOptions options)
        {
            var value = new ExpandoObject() as IDictionary<string, object?>;

            while (reader.Read() && reader.TokenType == JsonTokenType.PropertyName)
            {
                var key = reader.GetString()!;
                reader.Read();
                value.Add(key, Read(ref reader, typeof(object), options));
            }

            return value;
        }

        private object?[] ReadArray(ref Utf8JsonReader reader, JsonSerializerOptions options)
        {
            ArrayList value = new();

            while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
            {
                value.Add(Read(ref reader, typeof(object), options));
            }

            return value.ToArray();
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
}
