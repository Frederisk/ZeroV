using System;

using ZeroV.Game.Objects;

namespace ZeroV.Game.Elements.Particles;
public class StrokeParticleSource(Double startTime) : TimeSourceWithHit {
    public override Double StartTime => startTime;
    public override Double EndTime => this.StartTime;
}
