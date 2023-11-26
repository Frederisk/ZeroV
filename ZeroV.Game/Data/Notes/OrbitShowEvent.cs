using System;

namespace ZeroV.Game.Data.Particles;

public class OrbitShowEvent : OrbitEventNoteBase {
    public required Double Width { get; init; }
    public required Double Position { get; init; }
    public required OrbitShowAndExitAnimation Animation { get; init; }
}
