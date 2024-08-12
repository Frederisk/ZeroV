using System;

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Shapes;

using ZeroV.Game.Graphics.Shapes;
using ZeroV.Game.Scoring;
using ZeroV.Game.Screens.Gameplay;
using ZeroV.Game.Utils;

namespace ZeroV.Game.Elements.Particles;

public partial class PressParticle : ParticleBase {

    public PressParticle() : base() {
        //this.Type = ParticleType.Press;
        this.AutoSizeAxes = Axes.X;
        this.Origin = Anchor.BottomCentre;
    }

    private BlinkDiamond bottomDiamond = null!;
    private CompositeDrawable pillarBox = null!;

    [BackgroundDependencyLoader]
    private void load() {
        this.pillarBox = new Container<Box> {
            Origin = Anchor.TopCentre,
            Anchor = Anchor.TopCentre,
            Width = ZeroVMath.DIAMOND_SIZE,
            Children = [
                new Box {
                    Origin = Anchor.TopCentre,
                    Anchor = Anchor.TopCentre,
                    //Width = ZeroVMath.DIAMOND_SIZE,
                    RelativeSizeAxes = Axes.Both,
                    Colour = Colour4.Pink,
                },
                // Three black lines.
                new Box {
                    Origin = Anchor.TopCentre,
                    Anchor = Anchor.TopCentre,
                    Width = 6.1f,
                    RelativeSizeAxes = Axes.Y,
                    Colour = Colour4.Black,
                },
                new Box {
                    Origin = Anchor.TopLeft,
                    Anchor = Anchor.TopLeft,
                    Width = 6.1f,
                    RelativeSizeAxes = Axes.Y,
                    Colour = Colour4.Black,
                },
                new Box {
                    Origin = Anchor.TopRight,
                    Anchor = Anchor.TopRight,
                    Width = 6.1f,
                    RelativeSizeAxes = Axes.Y,
                    Colour = Colour4.Black,
                },
            ],
        };

        this.InternalChildren = [
            this.pillarBox,
            // bottom
            this.bottomDiamond = new BlinkDiamond {
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
            Y = maskingBounds.Y + (ZeroVMath.DIAMOND_SIZE / 2),
            Height = maskingBounds.Height + ZeroVMath.DIAMOND_SIZE,
        };
        return base.ComputeIsMaskedAway(realMasking);
    }

    private TargetResult result;
    private Double? noTouchTime;

    private readonly Double deltaTime = TimeSpan.FromMilliseconds(100).TotalMilliseconds;

    // protected override TargetResult JudgeMain(in Double targetTime, in Double currentTime) =>
    //     base.JudgeMain(targetTime, currentTime);

    public override TargetResult? JudgeEnter(in Double currentTime, in Boolean isTouchDown) {
        // When this particle is touched down first time, judge it and remember the result.
        if (isTouchDown && this.result is TargetResult.None) {
            this.result = this.JudgeMain(this.Source!.StartTime, currentTime);
        }
        return null;
    }

    public override TargetResult? JudgeUpdate(in Double currentTime, in Boolean hasTouches) {
        // If this particle is never touched down, judge miss.
        if (this.result is TargetResult.None) {
            return base.JudgeUpdate(currentTime, hasTouches); // judge miss here
        }
        // If this particle is holding...
        if (hasTouches) {
            // If it's holding and the time is enough, return the touch down result.
            if (currentTime >= this.Source!.EndTime) {
                return this.result;
            }
            // Clear the no touch time.
            this.noTouchTime = null;
            // And update the length of the particle.
            if (currentTime > this.Source!.StartTime) {
                this.updateInnerLength(currentTime);
            }
        }
        // If this particle is not holding...
        else {
            // Judge it immediately.
            TargetResult endResult = this.JudgeMain(this.Source!.EndTime, currentTime);
            // If it's not too early, return the touch down result.
            if (endResult is not TargetResult.None) {
                return this.result;
            }
            // Wait for the deltaTime to judge, until time is up. When time is up, judge miss.
            if ((currentTime - this.noTouchTime) > this.deltaTime) {
                this.result = TargetResult.Miss;
                return TargetResult.Miss;
            }
            // Set the first no touch time.
            this.noTouchTime ??= currentTime;
        }
        return null;
    }

    private void updateInnerLength(Double currentTime) {
        this.bottomDiamond.Y = -(Single)((ZeroVMath.SCREEN_DRAWABLE_Y + (ZeroVMath.DIAMOND_SIZE / 2) - ZeroVMath.SCREEN_GAME_BASELINE_Y) * (currentTime - this.Source!.StartTime) / this.gameplayScreen.ParticleFallingTime);
        this.pillarBox.Height = (Single)((ZeroVMath.SCREEN_DRAWABLE_Y + (ZeroVMath.DIAMOND_SIZE / 2) - ZeroVMath.SCREEN_GAME_BASELINE_Y) * (this.Source!.EndTime - currentTime) / this.gameplayScreen.ParticleFallingTime);
    }

    [Resolved]
    private GameplayScreen gameplayScreen { get; set; } = null!;

    public void UpdateLength(Double startTime, Double endTime) {
        this.Height = (Single)((ZeroVMath.SCREEN_DRAWABLE_Y + (ZeroVMath.DIAMOND_SIZE / 2) - ZeroVMath.SCREEN_GAME_BASELINE_Y) * (endTime - startTime) / this.gameplayScreen.ParticleFallingTime);
        this.pillarBox.Height = this.Height;
    }

    public override void OnDequeueInJudge() {
        base.OnDequeueInJudge(); // this.Alpha = 0f; and markup
        this.Alpha = this.result is TargetResult.Miss or TargetResult.None ? 0.5f : 0;
    }

    protected override void FreeAfterUse() {
        // Reset Particle
        this.result = TargetResult.None;
        this.noTouchTime = null;
        this.bottomDiamond.Y = 0;
        base.FreeAfterUse();
    }
}
