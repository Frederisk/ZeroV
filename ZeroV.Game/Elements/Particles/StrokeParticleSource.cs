using System;

namespace ZeroV.Game.Elements.Particles;

public class StrokeParticleSource(Double startTime) : ParticleSource {
    public override Double StartTime => startTime;
    public override Double EndTime => this.StartTime;
}
