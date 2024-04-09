using System;

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Shapes;

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
                Colour = Colour4.Pink,
            },
            new Box {
                Origin = Anchor.BottomCentre,
                Anchor = Anchor.BottomCentre,
                Width = 6.1f,
                RelativeSizeAxes = Axes.Y,
                Colour = Colour4.Black,
            },
            new Box {
                Origin = Anchor.BottomLeft,
                Anchor = Anchor.BottomLeft,
                Width = 6.1f,
                RelativeSizeAxes = Axes.Y,
                Colour = Colour4.Black,
            },
            new Box {
                Origin = Anchor.BottomRight,
                Anchor = Anchor.BottomRight,
                Width = 6.1f,
                RelativeSizeAxes = Axes.Y,
                Colour = Colour4.Black,
            },
            // bottom
            new BlinkDiamond {
                Anchor = Anchor.BottomCentre,
                InnerColor = Colour4.Pink,
                OuterColor = Colour4.Black,
            },
            // top
            new BlinkDiamond {
                Anchor = Anchor.TopCentre,
                InnerColor = Colour4.Pink,
                OuterColor = Colour4.Black,
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
    private Double? noTouchTime;

    private readonly Double deltaTime = TimeSpan.FromMilliseconds(100).TotalMilliseconds;

    // protected override TargetResult JudgeMain(in Double targetTime, in Double currentTime) =>
    //     base.JudgeMain(targetTime, currentTime);

    public override TargetResult? JudgeEnter(in Double currentTime, in Boolean isNewTouch) {
        if (isNewTouch && this.result is TargetResult.None) {
            this.result = this.JudgeMain(this.Source!.StartTime, currentTime);
            return TargetResult.None;
        }
        return null;
    }

    public override TargetResult? JudgeUpdate(in Double currentTime, in Boolean hasTouches) {
        if (this.result is not TargetResult.None) {
            if ((currentTime - this.noTouchTime) > this.deltaTime) {
                if (hasTouches) {
                    this.noTouchTime = null;
                } else {
                    return TargetResult.Miss;
                }
            }

            TargetResult endResult = this.JudgeMain(this.Source!.EndTime, currentTime);
            if (!hasTouches) {
                if (endResult is TargetResult.None) {
                    if (this.noTouchTime is null) {
                        this.noTouchTime = currentTime;
                    } else {
                        return TargetResult.None;
                    }
                } else {
                    return this.result;
                }
            } else {
                this.noTouchTime = null;
                // TODO: Calculate the length here.
                if (endResult is TargetResult.MaxPerfect) {
                    return this.result;
                }
            }
        } else {
            TargetResult result = this.JudgeMain(this.Source!.StartTime, currentTime);
            if (result is TargetResult.Miss) {
                return TargetResult.Miss;
            }
        }

        return null;
    }

    protected override void FreeAfterUse() {
        // Reset Particle
        this.result = TargetResult.None;
        this.noTouchTime = null;
        base.FreeAfterUse();
    }
}
