using System;

namespace ZeroV.Game.Elements.Particles;

public class StrokeParticleSource : ParticleSource {

    public StrokeParticleSource(Double startTime) {
        this.StartTimeValue = this.EndTimeValue = startTime;
    }
}
