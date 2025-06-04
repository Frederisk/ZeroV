using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ZeroV.Game.Utils.Json;

public class DateTimeJsonConverter : JsonConverter<DateTime> {

    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        String timeString = reader.GetString() ?? throw new JsonException("Expected a string value.");
        return DateTime.Parse(timeString, null, DateTimeStyles.RoundtripKind);
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options) {
        writer.WriteStringValue(value.ToString("O"));
    }
}
