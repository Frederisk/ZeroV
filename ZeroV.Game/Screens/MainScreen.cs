using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Screens;

using osuTK;

using ZeroV.Game.Utils;

namespace ZeroV.Game.Screens;

public partial class MainScreen : Screen {

    [BackgroundDependencyLoader]
    private void load() {
        this.InternalChildren = [
            new Box {
                Colour = Colour4.Violet,
                // 1366 * 768
                RelativeSizeAxes = Axes.Both,
            },
            new Box {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Size = new Vector2(ZeroVMath.SCREEN_DRAWABLE_X, ZeroVMath.SCREEN_DRAWABLE_Y),
            },
            //new SpriteText {
            //    Y = 20,
            //    Text = "Main Screen",
            //    Anchor = Anchor.TopCentre,
            //    Origin = Anchor.TopCentre,
            //    Font = FontUsage.Default.With(size: 40)
            //},
            //new SpinningBox {
            //    Anchor = Anchor.Centre,
            //}
            //new Orbit {
            //    Anchor = Anchor.Centre,
            //    //Height = 1024
            //},
            //new BlinkParticles
            //{
            //    Anchor = Anchor.Centre
            //}
        ];
    }
}
