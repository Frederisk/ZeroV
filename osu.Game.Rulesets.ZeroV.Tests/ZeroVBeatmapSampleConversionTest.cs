using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

using osu.Game.Audio;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.ZeroV.Objects;
using osu.Game.Tests.Beatmaps;

namespace osu.Game.Rulesets.ZeroV.Tests;

[TestFixture]
public class ZeroVBeatmapSampleConversionTest : BeatmapConversionTest<ConvertMapping<SampleConvertValue>, SampleConvertValue> {
    protected override String ResourceAssembly => "osu.Game.Rulesets.ZeroV.Tests";

    // TODO: Create test bitmap file.
    // [TestCase("convert-samples")]
    // [TestCase("zero-v-samples")]
    // [TestCase("slider-convert-samples")]
    public void Test(String name) => base.Test(name);

    protected override IEnumerable<SampleConvertValue> CreateConvertValue(HitObject hitObject) {
        yield return new SampleConvertValue() {
            StartTime = hitObject.StartTime,
            EndTime = hitObject.GetEndTime(),
            Column = ((ZeroVHitObject)hitObject).Column,
            Samples = this.getSampleNames(hitObject.Samples)
            // NodeSamples = this.getNodeSampleNames((hitObject as BlinkParticle)?.NodeSamples)
        };
    }

    private IList<String> getSampleNames(IList<HitSampleInfo> hitSampleInfo)
        => hitSampleInfo.Select(sample => sample.LookupNames.First()).ToList();

    private IList<IList<String>> getNodeSampleNames(IList<IList<HitSampleInfo>> hitSampleInfo)
        => hitSampleInfo?.Select(this.getSampleNames).ToList();

    protected override Ruleset CreateRuleset() => new ZeroVRuleset();
}

public record struct SampleConvertValue {
    public Double StartTime;
    public Double EndTime;
    public Int32 Column;
    public IList<String> Samples;
    // public IList<IList<String>> NodeSamples;
}
