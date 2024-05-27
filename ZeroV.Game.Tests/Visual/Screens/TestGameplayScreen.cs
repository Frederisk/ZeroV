using System;

using NUnit.Framework;

using osu.Framework.Graphics;
using osu.Framework.Screens;

using ZeroV.Game.Elements;
using ZeroV.Game.Elements.Particles;
using ZeroV.Game.Objects;
using ZeroV.Game.Screens;

namespace ZeroV.Game.Tests.Visual.Screens;

[TestFixture]
public partial class TestGameplayScreen : ZeroVTestScene {

    public TestGameplayScreen() {
        // For test, the beatmap instance will deserialize after beatmap selected.
        var beatmap = new Beatmap() {
            OrbitSources = new[] {
                new OrbitSource() {
                    KeyFrames = new[] {
                        new OrbitSource.KeyFrame() {
                             Time = 0,
                             XPosition = 0,
                             Width = 128,
                             Colour = Colour4.Green,
                        },
                        new OrbitSource.KeyFrame() {
                             Time = 10000,
                             XPosition = 0,
                             Width = 128,
                             Colour = Colour4.Green,
                        }
                    },
                    HitObjects = Array.Empty<ParticleSource>()
                }
            }
        };
        this.Add(new ScreenStack(new GameplayScreen(beatmap) { RelativeSizeAxes = Axes.Both }) { RelativeSizeAxes = Axes.Both });
    }
}
