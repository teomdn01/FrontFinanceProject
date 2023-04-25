using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Org.Front.Core.Contracts.Models.Deserialization
{
    public class StringOrNumberToDecimalConverter : JsonConverter<decimal?>
    {
        public override bool CanConvert(Type objectType) => true;

        
        public override decimal? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return null;
            }
            
            using var jsonDocument = JsonDocument.ParseValue(ref reader);
            if (jsonDocument.RootElement.ValueKind == JsonValueKind.String)
            {
                var stringValue = JsonSerializer.Deserialize(ref reader, typeof(string), options);
                return Convert.ToDecimal(stringValue, CultureInfo.InvariantCulture);
            }
            else
            {
                return JsonSerializer.Deserialize(ref reader, typeof(decimal), options) as decimal?;
            }
        }

        public override void Write(Utf8JsonWriter writer, decimal? value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, value.GetType(), options);
        }
    }
}
