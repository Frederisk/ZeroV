using NUnit.Framework;

using osu.Framework.Graphics;
using osu.Framework.Screens;

using ZeroV.Game.Screens;

namespace ZeroV.Game.Tests.Visual.Screens;

[TestFixture]
public partial class TestSceneOffsetScreen : ZeroVTestScene {

    public TestSceneOffsetScreen() {
        this.Add(new ScreenStack(new OffsetScreen()) { RelativeSizeAxes = Axes.Both });
    }
}
