using osu.Framework.Allocation;
using osu.Framework.Graphics;

using ZeroV.Game.Utils;
using ZeroV.Game.Graphics.Shapes;

namespace ZeroV.Game.Graphics.Icons;

public partial class StartIcon : IconBase {

    [BackgroundDependencyLoader]
    private void load() {
        this.InternalChild = new OrientedTriangle(OrientedTriangle.Orientation.Right) {
            Anchor = Anchor.CentreRight,
            Origin = Anchor.CentreRight,
            Height = ZeroVMath.SQRT_3 / 2.0f,
            Width = 0.75f,
            RelativeSizeAxes = Axes.Both,
        };
    }
}
