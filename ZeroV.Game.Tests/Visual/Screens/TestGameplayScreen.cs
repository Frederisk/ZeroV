using System;

using NUnit.Framework;

using osu.Framework.Graphics;
using osu.Framework.Screens;

using osuTK.Graphics;

using ZeroV.Game.Elements;
using ZeroV.Game.Objects;
using ZeroV.Game.Screens;

namespace ZeroV.Game.Tests.Visual.Screens;

[TestFixture]
public partial class TestGameplayScreen : ZeroVTestScene {

    public TestGameplayScreen() {
        // For test, the beatmap instance will deserialize after beatmap selected.
        var beatmap = new ZeroVBeatmap() {
            Orbits = new[] {
                new Orbit() {
                    KeyFrames = new[] {
                        new Orbit.KeyFrame() {
                             Time = 0,
                             Position = 0,
                             Width = 128,
                             Color = Color4.Green
                        },
                        new Orbit.KeyFrame() {
                             Time = 10000,
                             Position = 0,
                             Width = 128,
                             Color = Color4.Green
                        }
                    },
                    HitObjects = Array.Empty<ZeroVHitObject>()
                }
            }
        };
        this.Add(new ScreenStack(new GameplayScreen(beatmap) { RelativeSizeAxes = Axes.Both }) { RelativeSizeAxes = Axes.Both });
    }
}
