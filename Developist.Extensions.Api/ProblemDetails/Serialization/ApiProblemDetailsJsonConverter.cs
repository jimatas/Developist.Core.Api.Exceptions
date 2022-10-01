﻿using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Developist.Extensions.Api.ProblemDetails.Serialization
{
    internal class ApiProblemDetailsJsonConverter : JsonConverter<ApiProblemDetails>
    {
        public override ApiProblemDetails? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            ApiProblemDetails value = new();

            while (reader.Read() && reader.TokenType == JsonTokenType.PropertyName)
            {
                if (reader.ValueTextEquals("type"))
                {
                    reader.Read();
                    value.Type = new Uri(reader.GetString()!);
                }
                else if (reader.ValueTextEquals("title"))
                {
                    reader.Read();
                    value.Title = reader.GetString();
                }
                else if (reader.ValueTextEquals("status"))
                {
                    reader.Read();
                    value.Status = (HttpStatusCode)reader.GetInt32();
                }
                else if (reader.ValueTextEquals("detail"))
                {
                    reader.Read();
                    value.Detail = reader.GetString();
                }
                else if (reader.ValueTextEquals("instance"))
                {
                    reader.Read();
                    value.Instance = new Uri(reader.GetString()!);
                }
                else
                {
                    var key = reader.GetString()!;
                    reader.Read();
                    value.Extensions[key] = JsonSerializer.Deserialize(ref reader, typeof(object), options.WithObjectConverter());
                }
            }

            return value;
        }

        public override void Write(Utf8JsonWriter writer, ApiProblemDetails value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            if (value.Type is not null)
            {
                writer.WriteString("type", value.Type.ToString());
            }

            if (value.Title is not null)
            {
                writer.WriteString("title", value.Title);
            }

            if (value.Status is not null)
            {
                writer.WriteNumber("status", (int)value.Status.Value);
            }

            if (value.Detail is not null)
            {
                writer.WriteString("detail", value.Detail);
            }

            if (value.Instance is not null)
            {
                writer.WriteString("instance", value.Instance.ToString());
            }

            foreach (var extension in value.Extensions)
            {
                writer.WritePropertyName(extension.Key);
                JsonSerializer.Serialize(writer, extension.Value, extension.Value?.GetType() ?? typeof(object), options.WithObjectConverter());
            }

            writer.WriteEndObject();
        }
    }
}
