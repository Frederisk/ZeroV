using System.Collections.Generic;

using osu.Framework.Allocation;
using osu.Framework.Input;
using osu.Game.Beatmaps;
using osu.Game.Input.Handlers;
using osu.Game.Replays;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.UI;
using osu.Game.Rulesets.UI.Scrolling;
using osu.Game.Rulesets.ZeroV.Objects;
using osu.Game.Rulesets.ZeroV.Objects.Drawables;
using osu.Game.Rulesets.ZeroV.Replays;

namespace osu.Game.Rulesets.ZeroV.UI;

[Cached]
public partial class DrawableZeroVRuleset : DrawableScrollingRuleset<ZeroVHitObject> {
    public DrawableZeroVRuleset(ZeroVRuleset ruleset, IBeatmap beatmap, IReadOnlyList<Mod> mods = null)
        : base(ruleset, beatmap, mods) {
        this.Direction.Value = ScrollingDirection.Left;
        this.TimeRange.Value = 6000;
    }

    protected override Playfield CreatePlayfield() => new ZeroVPlayfield();

    protected override ReplayInputHandler CreateReplayInputHandler(Replay replay) => new ZeroVFramedReplayInputHandler(replay);

    public override DrawableHitObject<ZeroVHitObject> CreateDrawableRepresentation(ZeroVHitObject h) => new DrawableZeroVHitObject(h);

    protected override PassThroughInputManager CreateInputManager() => new ZeroVInputManager(this.Ruleset?.RulesetInfo);
}
