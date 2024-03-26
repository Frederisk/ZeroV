using System;

using osu.Framework.Input.Events;

using ZeroV.Game.Objects;
using ZeroV.Game.Scoring;

namespace ZeroV.Game.Elements.Particles;

public class PressParticleSource(Double startTime, Double endTime) : TimeSourceWithHit {
    public override Double StartTime => startTime;
    public override Double EndTime => endTime;

    public override TargetResult Judge(Orbit orbit, Double currTime, TouchEvent? touchEvent) {
        return TargetResult.None;
    }
}
