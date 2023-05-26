using System.Text.Json;

namespace Developist.Core.Api.ProblemDetails.Serialization;

internal static class JsonSerializerOptionsExtensions
{
    public static JsonSerializerOptions WithObjectConverter(this JsonSerializerOptions options)
    {
        if (options.Converters.All(converter => converter.GetType() != typeof(ObjectJsonConverter)))
        {
            options = new JsonSerializerOptions(options);
            options.Converters.Add(new ObjectJsonConverter());
        }

        return options;
    }

    public static JsonSerializerOptions WithoutObjectConverter(this JsonSerializerOptions options)
    {
        var objectConverter = options.Converters.FirstOrDefault(converter => converter.GetType() == typeof(ObjectJsonConverter));
        if (objectConverter is not null)
        {
            options = new JsonSerializerOptions(options);
            if (options.Converters.Remove(objectConverter))
            {
                options = options.WithoutObjectConverter();
            }
        }

        return options;
    }
}
