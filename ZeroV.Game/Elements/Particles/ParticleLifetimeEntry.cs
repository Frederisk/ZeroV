using System;

using ZeroV.Game.Graphics;
using ZeroV.Game.Objects;

namespace ZeroV.Game.Elements.Particles;

public class ParticleLifetimeEntry : ZeroVLifetimeEntry<TimeSourceWithHit, ParticleBase> {
    public ParticleLifetimeEntry(TimeSourceWithHit source) : base(source) {
        this.LifetimeStart = source.StartTime;
        this.LifetimeEnd = source.EndTime;
    }
}
