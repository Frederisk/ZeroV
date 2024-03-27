using ZeroV.Game.Elements;
using ZeroV.Game.Scoring;

namespace ZeroV.Game.Objects;

public abstract class TimeSourceWithHit : TimeSource {

    public abstract TargetResult Judge(in JudgeInput input);
}
