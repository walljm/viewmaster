using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ViewMaster.DesktopController
{
    public static class JsonSerializerSettingsProvider
    {
        public class IPAddressConverter : JsonConverter<IPAddress>
        {
            public override IPAddress Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
                IPAddress.Parse(reader.GetString() ?? throw new InvalidOperationException("Value cannot be null"));

            public override void Write(Utf8JsonWriter writer, IPAddress value, JsonSerializerOptions options) =>
                writer.WriteStringValue(value.ToString());
        }

        private static JsonSerializerOptions? defaultOptions;
        public static JsonSerializerOptions Default => defaultOptions ??= Create();

        private const int DefaultMaxDepth = 32;

        public static void Apply(JsonSerializerOptions options)
        {
            options.MaxDepth = DefaultMaxDepth;
            options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.AllowTrailingCommas = true;
            options.WriteIndented = true;
            options.ReadCommentHandling = JsonCommentHandling.Skip;

            options.Converters.Add(new IPAddressConverter());
            options.Converters.Add(new JsonStringEnumConverter());
            options.Converters.Add(new JsonStringEnumConverter());
        }

        public static JsonSerializerOptions Create()
        {
            var options = new JsonSerializerOptions();
            Apply(options);
            return options;
        }
    }
}
