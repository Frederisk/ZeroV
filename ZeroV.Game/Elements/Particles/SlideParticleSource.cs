using System;

using osuTK;

using ZeroV.Game.Scoring;

namespace ZeroV.Game.Elements.Particles;
public class SlideParticleSource(Double startTime, SlidingDirection direction) : ParticleSource {
    public override Double StartTime => startTime;
    public override Double EndTime => this.StartTime;

    public SlidingDirection Direction => direction;

    private TargetResult result;

    //private TargetResult judgeTouch(in JudgeInput input) {
    //    //this.result = Judgment.JudgeSlide(startTime, input.CurrentTime);
    //    //switch (input.IsTouchDown) {
    //    //    case null or false when this.result != TargetResult.Miss:
    //    //        this.result = TargetResult.None;
    //    //        break;
    //    //}

    //    //return this.result switch {
    //    //    TargetResult.Miss => TargetResult.Miss,
    //    //    _ => TargetResult.None
    //    //};
    //}
    public override TargetResult? JudgeMove(Double currentTime, Vector2 delta) {
        //if (input.TouchMoveDelta.HasValue) {
            //Vector2 delta = input.TouchMoveDelta.Value;
        var succeed = this.Direction switch {
            SlidingDirection.Left => delta.X < 0,
            SlidingDirection.Right => delta.X > 0,
            SlidingDirection.Up => delta.Y < 0,
            SlidingDirection.Down => delta.Y > 0,
            _ => throw new NotImplementedException($"Unknown SlidingDirection {this.Direction}")
        };

        return succeed ? this.result : TargetResult.Miss;
        //} else if (input.IsTouchLeave == true) {
        //    return TargetResult.Miss;
        //} else if (Judgment.JudgeSlide(startTime, input.CurrentTime) == TargetResult.Miss) {
        //    return TargetResult.Miss;
        //} else {
        //    return TargetResult.None;
        //}
    }
}
