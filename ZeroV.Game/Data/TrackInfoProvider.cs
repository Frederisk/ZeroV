using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using ZeroV.Game.Data.KeyValueStorage;
using ZeroV.Game.Objects;

namespace ZeroV.Game.Data;

public class TrackInfoProvider(IKeyValueStorage keyValueStorage) : StorageDataProvider<IReadOnlyList<TrackInfo>>(keyValueStorage) {
    protected override String StorageKey { get; } = "TrackInfoList";
}
