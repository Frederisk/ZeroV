using System;

using ZeroV.Game.Objects;
using ZeroV.Game.Scoring;

namespace ZeroV.Game.Elements.Particles;

public class PressParticleSource(Double startTime, Double endTime) : TimeSourceWithHit {
    public override Double StartTime => startTime;
    public override Double EndTime => endTime;

    private TargetResult result;
    public override TargetResult Judge(in JudgeInput input) {
        if (this.result == TargetResult.None) {
            this.result = Judgment.JudgeBlink(startTime, input.CurrentTime);
            switch (input.IsTouchDown) {
                case null or false when this.result != TargetResult.Miss:
                    this.result = TargetResult.None;
                    break;
            }

            return this.result switch {
                TargetResult.Miss => TargetResult.Miss,
                _ => TargetResult.None
            };         
        } else {
            TargetResult result = Judgment.JudgeBlink(endTime, input.CurrentTime);

            switch (result) {
                case TargetResult.None when !input.IsTouchPress: return TargetResult.Miss;
                case TargetResult.None: return TargetResult.None;
                case TargetResult.Miss:
                case var _ when !input.IsTouchPress:
                    return this.result;
                default: return TargetResult.None;
            }
        }
    }
}
