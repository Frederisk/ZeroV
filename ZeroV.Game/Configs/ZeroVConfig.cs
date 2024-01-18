using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using osu.Framework.Allocation;
using osu.Framework.Configuration;
using osu.Framework.Platform;

namespace ZeroV.Game.Configs;

public partial class ZeroVConfig : IniConfigManager<ZeroVGameSetting> {

    public ZeroVConfig(Storage storage, IDictionary<ZeroVGameSetting, Object>? defaultOverrides = null) : base(storage, defaultOverrides) {
    }

    protected override void InitialiseDefaults() {
        // base.InitialiseDefaults(); // It's empty.

        this.SetDefault<Int32>(ZeroVGameSetting.StartTimeOffset, 0);
        this.SetDefault<Int32>(ZeroVGameSetting.MaxPerfectOffset, 0);
        this.SetDefault<Int32>(ZeroVGameSetting.PerfectOffset, 0);
        this.SetDefault<Int32>(ZeroVGameSetting.GoodOffset, 0);
    }
}
