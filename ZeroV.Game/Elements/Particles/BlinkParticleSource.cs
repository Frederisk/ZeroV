using System;

using ZeroV.Game.Scoring;

namespace ZeroV.Game.Elements.Particles;

public class BlinkParticleSource(Double startTime) : ParticleSource {
    public override Double StartTime => startTime;
    public override Double EndTime => this.StartTime;

    public override TargetResult JudgeEnter(Double currentTime, Boolean isNewTouch) {
        // base.JudgeEnter(currentTime, isNewTouch); // just return null
        if (isNewTouch) {
            TargetResult result = Judgment.JudgeBlink(this.StartTime, currentTime);
            return result;
        }
        return TargetResult.None;
    }

    public override TargetResult JudgeUpdate(Double currentTime) {
        // base.JudgeUpdate(currentTime); // just return null
        TargetResult result = Judgment.JudgeBlink(this.StartTime, currentTime);
        if (result is TargetResult.Miss) {
            return result;
        }
        return TargetResult.None;
    }
}
