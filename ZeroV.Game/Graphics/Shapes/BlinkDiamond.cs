using System;

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;

using osuTK;

using ZeroV.Game.Utils;

namespace ZeroV.Game.Graphics.Shapes;

public partial class BlinkDiamond : CompositeDrawable {

    public BlinkDiamond() {
        this.Anchor = Anchor.Centre;
        this.Origin = Anchor.Centre;
        this.AutoSizeAxes = Axes.Both;
    }

    public Single OuterDiameterSize { get; init; } = ZeroVMath.DIAMOND_OUTER_SIZE;

    public Single InnerDiameterSize { get; init; } = ZeroVMath.DIAMOND_INNER_SIZE;

    public Colour4 OuterColor { get; init; } = Colour4.Black;

    public Colour4 InnerColor { get; init; } = Colour4.Red;

    [BackgroundDependencyLoader]
    private void load() {
        this.InternalChildren = [
            new Diamond {
                Size = new Vector2(this.OuterDiameterSize),
                Colour = this.OuterColor,
            },
            new Diamond {
                Size = new Vector2(this.InnerDiameterSize),
                Colour = this.InnerColor,
            },
        ];
    }
}
