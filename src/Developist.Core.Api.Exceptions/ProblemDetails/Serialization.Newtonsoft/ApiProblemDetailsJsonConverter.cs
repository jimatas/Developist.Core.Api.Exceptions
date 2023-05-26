using System.Dynamic;
using System.Net;

namespace Developist.Core.Api.ProblemDetails.Serialization.Newtonsoft;

internal class ApiProblemDetailsJsonConverter : JsonNet.JsonConverter<ApiProblemDetails>
{
    public override ApiProblemDetails? ReadJson(JsonNet.JsonReader reader, Type objectType, ApiProblemDetails? existingValue, bool hasExistingValue, JsonNet.JsonSerializer serializer)
    {
        var result = new ApiProblemDetails();

        while (reader.Read() && reader.TokenType == JsonNet.JsonToken.PropertyName)
        {
            if (reader.Value!.Equals("type"))
            {
                result.Type = new Uri(reader.ReadAsString()!);
            }
            else if (reader.Value.Equals("title"))
            {
                result.Title = reader.ReadAsString();
            }
            else if (reader.Value.Equals("status"))
            {
                result.Status = (HttpStatusCode)reader.ReadAsInt32()!;
            }
            else if (reader.Value.Equals("detail"))
            {
                result.Detail = reader.ReadAsString();
            }
            else if (reader.Value.Equals("instance"))
            {
                result.Instance = new Uri(reader.ReadAsString()!);
            }
            else
            {
                var key = reader.Value.ToString()!;
                reader.Read();
                result.Extensions[key] = serializer.Deserialize(reader, typeof(ExpandoObject));
            }
        }

        return result;
    }

    public override void WriteJson(JsonNet.JsonWriter writer, ApiProblemDetails? value, JsonNet.JsonSerializer serializer)
    {
        writer.WriteStartObject();

        if (value!.Type is not null)
        {
            writer.WritePropertyName("type");
            writer.WriteValue(value.Type.ToString());
        }

        if (value.Title is not null)
        {
            writer.WritePropertyName("title");
            writer.WriteValue(value.Title);
        }

        if (value.Status is not null)
        {
            writer.WritePropertyName("status");
            writer.WriteValue((int)value.Status.Value);
        }

        if (value.Detail is not null)
        {
            writer.WritePropertyName("detail");
            writer.WriteValue(value.Detail);
        }

        if (value.Instance is not null)
        {
            writer.WritePropertyName("instance");
            writer.WriteValue(value.Instance.ToString());
        }

        foreach (var extension in value.Extensions)
        {
            writer.WritePropertyName(extension.Key);
            serializer.Serialize(writer, extension.Value, extension.Value?.GetType() ?? typeof(object));
        }

        writer.WriteEndObject();
    }
}
