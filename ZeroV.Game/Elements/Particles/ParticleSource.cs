using System;

using osuTK;

using ZeroV.Game.Objects;
using ZeroV.Game.Scoring;

namespace ZeroV.Game.Elements.Particles;

public abstract class ParticleSource : TimeSource {
    public virtual TargetResult? JudgeEnter(Double currentTime, Boolean isTouchDown) {
        return null;
    }

    public virtual TargetResult? JudgeMove(Double currentTime, Vector2 delta) {
        return null;
    }

    public virtual TargetResult? JudgeLeave(Double currentTime, Boolean isTouchUp) {
        return null;
    }

    public virtual TargetResult? JudgeUpdate(Double currentTime) {
        return null;
    }
}
