using NUnit.Framework;

using osu.Framework.Graphics;
using osu.Framework.Screens;

using ZeroV.Game.Screens;

namespace ZeroV.Game.Tests.Visual.Screens;

[TestFixture]
public partial class TestGameplayScreen : ZeroVTestScene {

    public TestGameplayScreen() {
        this.Add(new ScreenStack(new GameplayScreen { RelativeSizeAxes = Axes.Both }) { RelativeSizeAxes = Axes.Both });
    }
}
