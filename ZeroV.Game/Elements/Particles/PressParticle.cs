using System;

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Shapes;

using osuTK.Graphics;

using ZeroV.Game.Graphics.Shapes;
using ZeroV.Game.Scoring;

namespace ZeroV.Game.Elements.Particles;

public partial class PressParticle : ParticleBase {

    public PressParticle() : base() {
        //this.Type = ParticleType.Press;
        this.AutoSizeAxes = Axes.X;
        this.Origin = Anchor.BottomCentre;
    }

    [BackgroundDependencyLoader]
    private void load() {
        this.InternalChildren = [
            new Box {
                Origin = Anchor.BottomCentre,
                Anchor = Anchor.BottomCentre,
                Width = Single.Sqrt(2 * 52 * 52),
                RelativeSizeAxes = Axes.Y,
                Colour = Color4.Pink,
            },
            new Box {
                Origin = Anchor.BottomCentre,
                Anchor = Anchor.BottomCentre,
                Width = 6.1f,
                RelativeSizeAxes = Axes.Y,
                Colour = Color4.Black,
            },
            new Box {
                Origin = Anchor.BottomLeft,
                Anchor = Anchor.BottomLeft,
                Width = 6.1f,
                RelativeSizeAxes = Axes.Y,
                Colour = Color4.Black,
            },
            new Box {
                Origin = Anchor.BottomRight,
                Anchor = Anchor.BottomRight,
                Width = 6.1f,
                RelativeSizeAxes = Axes.Y,
                Colour = Color4.Black,
            },
            // buttom
            new BlinkDiamond {
                Anchor = Anchor.BottomCentre,
                InnerColor = Color4.Pink,
                OuterColor = Color4.Black,
            },
            // top
            new BlinkDiamond {
                Anchor = Anchor.TopCentre,
                InnerColor = Color4.Pink,
                OuterColor = Color4.Black,
            },
        ];
    }

    protected override Boolean ComputeIsMaskedAway(RectangleF maskingBounds) {
        // TODO: It seems that `with` is faster than `+=` here.
        //maskingBounds.Y += 37;
        //maskingBounds.Height += 74;
        RectangleF realMasking = maskingBounds with {
            Y = maskingBounds.Y + 37,
            Height = maskingBounds.Height + 74,
        };
        return base.ComputeIsMaskedAway(realMasking);
    }

    private TargetResult result;
    //public override TargetResult Judge(in JudgeInput input) {
    //    if (this.result == TargetResult.None) {
    //        this.result = Judgment.JudgeBlink(startTime, input.CurrentTime);
    //        switch (input.IsTouchDown) {
    //            case null or false when this.result != TargetResult.Miss:
    //                this.result = TargetResult.None;
    //                break;
    //        }

    //        return this.result switch {
    //            TargetResult.Miss => TargetResult.Miss,
    //            _ => TargetResult.None
    //        };
    //    } else {
    //        TargetResult result = Judgment.JudgeBlink(endTime, input.CurrentTime);

    //        switch (result) {
    //            case TargetResult.None when !input.HasTouches: return TargetResult.Miss;
    //            case TargetResult.None: return TargetResult.None;
    //            case TargetResult.Miss:
    //            case var _ when !input.HasTouches:
    //                return this.result;
    //            default: return TargetResult.None;
    //        }
    //    }
    //}

    protected override void FreeAfterUse() {
        // Reset Particle
        this.result = TargetResult.None;
        base.FreeAfterUse();
    }
}
