using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Game.Tests.Visual;

using osuTK.Graphics;

namespace osu.Game.Rulesets.ZeroV.Tests;

public partial class TestSceneOsuGame : OsuTestScene {
    [BackgroundDependencyLoader]
    private void load() {
        this.Children = new Drawable[] {
            new Box {
                RelativeSizeAxes = Axes.Both,
                Colour = Color4.Black,
            },
        };

        this.AddGame(new OsuGame());
    }
}
