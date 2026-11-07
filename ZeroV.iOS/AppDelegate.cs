using Foundation;

using osu.Framework;
using osu.Framework.iOS;

using ZeroV.Game;

namespace ZeroV.iOS;

[Register("AppDelegate")]
public class AppDelegate : GameApplicationDelegate {
    protected override osu.Framework.Game CreateGame() => new ZeroVGame();
}
