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
            var tokenType = reader.TokenType;
            if (tokenType == JsonTokenType.Null)
            {
                return null;
            }

            if (tokenType == JsonTokenType.Number)
            {
                if (reader.TryGetInt32(out var int32Value))
                {
                    return int32Value;
                }

                if (reader.TryGetInt64(out var int64Value))
                {
                    return int64Value;
                }

                return reader.GetDouble();
            }

            if (tokenType is JsonTokenType.True or JsonTokenType.False)
            {
                return reader.GetBoolean();
            }

            if (tokenType == JsonTokenType.String)
            {
                return reader.GetString();
            }

            if (tokenType == JsonTokenType.StartObject)
            {
                return ReadObject(ref reader, options);
            }

            if (tokenType == JsonTokenType.StartArray)
            {
                return ReadArray(ref reader, options);
            }

            throw new JsonException($"Unsupported JSON token '{tokenType}'.");
        }

        private object ReadObject(ref Utf8JsonReader reader, JsonSerializerOptions options)
        {
            var obj = new ExpandoObject() as IDictionary<string, object?>;

            while (reader.Read() && reader.TokenType == JsonTokenType.PropertyName)
            {
                var key = reader.GetString()!;
                reader.Read();
                obj.Add(key, Read(ref reader, typeof(object), options));
            }

            return obj;
        }

        private object?[] ReadArray(ref Utf8JsonReader reader, JsonSerializerOptions options)
        {
            ArrayList array = new();

            while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
            {
                array.Add(Read(ref reader, typeof(object), options));
            }

            return array.ToArray();
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
