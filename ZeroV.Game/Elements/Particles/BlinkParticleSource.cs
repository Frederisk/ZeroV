using System;

namespace ZeroV.Game.Elements.Particles;

public class BlinkParticleSource : ParticleSource {

    public BlinkParticleSource(Double startTime) {
        this.StartTimeValue = this.EndTimeValue = startTime;
    }
}
