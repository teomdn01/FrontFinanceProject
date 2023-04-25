using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Org.Front.Core.Contracts.Models.Deserialization
{
    public class DecimalToStringConverter : JsonConverter<decimal>
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeof(decimal) == typeToConvert;
        }

        public override decimal Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Number)
            {
                return reader.GetDecimal();
            }

            if (reader.TokenType == JsonTokenType.String)
            {
                var str = reader.GetString();
                return decimal.Parse(str, System.Globalization.CultureInfo.InvariantCulture);
            }

            throw new InvalidOperationException($"Invalid json token: {reader.TokenType}");
        }

        public override void Write(Utf8JsonWriter writer, decimal value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
