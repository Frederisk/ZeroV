using ZeroV.Game.Scoring;

namespace ZeroV.Game.Objects;

public abstract class ZeroVObjectSourceWithHit : ZeroVObjectSource {
    public abstract HitWindows HitWindows { get; }
}
