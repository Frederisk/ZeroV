using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;

using osuTK;

using ZeroV.Game.Utils;

namespace ZeroV.Game.Graphics.Icons;

public partial class CrossIcon : IconBase {

    [BackgroundDependencyLoader]
    private void load() {
        this.InternalChild = new Container {
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            RelativeSizeAxes = Axes.Both,
            Size = new Vector2(3 / (ZeroVMath.SQRT_2 * ZeroVMath.SQRT_5)),
            Rotation = 45,
            Children = [
                new Box {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    Height = 1f / 3f,
                    //Rotation = 45,
                },
                new Box {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    Width = 1f / 3f,
                    //Rotation = 45,
                },
            ],
        };
    }
}
