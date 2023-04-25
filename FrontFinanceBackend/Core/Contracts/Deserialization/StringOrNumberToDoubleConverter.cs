using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Org.Front.Core.Contracts.Models.Deserialization
{
    public class StringOrNumberToDoubleConverter : JsonConverter<double?>
    {
        public override bool CanConvert(Type objectType) => true;


        public override double? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return null;
            }

            using var jsonDocument = JsonDocument.ParseValue(ref reader);
            if (jsonDocument.RootElement.ValueKind == JsonValueKind.String)
            {
                var stringValue = JsonSerializer.Deserialize(ref reader, typeof(string), options);
                return Convert.ToDouble(stringValue, CultureInfo.InvariantCulture);
            }
            else
            {
                return JsonSerializer.Deserialize(ref reader, typeof(decimal), options) as double?;
            }
        }

        public override void Write(Utf8JsonWriter writer, double? value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, value.GetType(), options);
        }
    }
}
