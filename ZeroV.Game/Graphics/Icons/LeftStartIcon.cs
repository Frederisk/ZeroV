using osu.Framework.Allocation;
using osu.Framework.Graphics.Rendering;
using osu.Framework.Graphics;

using ZeroV.Game.Utils;
using ZeroV.Game.Graphics.Shapes;

namespace ZeroV.Game.Graphics.Icons;

public partial class LeftStartIcon : IconBase {

    [BackgroundDependencyLoader]
    private void load(IRenderer renderer) {
        this.InternalChild = new OrientedTriangle(OrientedTriangle.Orientation.Left) {
            Anchor = Anchor.CentreLeft,
            Origin = Anchor.CentreLeft,
            Height = ZeroVMath.SQRT_3 / 2.0f,
            Width = 0.75f,
            RelativeSizeAxes = Axes.Both,
        };
    }
}
