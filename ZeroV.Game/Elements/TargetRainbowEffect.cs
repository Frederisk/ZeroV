using System;

using osu.Framework.Allocation;
using osu.Framework.Extensions.EnumExtensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Pooling;
using osu.Framework.Graphics.Shapes;

using osuTK;

using ZeroV.Game.Graphics.Shapes;
using ZeroV.Game.Scoring;

namespace ZeroV.Game.Elements;

public partial class TargetSpinEffect : PoolableDrawable {
    private RainbowDiamond rainbowDiamond = null!;
    private Triangle earlyTriangle = null!;
    private Triangle laterTriangle = null!;
    private TargetResult result;

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
            // HsvaColour = new Vector4(-1, 0.5f, 1, 0.7f),
            SizeRatio = 0,
            BorderRatio = 0.04f,
        };
        this.earlyTriangle = new Triangle {
            Origin = Anchor.BottomCentre,
            Anchor = Anchor.Centre,
            RelativeSizeAxes = Axes.Both,
            Size = new Vector2(0.4f, 0.2f),
            Colour = ColourInfo.GradientVertical(
                Colour4.SteelBlue.Opacity(1f),
                Colour4.SteelBlue.Opacity(0.3f)
            ),
        };
        this.earlyTriangle.FadeOut();
        this.laterTriangle = new Triangle {
            Origin = Anchor.BottomCentre,
            Anchor = Anchor.Centre,
            RelativeSizeAxes = Axes.Both,
            Size = new Vector2(0.4f, -0.2f),
            Colour = ColourInfo.GradientVertical(
                Colour4.Red.Opacity(1f),
                Colour4.Red.Opacity(0.3f)
            ),
        };
        this.laterTriangle.FadeOut();

        this.InternalChildren = [
            this.earlyTriangle,
            this.laterTriangle,
            this.rainbowDiamond,
        ];
    }

    public void SetUpTargetColour(TargetResult result) {
        this.result = result;
        this.rainbowDiamond.HsvaColour = this.result switch {
            TargetResult.MaxPerfect => new Vector4(-1, 0.5f, 1, 0.7f),
            _ when result.HasFlagFast(TargetResult.Perfect) => new Vector4(0.123f, 0.65f, 1, 0.7f),
            _ when result.HasFlagFast(TargetResult.Normal) => new Vector4(0.517f, 0.5f, 1, 0.7f),
            _ => throw new InvalidOperationException(),
        };
    }

    protected override void PrepareForUse() {
        base.PrepareForUse();

        this.rainbowDiamond.TransformTo<RainbowDiamond, Single>(nameof(this.rainbowDiamond.SizeRatio), 1, 500, Easing.Out);
        this.rainbowDiamond.RotateTo(180, 500, Easing.Out);
        this.rainbowDiamond.Delay(400).Then().FadeOut(100);
        if (this.result.HasFlagFast(TargetResult.Early)) {
            this.earlyTriangle.FadeIn(400, Easing.Out).Then().FadeOut(100);
        } else if (this.result.HasFlagFast(TargetResult.NormalLate)) {
            this.laterTriangle.FadeIn(400, Easing.Out).Then().FadeOut(100);
        }

        this.Delay(700).Then().Expire();
    }

    protected override void FreeAfterUse() {
        this.rainbowDiamond.ClearTransforms();
        this.rainbowDiamond.SizeRatio = 0;
        this.rainbowDiamond.Rotation = 0;
        this.rainbowDiamond.Alpha = 1;

        base.FreeAfterUse();
    }
}
