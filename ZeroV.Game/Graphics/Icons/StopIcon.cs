using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;

using ZeroV.Game.Utils;

using osuTK;

namespace ZeroV.Game.Graphics.Icons;

public partial class StopIcon : IconBase {

    [BackgroundDependencyLoader]
    private void load() {
        this.InternalChild = new Box {
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            RelativeSizeAxes = Axes.Both,
            Size = new Vector2(ZeroVMath.SQRT_2 / 2f), // 0.7071067811865476
        };
    }
}
