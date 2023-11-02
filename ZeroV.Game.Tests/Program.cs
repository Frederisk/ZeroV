using osu.Framework;
using osu.Framework.Platform;

namespace ZeroV.Game.Tests;

public static class Program {
    public static void Main() {
        using GameHost host = Host.GetSuitableDesktopHost("visual-tests");
        using ZeroVTestBrowser game = new();
        host.Run(game);
    }
}
