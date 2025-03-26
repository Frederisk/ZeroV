using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using osu.Game.Beatmaps;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Objects.Types;
using osu.Game.Rulesets.ZeroV.Objects;
using osu.Game.Rulesets.ZeroV.UI;

using osuTK;

namespace osu.Game.Rulesets.ZeroV.Beatmaps;

public class ZeroVBeatmapConverter : BeatmapConverter<ZeroVHitObject> {
    private readonly Single minPosition;
    private readonly Single maxPosition;

    public ZeroVBeatmapConverter(IBeatmap beatmap, Ruleset ruleset)
        : base(beatmap, ruleset) {
        if (beatmap.HitObjects.Any()) {
            this.minPosition = beatmap.HitObjects.Min(this.getUsablePosition);
            this.maxPosition = beatmap.HitObjects.Max(this.getUsablePosition);
        }
    }

    public override Boolean CanConvert() => this.Beatmap.HitObjects.All(h => h is IHasXPosition and IHasYPosition);

    protected override IEnumerable<ZeroVHitObject> ConvertHitObject(HitObject original, IBeatmap beatmap, CancellationToken cancellationToken) {
        yield return new BlinkParticle {
            Samples = original.Samples,
            StartTime = original.StartTime,
            Lane = this.getLane(original)
        };
    }

    private Int32 getLane(HitObject hitObject) => (Int32)MathHelper.Clamp(
        (this.getUsablePosition(hitObject) - this.minPosition) / (this.maxPosition - this.minPosition) * ZeroVPlayfield.LANE_COUNT, 0, ZeroVPlayfield.LANE_COUNT - 1);

    private Single getUsablePosition(HitObject h) => (h as IHasYPosition)?.Y ?? ((IHasXPosition)h).X;
}
