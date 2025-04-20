using System;

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Pooling;
using osu.Framework.Graphics.Shapes;

using osuTK;

using ZeroV.Game.Graphics.Shapes;

namespace ZeroV.Game.Elements;

public partial class TargetSpinEffect : PoolableDrawable {
    private RainbowDiamond rainbowDiamond = null!;

    public TargetSpinEffect() {
        this.Origin = Anchor.Centre;
        this.Anchor = Anchor.Centre;
        this.Size = new Vector2(256);
    }

    [BackgroundDependencyLoader]
    private void load() {
        this.rainbowDiamond = new RainbowDiamond {
            Origin = Anchor.Centre,
            Anchor = Anchor.Centre,
            RelativeSizeAxes = Axes.Both,
            HsvaColour = new Vector4(-1, 0.5f, 1, 0.7f),
            SizeRatio = 0,
            BorderRatio = 0.04f,
        };
        this.InternalChild = this.rainbowDiamond;
    }

    protected override void PrepareForUse() {
        base.PrepareForUse();

        this.rainbowDiamond.TransformTo<RainbowDiamond, Single>(nameof(this.rainbowDiamond.SizeRatio), 1, 500, Easing.Out);
        this.rainbowDiamond.RotateTo(180, 500, Easing.Out);
        this.rainbowDiamond.Delay(400).Then().FadeOut(100);
        this.Delay(700).Then().Expire();
    }

    protected override void FreeAfterUse() {
        base.FreeAfterUse();

        this.rainbowDiamond.ClearTransforms();
        this.rainbowDiamond.SizeRatio = 0;
        this.rainbowDiamond.Rotation = 0;
        this.rainbowDiamond.Alpha = 1;
    }
}
