using System.Net;
using System.Net.NetworkInformation;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ViewMaster.DesktopController
{
    public static class JsonSerializerSettingsProvider
    {
        public class IPAddressConverter : JsonConverter<IPAddress>
        {
            public override IPAddress Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
                IPAddress.Parse(reader.GetString()?? throw new InvalidOperationException("Value cannot be null"));

            public override void Write(Utf8JsonWriter writer, IPAddress value, JsonSerializerOptions options) =>
                writer.WriteStringValue(value.ToString());
        }

        public class IPAddressWithSubnetConverter : JsonConverter<(IPAddress, int)>
        {
            public override (IPAddress, int) Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                var originalValue = reader.GetString();

                if (originalValue is null)
                {
                    throw new InvalidOperationException($"Subnet ({originalValue}) not in a valid format.");
                }

                var prefixLengthDividerIndex = originalValue.IndexOf('/');
                if (prefixLengthDividerIndex == -1)
                {
                    throw new InvalidOperationException($"Subnet ({originalValue}) not in a valid format.");
                }
                else
                {
                    var address = originalValue[..prefixLengthDividerIndex];
                    var prefix = originalValue[(prefixLengthDividerIndex + 1)..];

                    if (!IPAddress.TryParse(address, out var addressValue))
                    {
                        throw new InvalidOperationException($"Subnet ({originalValue}) not in a valid format.");
                    }

                    if (!int.TryParse(prefix, out var prefixValue))
                    {
                        throw new InvalidOperationException($"Subnet ({originalValue}) not in a valid format.");
                    }

                    return (addressValue, prefixValue);
                }
            }

            public override void Write(Utf8JsonWriter writer, (IPAddress, int) value, JsonSerializerOptions options) =>
                writer.WriteStringValue($"{value.Item1}/{value.Item2}");
        }

        public class PhysicalAddressConverter : JsonConverter<PhysicalAddress>
        {
            public override PhysicalAddress Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
                PhysicalAddress.Parse(reader.GetString());

            public override void Write(Utf8JsonWriter writer, PhysicalAddress value, JsonSerializerOptions options) =>
                writer.WriteStringValue(value.ToString());
        }

        private const int DefaultMaxDepth = 32;

        public static readonly IPAddressConverter IPAddressConverterInstance = new();
        public static readonly IPAddressWithSubnetConverter IPAddressWithSubnetConverterInstance = new();
        public static readonly PhysicalAddressConverter PhysicalAddressConverterInstance = new();

        private static JsonSerializerOptions? defaultOptions;
        public static JsonSerializerOptions Default => defaultOptions ??= Create();

        public static void Apply(JsonSerializerOptions options)
        {
            options.MaxDepth = DefaultMaxDepth;
            options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.AllowTrailingCommas = true;
            options.PropertyNameCaseInsensitive = true;
            options.WriteIndented = true;
            options.ReadCommentHandling = JsonCommentHandling.Skip;

            options.Converters.Add(IPAddressConverterInstance);
            options.Converters.Add(IPAddressWithSubnetConverterInstance);
            options.Converters.Add(PhysicalAddressConverterInstance);
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
