using System;

namespace ZeroV.Game.Elements.Particles;

public class PressParticleSource : ParticleSource {

    public PressParticleSource(Double startTime, Double endTime) {
        this.StartTimeValue = startTime;
        this.EndTimeValue = endTime;
    }
}
