using System;

using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;

using osu.Framework.Android;

using ZeroV.Game;

namespace ZeroV.Android;

[Activity(ConfigurationChanges = DEFAULT_CONFIG_CHANGES, Exported = true, LaunchMode = DEFAULT_LAUNCH_MODE, MainLauncher = true, ScreenOrientation = ScreenOrientation.SensorLandscape)]
public class ZeroVActivity : AndroidGameActivity {

    protected override osu.Framework.Game CreateGame() => new ZeroVGame();

    protected override void OnCreate(Bundle? savedInstanceState) {
        base.OnCreate(savedInstanceState);

        // Request maximum refresh rate mode for high performance display on Android devices
        if (Build.VERSION.SdkInt >= BuildVersionCodes.M && this.Window?.WindowManager?.DefaultDisplay is not null) {
#pragma warning disable CA1416 // The system version has been verified. It's saft to ignore Validate platform compatibility
            Display display = this.Window.WindowManager.DefaultDisplay;
            Display.Mode[]? modes = display.GetSupportedModes();
            if (modes is not null && modes.Length > 0) {
                Display.Mode? maxMode = null;
                foreach (Display.Mode mode in modes) {
                    if (maxMode is null || mode.RefreshRate > maxMode.RefreshRate) {
                        maxMode = mode;
                    }
                }

                if (maxMode is not null && this.Window.Attributes is not null) {
                    WindowManagerLayoutParams layoutParams = this.Window.Attributes;
                    layoutParams.PreferredDisplayModeId = maxMode.ModeId;
                    this.Window.Attributes = layoutParams;
                }
            }
#pragma warning restore CA1416 // The system version has been verified. It's saft to ignore Validate platform compatibility
        }
    }

    public override void SetOrientationBis(Int32 p0, Int32 p1, Boolean p2, String? p3) {
        // Do nothing here to avoid setting orientation by SDL.
        // base.SetOrientationBis(p0, p1, p2, p3);
    }
}
