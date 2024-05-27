using System;

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
    //public ParticleType Type { get; protected init; }

    public ParticleBase() {
        this.Origin = Anchor.Centre;
        this.Anchor = Anchor.Centre;
        this.AutoSizeAxes = Axes.Both;
        this.Y = -(ZeroVMath.SCREEN_DRAWABLE_Y + (ZeroVMath.DIAMOND_SIZE / 2));
    }

    protected override void FreeAfterUse() {
        // Reset Particle
        this.Y = -(ZeroVMath.SCREEN_DRAWABLE_Y + (ZeroVMath.DIAMOND_SIZE / 2));
        this.Alpha = 1f;
        base.FreeAfterUse();
    }

    protected virtual TargetResult JudgeMain(in Double targetTime, in Double currentTime) {
        // -: late, +: early,
        var offset = targetTime - currentTime;

        // late------------------------early
        // xxxxx-1000======0======+1000xxxxx
        return offset switch {
            // -1000~: None
            var x when x is > +1000 => TargetResult.None,
            // ~1000: Miss
            var x when x is < -1000 => TargetResult.Miss,
            // 1000~500: Bad
            var x when x is < -500 => TargetResult.NormalLate,
            var x when x is > +500 => TargetResult.NormalEarly,
            // 500~300: Normal
            var x when x is < -100 => TargetResult.PerfectLate,
            var x when x is > +100 => TargetResult.PerfectEarly,
            // 300~0: Perfect
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

    public virtual void OnDequeueInJudge() {
        this.Alpha = 0f;
    }
}
