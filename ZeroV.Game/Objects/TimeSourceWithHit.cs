using ZeroV.Game.Scoring;

namespace ZeroV.Game.Objects;

public abstract class TimeSourceWithHit : TimeSource {
    public abstract HitWindows HitWindows { get; }
}
