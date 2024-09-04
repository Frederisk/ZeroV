using System;
using System.Collections.Generic;

using ZeroV.Game.Data.KeyValueStorage;
using ZeroV.Game.Objects;
using ZeroV.Game.Utils;

namespace ZeroV.Game.Data;

public class TrackInfoProvider(IKeyValueStorage keyValueStorage) : StorageDataProvider<IReadOnlyList<TrackInfo>>(keyValueStorage) {
    protected override String StorageKey { get; } = ZeroVPath.TRACK_INFO_JSON_FILE;
}
