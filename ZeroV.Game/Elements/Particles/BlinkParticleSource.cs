using System;

using ZeroV.Game.Scoring;

namespace ZeroV.Game.Elements.Particles;

public class BlinkParticleSource(Double startTime) : ParticleSource {
    public override Double StartTime => startTime;
    public override Double EndTime => this.StartTime;
}
