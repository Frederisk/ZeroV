using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;

using osuTK;

using ZeroV.Game.Utils;

namespace ZeroV.Game.Graphics.Icons;

public partial class PauseIcon : IconBase {

    [BackgroundDependencyLoader]
    private void load() {
        this.InternalChild = new Container {
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            RelativeSizeAxes = Axes.Both,
            Size = new Vector2(ZeroVMath.SQRT_2 / 2f),
            Children = [
                new Box {
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                    Width = 1f / 3f,
                    // Margin = new MarginPadding(10f),
                    RelativeSizeAxes = Axes.Both,
                    //Colour = Colour4.Red,
                },
                new Box {
                    Anchor = Anchor.CentreRight,
                    Origin = Anchor.CentreRight,
                    Width = 1f / 3f,
                    // Margin = new MarginPadding(10f),
                    RelativeSizeAxes = Axes.Both,
                    //Colour = Colour4.Red,
                },
            ],
        };
    }
}
