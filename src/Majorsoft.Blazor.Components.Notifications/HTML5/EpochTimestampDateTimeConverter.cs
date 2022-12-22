using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Majorsoft.Blazor.Components.Notifications
{
    internal class EpochTimestampDateTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            long ms = reader.GetInt64();
            return DateTimeOffset.FromUnixTimeMilliseconds(ms).DateTime;
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            long ms = new DateTimeOffset(value).ToUnixTimeMilliseconds();
            writer.WriteNumberValue(ms);
        }
    }
}
