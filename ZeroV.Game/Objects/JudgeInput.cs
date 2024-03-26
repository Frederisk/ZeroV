using System;

using osu.Framework.Input;

using osuTK;

namespace ZeroV.Game.Objects;

public class JudgeInput {
    public required Double CurrentTime { get; set; }
    public TouchSource? TouchSource { get; set; } 
    public Boolean? IsTouchDown { get; set; }
    public Boolean IsTouchPress { get; set; }
    public Vector2? TouchMoveDelta { get; set; }
}
