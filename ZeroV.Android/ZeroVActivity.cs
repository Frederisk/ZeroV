using Android.App;
using Android.Content.PM;

using osu.Framework.Android;

using ZeroV.Game;

namespace ZeroV.Android;

[Activity(ConfigurationChanges = DEFAULT_CONFIG_CHANGES, Exported = true, LaunchMode = DEFAULT_LAUNCH_MODE, MainLauncher = true, ScreenOrientation = ScreenOrientation.SensorLandscape)]
public class ZeroVActivity : AndroidGameActivity {

    protected override osu.Framework.Game CreateGame() => new ZeroVGame();

    public override void SetOrientationBis(System.Int32 p0, System.Int32 p1, System.Boolean p2, System.String? p3) {
        // Do nothing here to avoid setting orientation by SDL.
        // base.SetOrientationBis(p0, p1, p2, p3);
    }
}
