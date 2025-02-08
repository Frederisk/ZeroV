using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;

using osuTK;

using ZeroV.Game.Graphics.Shapes;
using ZeroV.Game.Utils;


namespace ZeroV.Game.Graphics.Icons;

public partial class NextIcon : IconBase {

    [BackgroundDependencyLoader]
    private void load() {
        this.InternalChildren = [
            new Container {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                RelativeSizeAxes = Axes.Both,
                Size = new Vector2(11f / 14f, (5 * ZeroVMath.SQRT_3) / 14f),
                Child = new OrientedTriangle(OrientedTriangle.Orientation.Right) {
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                    RelativeSizeAxes = Axes.Both,
                    Width = 15f / 22f,
                },
            },
            new OrientedTriangle(OrientedTriangle.Orientation.Right) {
                Anchor = Anchor.CentreRight,
                Origin = Anchor.CentreRight,
                RelativeSizeAxes = Axes.Both,
                Size = new Vector2(15f / 28f, (5 * ZeroVMath.SQRT_3) / 14f),
            },
        ];
    }
}
