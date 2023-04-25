using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Org.Front.Core.Contracts.Extensions;

namespace Org.Front.Core.Contracts.Models.Deserialization
{
    /// <summary>
    /// String/Number to Decimal Converter. <br />
    /// Removes trailing zeroes.<br />
    /// Returns 0 on "" input instead of throwing exception.<br />
    /// Can be used on nullable decimals.<br />
    /// </summary>
    public class TrailingZeroesRemovingDecimalConverter : JsonConverter<decimal>
    {
        public override bool CanConvert(Type objectType) => objectType == typeof(decimal);

        public override decimal Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            decimal result;
            using var jsonDocument = JsonDocument.ParseValue(ref reader);
            if (jsonDocument.RootElement.ValueKind == JsonValueKind.String)
            {
                object stringValue = JsonSerializer.Deserialize(ref reader, typeof(string), options);
                result = (string)stringValue == ""
                    ? 0
                    : Convert.ToDecimal(stringValue, CultureInfo.InvariantCulture);
            }
            else
            {
                result = (decimal)JsonSerializer.Deserialize(ref reader, typeof(decimal), options);
            }

            return result.RemoveTrailingZeroes();
        }

        public override void Write(Utf8JsonWriter writer, decimal value, JsonSerializerOptions options) =>
            JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}