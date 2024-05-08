using System;
using System.Collections.Generic;

namespace ZeroV.Game.Objects;
public record TrackInfo {
    public required String Title { get; init; }

    public String? Album { get; init; }

    public Int32? TrackOrder { get; init; }

    public Object? Artists { get; init; }

    public required TimeSpan FileOffset { get; init; }

    public required String GameAuthor { get; init; }

    public String? Description { get; init; }

    public required Version GameVersion { get; init; }

    public required List<TrackInfo> Tracks { get; init; }
}
