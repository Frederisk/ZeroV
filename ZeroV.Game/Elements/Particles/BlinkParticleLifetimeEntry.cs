using System;

using ZeroV.Game.Graphics;

namespace ZeroV.Game.Elements.Particles;

public class BlinkParticleLifetimeEntry : ZeroVLifetimeEntry<BlinkParticleSource, BlinkParticle> {
    public BlinkParticleLifetimeEntry(BlinkParticleSource source) : base(source) {
        this.LifetimeStart = source.StartTime - TimeSpan.FromSeconds(10).TotalMilliseconds;
        this.LifetimeEnd = source.EndTime + TimeSpan.FromSeconds(5).TotalMilliseconds;
    }
}
