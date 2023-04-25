using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Org.Front.Core.Contracts.Models.Serialization
{
    public class DateTimeUtcConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options) =>
            DateTime.SpecifyKind(DateTimeOffset.Parse(reader.GetString()).DateTime, DateTimeKind.Utc);

        public override void Write(
            Utf8JsonWriter writer,
            DateTime dateTimeValue,
            JsonSerializerOptions options) =>
                writer.WriteStringValue(dateTimeValue.ToString(CultureInfo.InvariantCulture));
    }
}
