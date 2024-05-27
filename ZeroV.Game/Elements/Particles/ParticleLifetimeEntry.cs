using ZeroV.Game.Graphics;

namespace ZeroV.Game.Elements.Particles;

public class ParticleLifetimeEntry : ZeroVLifetimeEntry<ParticleSource, ParticleBase> {

    public ParticleLifetimeEntry(ParticleSource source) : base(source) {
        this.LifetimeStart = source.StartTime;
        this.LifetimeEnd = source.EndTime;
    }
}
