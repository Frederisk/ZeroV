using System;

using ZeroV.Game.Objects;

namespace ZeroV.Game.Elements.Particles;

public class PressParticleSource(Double startTime, Double endTime) : TimeSourceWithHit {
    public override Double StartTime => startTime;
    public override Double EndTime => endTime;
}
