using System;
using System.Collections.Generic;

using ZeroV.Game.Elements;

namespace ZeroV.Game.Objects;

public class Beatmap {
    public required List<OrbitSource> OrbitSources { get; set; }

    public required Double Offset { get; set; }
}
