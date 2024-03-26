using System;

using osu.Framework.Input;

using osuTK;

using ZeroV.Game.Elements;
using ZeroV.Game.Scoring;

namespace ZeroV.Game.Objects;

public class JudgeInput {
    public required Double CurrentTime { get; set; }
    public TouchSource? TouchSource { get; set; } 
    public Boolean? IsTouchDown { get; set; }
    public Boolean IsTouchPress { get; set; }
    public Vector2? TouchMoveDelta { get; set; }
}

public abstract class TimeSourceWithHit : TimeSource {

    public abstract TargetResult Judge(JudgeInput input);
}
