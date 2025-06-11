using NUnit.Framework;

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Screens;

using ZeroV.Game.Screens.Preference;

namespace ZeroV.Game.Tests.Visual.Screens.Preference;

[TestFixture]
public partial class TestSceneFileSelectorScreen : ZeroVTestScene {

    [BackgroundDependencyLoader]
    private void load() {
        this.Add(new ScreenStack(new FileSelectorScreen()) { RelativeSizeAxes = Axes.Both });
    }
}
