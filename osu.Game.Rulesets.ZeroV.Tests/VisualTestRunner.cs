using System;

using osu.Framework;
using osu.Framework.Platform;
using osu.Game.Tests;

namespace osu.Game.Rulesets.ZeroV.Tests;

public static class VisualTestRunner {
    [STAThread]
    public static Int32 Main(String[] args) {
        using DesktopGameHost host = Host.GetSuitableDesktopHost(@"osu", new HostOptions { BindIPC = true });
        host.Run(new OsuTestBrowser());
        return 0;
    }
}
