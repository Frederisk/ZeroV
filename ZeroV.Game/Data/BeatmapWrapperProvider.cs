using System;
using System.Collections.Generic;

using osu.Framework.Platform;

namespace ZeroV.Game.Data;

public class BeatmapWrapperProvider(Storage storage) {
    // TODO: Implement IBeatmapWrapperProvider
    public IReadOnlyList<BeatmapWrapper> BeatmapWrappers => throw new NotImplementedException();
}
