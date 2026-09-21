using System;
using System.Text.Json.Serialization;

using ZeroV.Game.Utils.Json;

namespace ZeroV.Game.Objects;

public record ResultInfo {
    /// <summary>
    /// The UUID of the track.
    /// </summary>
    [JsonConverter(typeof(GuidJsonConverter))]
    public required Guid UUID { get; init; }

    /// <summary>
    /// The index of map.
    /// </summary>
    public required Int32 Index { get; init; }

    public required Version GameVersion { get; init; }

    public required Double Scoring { get; init; }

    public required Boolean IsFullCombo { get; init; }

    public required Boolean IsAllPerfect { get; init; }

    /// <summary>
    /// Have all the particles been judged.
    /// </summary>
    public required Boolean IsAllDone { get; init; }

    /// <summary>
    /// The time at which this score was obtained.
    /// </summary>
    public required DateTime FinishTime { get; init; }
}
