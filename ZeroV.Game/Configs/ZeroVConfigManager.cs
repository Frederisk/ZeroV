using System;
using System.Collections.Generic;

using osu.Framework.Configuration;
using osu.Framework.Platform;

using ZeroV.Game.Utils;

namespace ZeroV.Game.Configs;

public partial class ZeroVConfigManager(Storage storage, IDictionary<ZeroVSetting, Object>? defaultOverrides = null) : IniConfigManager<ZeroVSetting>(storage, defaultOverrides) {
    protected Storage Storage = storage;

    protected override void InitialiseDefaults() {
        // base.InitialiseDefaults(); // It's empty.
        this.SetDefault<Double>(ZeroVSetting.GlobalSoundOffset, 0);
        this.SetDefault<String>(ZeroVSetting.BeatmapStoragePath, this.Storage.GetFullPath(ZeroVPath.BEATMAPS_STORAGE_PATH));
        this.SetDefault<Double>(ZeroVSetting.GamePlayParticleFallingTime, TimeSpan.FromSeconds(2).TotalMilliseconds);
    }
}
