using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Org.Front.Core.Contracts.Models.Serialization
{
    public class Iso8601DateTimeUtcConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options) =>
            DateTime.SpecifyKind(
                DateTimeOffset.ParseExact(
                    reader.GetString(),
                    "yyyy-MM-ddTHH:mm:ss.fffZ",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None).DateTime,
                DateTimeKind.Utc);

        public override void Write(
            Utf8JsonWriter writer,
            DateTime dateTimeValue,
            JsonSerializerOptions options) =>
            writer.WriteStringValue(dateTimeValue.ToString("yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture));
    }
}