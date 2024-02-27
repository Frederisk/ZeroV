using System;

using ZeroV.Game.Objects;

namespace ZeroV.Game.Elements.Particles;

public class BlinkParticleSource(Double startTime) : TimeSourceWithHit {
    public override Double StartTime { get; } = startTime;
    public override Double EndTime => this.StartTime;
}
