using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ZeroV.Game.Utils.Json;

public class FileInfoJsonConverter : JsonConverter<FileInfo> {

    public override FileInfo Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        String filePath = reader.GetString() ?? throw new JsonException("Expected a string value.");
        return new FileInfo(filePath);
    }

    public override void Write(Utf8JsonWriter writer, FileInfo value, JsonSerializerOptions options) {
        writer.WriteStringValue(value.FullName);
    }
}}
