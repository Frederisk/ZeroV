using System;

using osu.Framework.Input.Events;

using ZeroV.Game.Objects;
using ZeroV.Game.Scoring;

namespace ZeroV.Game.Elements.Particles;
public class SlideParticleSource(Double startTime, SlidingDirection direction) : TimeSourceWithHit {
    public override Double StartTime => startTime;
    public override Double EndTime => this.StartTime;

    public SlidingDirection Direction => direction;

    public override TargetResult Judge(Orbit orbit, Double currTime, TouchEvent? touchEvent) {
        return TargetResult.None;
    }
}
