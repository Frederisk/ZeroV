using System;

namespace ZeroV.Game.Objects;

/// <summary>
/// Represents a beatmap info.
/// </summary>
public record MapInfo {
    public required TimeSpan MapOffset { get; init; }

    public required Double Difficulty { get; init; }

    public required Int32 BlinkCount { get; init; }

    public required Int32 PressCount { get; init; }

    public required Int32 SlideCount { get; init; }

    public required Int32 StrokeCount { get; init; }
}
