using System;

using osu.Framework.Bindables;
using osu.Framework.Graphics;

using ZeroV.Game.Graphics;
using ZeroV.Game.Objects;

namespace ZeroV.Game.Elements.Particles;

/// <summary>
/// The base class for all particles.
/// </summary>
public abstract partial class ParticleBase : ZeroVPoolableDrawable<TimeSourceWithHit> {

    public ParticleType Type { get; protected init; }

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
}

public enum ParticleType {
    Unknown,
    Blink,
    Press,
    Slide,
    Stroke,
}
