using System;

namespace ZeroV.Game.Elements.Particles;

public class SlideParticleSource(Double startTime, SlidingDirection direction) : ParticleSource {
    public override Double StartTime => startTime;
    public override Double EndTime => this.StartTime;

    public SlidingDirection Direction => direction;
}
