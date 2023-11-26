using System;

namespace ZeroV.Game.Data.Particles;

public abstract class NoteBase {
    public required TimeSpan StartTime { get; init; }
}
