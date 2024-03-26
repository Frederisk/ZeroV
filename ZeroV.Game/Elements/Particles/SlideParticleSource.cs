using System;

using osuTK;

using ZeroV.Game.Objects;
using ZeroV.Game.Scoring;

namespace ZeroV.Game.Elements.Particles;
public class SlideParticleSource(Double startTime, SlidingDirection direction) : TimeSourceWithHit {
    public override Double StartTime => startTime;
    public override Double EndTime => this.StartTime;

    public SlidingDirection Direction => direction;

    private TargetResult result;
    public override TargetResult Judge(JudgeInput input) {
        if (this.result == TargetResult.None) {
            return this.judgeTouch(input);
        } else {
            return this.judgeMove(input);
        }
    }

    private TargetResult judgeTouch(JudgeInput input) {
        this.result = Judgment.JudgeSlide(startTime, input.CurrentTime);
        switch (input.IsTouchDown) {
            case null or false when this.result != TargetResult.Miss:
                this.result = TargetResult.None;
                break;
        }

        return this.result switch {
            TargetResult.Miss => TargetResult.Miss,
            _ => TargetResult.None
        };
    }
    private TargetResult judgeMove(JudgeInput input) {
        if (input.TouchMoveDelta.HasValue) {
            Vector2 delta = input.TouchMoveDelta.Value;
            var succeed = this.Direction switch {
                SlidingDirection.Left => delta.X < 0,
                SlidingDirection.Right => delta.X > 0,
                SlidingDirection.Up => delta.Y < 0,
                SlidingDirection.Down => delta.Y > 0,
                _ => throw new NotImplementedException($"Unknown SlidingDirection {this.Direction}")
            };

            return succeed ? this.result : TargetResult.Miss;
        } else if (!input.IsTouchPress) {
            return TargetResult.Miss;
        } else if (Judgment.JudgeSlide(startTime, input.CurrentTime) == TargetResult.Miss) {
            return TargetResult.Miss;
        } else {
            return TargetResult.None;
        }
    }
}
