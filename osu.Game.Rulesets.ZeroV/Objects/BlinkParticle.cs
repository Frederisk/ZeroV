using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.ZeroV.Judgements;

namespace osu.Game.Rulesets.ZeroV.Objects;

public class BlinkParticle : ZeroVHitObject {
    public override Judgement CreateJudgement() => new ZeroVJudgement();
}
