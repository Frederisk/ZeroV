using System;

using ZeroV.Game.Objects;

namespace ZeroV.Game.Elements.Particles;

public abstract class ParticleSource : TimeSource {
    protected Double StartTimeValue;
    protected Double EndTimeValue;
    public sealed override Double StartTime => this.StartTimeValue;
    public sealed override Double EndTime => this.EndTimeValue;

    public void ApplyOffset(Double offset) {
        this.StartTimeValue += offset;
        this.EndTimeValue += offset;
    }
}
