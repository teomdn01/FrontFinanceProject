using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Org.Front.Core.Contracts.Models.Deserialization
{
    public class IntToStringConverter : JsonConverter<int>
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeof(int) == typeToConvert;
        }

        public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Number)
            {
                return reader.GetInt32();
            }

            if (reader.TokenType == JsonTokenType.String)
            {
                var str = reader.GetString();
                return int.Parse(str, CultureInfo.InvariantCulture);
            }

            throw new InvalidOperationException($"Invalid json token: {reader.TokenType}");
        }

        public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
