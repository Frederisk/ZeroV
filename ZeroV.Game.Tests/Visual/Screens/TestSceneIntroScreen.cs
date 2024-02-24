using osu.Framework.Graphics;
using osu.Framework.Screens;

using ZeroV.Game.Screens;

namespace ZeroV.Game.Tests.Visual.Screens;

public partial class TestSceneIntroScreen : ZeroVTestScene {
    public TestSceneIntroScreen() {
        this.Add(new ScreenStack(new IntroScreen()) { RelativeSizeAxes = Axes.Both });
    }

}
