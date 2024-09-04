using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ZeroV.Game.Utils.Json;

public class GuidJsonConverter : JsonConverter<Guid> {

    public override Guid Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        String guidString = reader.GetString() ?? throw new JsonException("Expected a string value.");
        return new Guid(guidString);
    }

    public override void Write(Utf8JsonWriter writer, Guid value, JsonSerializerOptions options) {
        writer.WriteStringValue(value.ToString());
    }
}
