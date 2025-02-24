using NUnit.Framework;

using osu.Framework.Graphics;
using osu.Framework.Screens;

using ZeroV.Game.Screens;

namespace ZeroV.Game.Tests.Visual.Screens;

[TestFixture]
public partial class TestScenePreferenceScreen : ZeroVTestScene {
    public TestScenePreferenceScreen() {
        this.Add(new ScreenStack(new PreferenceScreen()) { RelativeSizeAxes = Axes.Both});
    }
}
