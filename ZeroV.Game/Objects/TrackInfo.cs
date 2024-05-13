using System;
using System.Collections.Generic;

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

    public required List<MapInfo> Maps { get; init; }
}
