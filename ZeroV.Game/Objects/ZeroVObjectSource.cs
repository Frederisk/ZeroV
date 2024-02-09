using System;

namespace ZeroV.Game.Objects;

public abstract class ZeroVObjectSource {
    public abstract Double StartTime { get; }
    public abstract Double EndTime { get; }
    public Double Duration => this.EndTime - this.StartTime;
}
