using System;

using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Objects;

namespace osu.Game.Rulesets.ZeroV.Objects;

public class ZeroVHitObject : HitObject {

    /// <summary>
    /// Range = [-1,1]
    /// </summary>
    public Int32 Lane;

    public override Judgement CreateJudgement() => new();
}
