using System;

using osu.Framework.Input.Events;

using ZeroV.Game.Objects;
using ZeroV.Game.Scoring;

namespace ZeroV.Game.Elements.Particles;

public class BlinkParticleSource(Double startTime) : TimeSourceWithHit {
    public override Double StartTime => startTime;
    public override Double EndTime => this.StartTime;

    public override TargetResult Judge(Orbit orbit, Double currTime, TouchEvent? touchEvent) {
        if(touchEvent is null or TouchDownEvent) {
            return Judgment.JudgeBlink(startTime, currTime);
        }

        return TargetResult.None;
    }
}
