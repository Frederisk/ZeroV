using Android.App;

using osu.Framework.Android;

using ZeroV.Game;

namespace ZeroV.Android;

[Activity(ConfigurationChanges = DEFAULT_CONFIG_CHANGES, Exported = true, LaunchMode = DEFAULT_LAUNCH_MODE, MainLauncher = true)]
public class ZeroVActivity : AndroidGameActivity {
    protected override osu.Framework.Game CreateGame() => new ZeroVGame();
}
