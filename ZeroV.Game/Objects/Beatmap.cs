using System;

using ZeroV.Game.Elements;

namespace ZeroV.Game.Objects;

public class Beatmap {
    public ReadOnlyMemory<OrbitSource> Orbits { get; set; }
}
