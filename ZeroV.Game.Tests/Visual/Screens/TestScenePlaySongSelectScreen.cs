using NUnit.Framework;

using osu.Framework.Graphics;
using osu.Framework.Screens;

using ZeroV.Game.Screens;

namespace ZeroV.Game.Tests.Visual.Screens;

[TestFixture]
public partial class TestScenePlaySongSelectScreen : ZeroVTestScene {

    private readonly ScreenStack screenStack;

    public TestScenePlaySongSelectScreen() {
        this.Add(this.screenStack = new ScreenStack());
        
    }

    [Test]
    public void CreateScreen() {
        this.screenStack.Push(new PlaySongSelectScreen() { RelativeSizeAxes = Axes.Both });
    }
}
