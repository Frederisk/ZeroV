using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ZeroV.Game.Objects;

/// <summary>
/// Represents a track info.
/// </summary>
public record TrackInfo {
    public required String Title { get; init; }

    public required String? Album { get; init; }

    public required Int32? TrackOrder { get; init; }

    //public required Object? Artists { get; init; }

    public required String? Artists { get; init; }

    public required TimeSpan FileOffset { get; init; }

    public required String GameAuthor { get; init; }

    public required String? Description { get; init; }

    public required Version GameVersion { get; init; }

    public required IReadOnlyList<MapInfo> MapInfos { get; init; }

    /// <summary>
    /// The audio file that contains the track.
    /// </summary>
    [JsonConverter(typeof(FileInfoJsonConverter))]
    public required FileInfo TrackFile { get; init; }

    /// <summary>
    /// The XML file that contains the beatmap information.
    /// </summary>
    [JsonConverter(typeof(FileInfoJsonConverter))]
    public required FileInfo BeatmapFile { get; init; }
    // TODO: add background image file info


    private class FileInfoJsonConverter : JsonConverter<FileInfo> {
        public override FileInfo Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
            String filePath = reader.GetString() ?? throw new JsonException("Expected a string value.");
            return new FileInfo(filePath);
        }

        public override void Write(Utf8JsonWriter writer, FileInfo value, JsonSerializerOptions options) {
            writer.WriteStringValue(value.FullName);
        }
    }
}
