using System;

using osu.Framework.Input;

using osuTK;

namespace ZeroV.Game.Objects;

public readonly ref struct JudgeInput {
    public required Double CurrentTime { get; init; }
    public TouchSource? TouchSource { get; init; } 
    public Boolean? IsTouchDown { get; init; }
    public Boolean IsTouchPress { get; init; }
    public Vector2? TouchMoveDelta { get; init; }
}
