using System;

namespace ZeroV.Game.Elements.Particles;

public class SlideParticleSource : ParticleSource {

    public SlideParticleSource(Double startTime, SlidingDirection direction) {
        this.StartTimeValue = this.EndTimeValue = startTime;
        this.Direction = direction;
    }

    public SlidingDirection Direction { get; }
}
