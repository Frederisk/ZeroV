using System;
using System.Text.Json.Serialization;

using ZeroV.Game.Utils.Json;

namespace ZeroV.Game.Objects;
public record ResultInfo {
    [JsonConverter(typeof(GuidJsonConverter))]
    public required Guid UUID { get; init; }

    public required Int32 Index { get; init; }

    public required Version GameVersion { get; init; }

    public required Double Scoring { get; init; }

    public required Boolean IsFullCombo { get; init; }

    public required Boolean IsAllPerfect { get; init; }

    public required Boolean IsAllDone { get; init; }
}
