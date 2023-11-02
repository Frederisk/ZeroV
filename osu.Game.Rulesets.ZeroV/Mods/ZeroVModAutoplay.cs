using System.Collections.Generic;

using osu.Game.Beatmaps;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.ZeroV.Replays;

namespace osu.Game.Rulesets.ZeroV.Mods;

public class ZeroVModAutoplay : ModAutoplay {
    public override ModReplayData CreateReplayData(IBeatmap beatmap, IReadOnlyList<Mod> mods)
        => new ModReplayData(new ZeroVAutoGenerator(beatmap).Generate(), new ModCreatedUser { Username = "sample" });
}
