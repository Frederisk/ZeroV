using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using osu.Framework.Allocation;
using osu.Framework.Configuration;
using osu.Framework.Platform;

namespace ZeroV.Game.Configs;

public partial class ZeroVConfigManager(Storage storage, IDictionary<ZeroVSetting, Object>? defaultOverrides = null) : IniConfigManager<ZeroVSetting>(storage, defaultOverrides) {
    protected override void InitialiseDefaults() {
        // base.InitialiseDefaults(); // It's empty.

        this.SetDefault<Int32>(ZeroVSetting.StartTimeOffset, 0);
        this.SetDefault<Int32>(ZeroVSetting.MaxPerfectOffset, 0);
        this.SetDefault<Int32>(ZeroVSetting.PerfectOffset, 0);
        this.SetDefault<Int32>(ZeroVSetting.GoodOffset, 0);
    }
}
