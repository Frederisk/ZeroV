using System;
using System.Diagnostics;

using osu.Framework.Graphics;

using osuTK;

using ZeroV.Game.Graphics;
using ZeroV.Game.Scoring;
using ZeroV.Game.Utils;

namespace ZeroV.Game.Elements.Particles;

/// <summary>
/// The base class for all particles.
/// </summary>
public abstract partial class ParticleBase : ZeroVPoolableDrawable<ParticleSource> {
    public Boolean IsHidden { get; protected set; }

    public ParticleBase() {
        this.Origin = Anchor.Centre;
        this.Anchor = Anchor.Centre;
        this.AutoSizeAxes = Axes.Both;
        this.IsHidden = false;
        this.Y = -(ZeroVMath.SCREEN_DRAWABLE_Y + (ZeroVMath.DIAMOND_SIZE / 2));
    }

    protected override void FreeAfterUse() {
        // Reset Particle
        this.Y = -(ZeroVMath.SCREEN_DRAWABLE_Y + (ZeroVMath.DIAMOND_SIZE / 2));
        this.Alpha = 1f;
        this.IsHidden = false;
        base.FreeAfterUse();
    }

    protected virtual TargetResult JudgeMain(in Double targetTime, in Double currentTime) {
        // -: late, +: early,
        var offset = targetTime - currentTime;

        return offset switch {
            var x when x is > +ZeroVMath.JUDGE_TIME_MILLISECONDS_NONE_OR_MISS => TargetResult.None,
            var x when x is < -ZeroVMath.JUDGE_TIME_MILLISECONDS_NONE_OR_MISS => TargetResult.Miss,
            var x when x is < -ZeroVMath.JUDGE_TIME_MILLISECONDS_NORMAL => TargetResult.NormalLate,
            var x when x is > +ZeroVMath.JUDGE_TIME_MILLISECONDS_NORMAL => TargetResult.NormalEarly,
            var x when x is < -ZeroVMath.JUDGE_TIME_MILLISECONDS_PERFECT => TargetResult.PerfectLate,
            var x when x is > +ZeroVMath.JUDGE_TIME_MILLISECONDS_PERFECT => TargetResult.PerfectEarly,
            _ => TargetResult.MaxPerfect,
        };
    }

    public virtual TargetResult? JudgeEnter(in Double currentTime, in Boolean isTouchDown) {
        return null;
    }

    public virtual TargetResult? JudgeMove(in Double currentTime, in Vector2 delta) {
        return null;
    }

    public virtual TargetResult? JudgeLeave(in Double currentTime, in Boolean isTouchUp) {
        return null;
    }

    public virtual TargetResult? JudgeUpdate(in Double currentTime, in Boolean hasTouches) {
        // judge miss
        TargetResult result = this.JudgeMain(this.Source!.StartTime, currentTime);
        if (result is TargetResult.Miss) {
            return result;
        }
        // ignore other results
        return null;
    }

    public virtual void OnDequeueInJudge(TargetResult result) {
        Debug.Assert(result is not TargetResult.None);
        if (result is not TargetResult.Miss) {
            this.Alpha = 0f;
            this.IsHidden = true;
        }
    }
}
