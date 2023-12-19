using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Scoring;

namespace osu.Game.Rulesets.ZeroV.Judgements;

public class ZeroVJudgement : Judgement {
    protected override Double HealthIncreaseFor(HitResult result) {
        switch (result) {
            case HitResult.Meh:
                return -DEFAULT_MAX_HEALTH_INCREASE * 0.5;

            case HitResult.Ok:
                return -DEFAULT_MAX_HEALTH_INCREASE * 0.3;

            case HitResult.Good:
                return DEFAULT_MAX_HEALTH_INCREASE * 0.1;

            case HitResult.Great:
                return DEFAULT_MAX_HEALTH_INCREASE * 0.8;

            case HitResult.Perfect:
                return DEFAULT_MAX_HEALTH_INCREASE;

            default:
                return base.HealthIncreaseFor(result);
        }
    }
}
