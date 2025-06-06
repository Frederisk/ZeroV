using NUnit.Framework;

using osu.Framework.Graphics;
using osu.Framework.Screens;

using ZeroV.Game.Screens;

namespace ZeroV.Game.Tests.Visual.Screens;

[TestFixture]
public partial class TestSceneMainScreen : ZeroVTestScene {
    // Add visual tests to ensure correct behaviour of your game: https://github.com/ppy/osu-framework/wiki/Development-and-Testing
    // You can make changes to classes associated with the tests and they will recompile and update immediately.

    public TestSceneMainScreen() {
        this.Add(new ScreenStack(new MainScreen()) { RelativeSizeAxes = Axes.Both });
    }
}
