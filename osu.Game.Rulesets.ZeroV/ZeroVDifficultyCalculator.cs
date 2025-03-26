using System;
using System.Collections.Generic;
using System.Linq;

using osu.Game.Beatmaps;
using osu.Game.Rulesets.Difficulty;
using osu.Game.Rulesets.Difficulty.Preprocessing;
using osu.Game.Rulesets.Difficulty.Skills;
using osu.Game.Rulesets.Mods;

namespace osu.Game.Rulesets.ZeroV;

public class ZeroVDifficultyCalculator : DifficultyCalculator {
    public ZeroVDifficultyCalculator(IRulesetInfo ruleset, IWorkingBeatmap beatmap)
        : base(ruleset, beatmap) {
    }

    protected override DifficultyAttributes CreateDifficultyAttributes(IBeatmap beatmap, Mod[] mods, Skill[] skills, Double clockRate) {
        return new DifficultyAttributes(mods, 0);
    }

    protected override IEnumerable<DifficultyHitObject> CreateDifficultyHitObjects(IBeatmap beatmap, Double clockRate) => Enumerable.Empty<DifficultyHitObject>();

    protected override Skill[] CreateSkills(IBeatmap beatmap, Mod[] mods, Double clockRate) => Array.Empty<Skill>();
}
