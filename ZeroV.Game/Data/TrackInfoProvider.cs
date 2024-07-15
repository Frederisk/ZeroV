using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using ZeroV.Game.Data.KeyValueStorage;
using ZeroV.Game.Objects;

namespace ZeroV.Game.Data;

public class TrackInfoProvider(IKeyValueStorage keyValueStorage) {
    private IKeyValueStorage keyValueStorage { get; } = keyValueStorage;

    private readonly String track_info_list_key = "TrackInfoList";

    public virtual async Task SetTrackInfoListAsync(IReadOnlyList<TrackInfo> trackInfos) =>
        await this.keyValueStorage.SetAsync<IReadOnlyList<TrackInfo>>(track_info_list_key, trackInfos);

    public virtual async Task<IReadOnlyList<TrackInfo>?> GetTrackInfoListAsync() =>
         await this.keyValueStorage.GetAsync<IReadOnlyList<TrackInfo>>(track_info_list_key);
}
