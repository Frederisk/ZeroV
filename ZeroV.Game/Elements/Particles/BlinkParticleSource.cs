using System;

using ZeroV.Game.Objects;
using ZeroV.Game.Scoring;

namespace ZeroV.Game.Elements.Particles;

public class BlinkParticleSource(Double startTime) : TimeSourceWithHit {
    public override Double StartTime => startTime;
    public override Double EndTime => this.StartTime;

    public override TargetResult Judge(JudgeInput input) {
        TargetResult result = Judgment.JudgeBlink(startTime, input.CurrentTime);
        switch (input.IsTouchDown) {
            case null or false when result != TargetResult.Miss:
                result = TargetResult.None;
                break;
        }

        return result;
    }
}
