using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Screens;

using osuTK;
using osuTK.Graphics;

namespace ZeroV.Game.Screens;

public partial class IntroScreen : Screen {

    [BackgroundDependencyLoader]
    private void load() {
        this.InternalChildren = new Drawable[] {
            new Box {
                Colour = Color4.Violet,
                // 1366 * 768
                RelativeSizeAxes = Axes.Both,
            },
            new Box {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Size = new Vector2(100, 100)
            },
            new SpriteText {
                Y = 20,
                Text = "IntroScreen",
                Anchor = Anchor.TopCentre,
                Origin = Anchor.TopCentre,
                Font = FontUsage.Default.With(size: 40)
            }
        };
    }

    protected override void LoadComplete() {
        base.LoadComplete();
    }

    private void continueToMain() {
        this.Push(new MainScreen());
    }
}
