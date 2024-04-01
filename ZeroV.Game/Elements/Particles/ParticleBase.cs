using System;

using osu.Framework.Graphics;

using osuTK;

using ZeroV.Game.Graphics;
using ZeroV.Game.Scoring;

namespace ZeroV.Game.Elements.Particles;

/// <summary>
/// The base class for all particles.
/// </summary>
public abstract partial class ParticleBase : ZeroVPoolableDrawable<ParticleSource> {
    //public ParticleType Type { get; protected init; }

    // public Orbit FatherOrbit { get; private set; } = null!;

    public ParticleBase() {
        this.Origin = Anchor.Centre;
        this.Anchor = Anchor.Centre;
        this.AutoSizeAxes = Axes.Both;
        // this.StartTimeBindable = new BindableDouble();
    }

    // public Boolean IsRecyclable { get; private set; }

    // public virtual void Recycle(Orbit fatherOrbit, Double startTime, Double? endTime = null) {
    //     this.FatherOrbit = fatherOrbit;
    //     // TODO: Create appropriate methods to make objects reusable.
    //     this.StartTimeBindable = new BindableDouble {
    //         Value = startTime,
    //     };
    //     this.EndTime = endTime ?? startTime;
    // }

    protected override void FreeAfterUse() {
        // Reset Particle
        this.Alpha = 1f;
        base.FreeAfterUse();
    }

    protected abstract TargetResult JudgeMain(in Double targetTime, in Double currentTime);

    public virtual TargetResult? JudgeEnter(in Double currentTime, in Boolean isNewTouch) {
        return null;
    }

    public virtual TargetResult? JudgeMove(in Double currentTime, in Vector2 delta) {
        return null;
    }

    public virtual TargetResult? JudgeLeave(in Double currentTime, in Boolean isTouchUp) {
        return null;
    }

    public virtual TargetResult? JudgeUpdate(in Double currentTime, in Boolean hasTouches) {
        return null;
    }
}
