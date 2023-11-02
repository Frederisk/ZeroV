using osu.Framework;
using osu.Framework.Platform;

using ZeroV.Game;

namespace ZeroV.Desktop;

public static class Program {
    public static void Main() {
        using GameHost host = Host.GetSuitableDesktopHost(@"ZeroV");
        using osu.Framework.Game game = new ZeroVGame();
        host.Run(game);
    }
}
