using System;

namespace ZeroV.Game.Data.Particles;

public class PressNote : NoteBase {
    public required TimeSpan EndTime { get; init; }
}
