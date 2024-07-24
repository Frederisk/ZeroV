using NUnit.Framework;

using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;

using ZeroV.Game.Screens;

namespace ZeroV.Game.Tests.Visual.Screens;

[TestFixture]
public partial class TestScenePauseOverlay : ZeroVTestScene {
    private PauseOverlay overlay = default!;

    [Test]
    public void CreateOverlay() {
        this.Add(new Box() { RelativeSizeAxes = Axes.Both, Colour = Colour4.White });

#pragma warning disable IDE0028 // Use collection initializers or expressions
        this.Add(this.overlay = new PauseOverlay());
#pragma warning restore IDE0028 // Use collection initializers or expressions

        this.overlay.Show();
    }

}
