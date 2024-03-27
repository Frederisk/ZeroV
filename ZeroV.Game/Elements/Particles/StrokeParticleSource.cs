using System;

using osu.Framework.Input.Events;

using ZeroV.Game.Objects;
using ZeroV.Game.Scoring;

namespace ZeroV.Game.Elements.Particles;
public class StrokeParticleSource(Double startTime) : TimeSourceWithHit {
    public override Double StartTime => startTime;
    public override Double EndTime => this.StartTime;

    public override TargetResult Judge(in JudgeInput input) {
        TargetResult result = Judgment.JudgeStroke(startTime, input.CurrentTime);
        if(!input.IsTouchPress && result != TargetResult.Miss) {
            result = TargetResult.None;
        }

        return result;
    }
}
