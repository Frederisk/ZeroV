using System;
using System.Collections.Generic;

using osu.Framework.Platform;

using ZeroV.Game.Objects;

namespace ZeroV.Game.Data;

public class TrackInfoProvider(Storage storage) {
    // TODO: Implement IBeatmapWrapperProvider
    public IReadOnlyList<TrackInfo> TrackInfoList => throw new NotImplementedException();
}
