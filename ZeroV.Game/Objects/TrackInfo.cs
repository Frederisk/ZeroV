using System;
using System.Collections.Generic;
using System.IO;

namespace ZeroV.Game.Objects;
public record TrackInfo {
    public required String Title { get; init; }

    public required String? Album { get; init; }

    public required Int32? TrackOrder { get; init; }

    public required Object? Artists { get; init; }

    public required TimeSpan FileOffset { get; init; }

    public required String GameAuthor { get; init; }

    public required String? Description { get; init; }

    public required Version GameVersion { get; init; }

    public required IReadOnlyList<MapInfo> Maps { get; init; }

    //public required FileInfo InfoFile { get; init; }

    public required FileInfo TrackFile { get; init; }

    public required FileInfo InfoFile { get; init; }
    // TODO: add background image file info
}

public record MapInfo {
    public required TimeSpan MapOffset { get; init; }

    public required Double Difficulty { get; init; }

    public required Int32 BlinkCount { get; init; }

    public required Int32 PressCount { get; init; }

    public required Int32 SlideCount { get; init; }

    public required Int32 StrokeCount { get; init; }
}
