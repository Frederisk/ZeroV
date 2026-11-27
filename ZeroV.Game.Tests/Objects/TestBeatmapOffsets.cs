using System;

using NUnit.Framework;

using ZeroV.Game.Elements.Orbits;
using ZeroV.Game.Elements.Particles;
using ZeroV.Game.Objects;

namespace ZeroV.Game.Tests.Objects;

[TestFixture]
internal class TestBeatmapOffsets {

    [Test]
    public void TestPositiveOffsetMovesChartEventsLater() {
        BlinkParticleSource particle = new(1000);
        Beatmap beatmap = createBeatmap(500, particle);

        beatmap.ApplyOffset(125);

        Assert.Multiple(() => {
            Assert.That(beatmap.OrbitSources[0].KeyFrames[0].Time, Is.EqualTo(625));
            Assert.That(particle.StartTime, Is.EqualTo(1125));
            Assert.That(particle.EndTime, Is.EqualTo(1125));
        });
    }

    [Test]
    public void TestFileAndGlobalOffsetsComposeAsChartDelay() {
        BlinkParticleSource particle = new(1000);
        Beatmap beatmap = createBeatmap(500, particle);
        Double fileOffset = TimeSpan.FromMilliseconds(-300).TotalMilliseconds;
        Double globalSoundOffset = 50;

        beatmap.ApplyOffset(fileOffset + globalSoundOffset);

        Assert.Multiple(() => {
            Assert.That(beatmap.OrbitSources[0].KeyFrames[0].Time, Is.EqualTo(250));
            Assert.That(particle.StartTime, Is.EqualTo(750));
            Assert.That(particle.EndTime, Is.EqualTo(750));
        });
    }

    private static Beatmap createBeatmap(Double keyFrameTime, ParticleSource particle) =>
        new() {
            OrbitSources = [
                new OrbitSource {
                    KeyFrames = [
                        new OrbitSource.KeyFrame {
                            Time = keyFrameTime,
                        },
                    ],
                    HitObjects = [particle],
                },
            ],
        };
}
