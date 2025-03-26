using System;
using System.Collections.Generic;

using osu.Framework.Graphics;
using osu.Framework.Input.Bindings;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Difficulty;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.UI;
using osu.Game.Rulesets.ZeroV.Beatmaps;
using osu.Game.Rulesets.ZeroV.Mods;
using osu.Game.Rulesets.ZeroV.UI;

namespace osu.Game.Rulesets.ZeroV;

public class ZeroVRuleset : Ruleset {
    public override String Description => "gather the osu!coins";

    public override DrawableRuleset CreateDrawableRulesetWith(IBeatmap beatmap, IReadOnlyList<Mod> mods = null) => new DrawableZeroVRuleset(this, beatmap, mods);

    public override IBeatmapConverter CreateBeatmapConverter(IBeatmap beatmap) => new ZeroVBeatmapConverter(beatmap, this);

    public override DifficultyCalculator CreateDifficultyCalculator(IWorkingBeatmap beatmap) => new ZeroVDifficultyCalculator(this.RulesetInfo, beatmap);

    public override IEnumerable<Mod> GetModsFor(ModType type) {
        switch (type) {
            case ModType.Automation:
                return new[] { new ZeroVModAutoplay() };

            default:
                return Array.Empty<Mod>();
        }
    }

    public override String ShortName => "zerov";

    public override IEnumerable<KeyBinding> GetDefaultKeyBindings(Int32 variant = 0) => new[]
    {
        new KeyBinding(InputKey.W, ZeroVAction.MoveUp),
        new KeyBinding(InputKey.S, ZeroVAction.MoveDown),
    };

    public override Drawable CreateIcon() => new ZeroVRulesetIcon(this);

    // Leave this line intact. It will bake the correct version into the ruleset on each build/release.
    public override String RulesetAPIVersionSupported => CURRENT_RULESET_API_VERSION;
}
