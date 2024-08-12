using System;
using System.IO;

using NUnit.Framework;

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Screens;
using osu.Framework.Testing;

using ZeroV.Game.Elements.Orbits;
using ZeroV.Game.Elements.Particles;
using ZeroV.Game.Objects;
using ZeroV.Game.Screens.Gameplay;

namespace ZeroV.Game.Tests.Visual.Screens;

[TestFixture]
public partial class TestSceneGameplayScreen : ZeroVTestScene {

    [Cached]
    private ScreenStack screenStack;

    public TestSceneGameplayScreen() {
        this.screenStack = new ScreenStack() { RelativeSizeAxes = Axes.Both };
        this.Add(this.screenStack);
    }

    [SetUpSteps]
    public void SetUpSteps() {
        this.AddStep("create screen", this.createScreen);
    }

    private void createScreen() {
        // For test, the beatmap instance will deserialize after beatmap selected.
        var beatmap = new Beatmap() {
            OrbitSources = [
                new OrbitSource() {
                    KeyFrames = [
                        new OrbitSource.KeyFrame() {
                             Time = 0,
                             XPosition = 0,
                             Width = 128,
                             Colour = Colour4.Azure
                        },
                        new OrbitSource.KeyFrame() {
                             Time = 4000,
                             XPosition = 0,
                             Width = 128,
                             Colour = Colour4.Red
                        },
                        new OrbitSource.KeyFrame() {
                             Time = 5000,
                             XPosition = 256,
                             Width = 256,
                             Colour = Colour4.Orange
                        },
                        new OrbitSource.KeyFrame() {
                             Time = 6000,
                             XPosition = 256,
                             Width = 256,
                             Colour = Colour4.Yellow
                        },
                        new OrbitSource.KeyFrame() {
                             Time = 7000,
                             XPosition = -100,
                             Width = 128,
                             Colour = Colour4.Green
                        },
                        new OrbitSource.KeyFrame() {
                             Time = 8000,
                             XPosition = -90,
                             Width = 256,
                             Colour = Colour4.Cyan
                        },
                        new OrbitSource.KeyFrame() {
                             Time = 9000,
                             XPosition = -300,
                             Width = 162,
                             Colour = Colour4.Blue
                        },
                        new OrbitSource.KeyFrame() {
                             Time = 10000,
                             XPosition = -200,
                             Width = 128,
                             Colour = Colour4.Purple
                        },
                        new OrbitSource.KeyFrame() {
                             Time = 17000,
                             XPosition = -300,
                             Width = 128,
                             Colour = Colour4.Purple
                        },
                    ],
                    HitObjects = [
                        new BlinkParticleSource(TimeSpan.FromSeconds(3).TotalMilliseconds),
                        new BlinkParticleSource(TimeSpan.FromSeconds(8).TotalMilliseconds),
                        new BlinkParticleSource(TimeSpan.FromSeconds(9).TotalMilliseconds),
                        new PressParticleSource(TimeSpan.FromSeconds(10).TotalMilliseconds, TimeSpan.FromSeconds(11).TotalMilliseconds),
                        new SlideParticleSource(TimeSpan.FromSeconds(12).TotalMilliseconds, SlidingDirection.Left),
                        new SlideParticleSource(TimeSpan.FromSeconds(13).TotalMilliseconds, SlidingDirection.Up),
                        new SlideParticleSource(TimeSpan.FromSeconds(14).TotalMilliseconds, SlidingDirection.Right),
                        new SlideParticleSource(TimeSpan.FromSeconds(15).TotalMilliseconds, SlidingDirection.Down),
                        new StrokeParticleSource(TimeSpan.FromSeconds(16).TotalMilliseconds),
                    ]
                },
                new OrbitSource() {
                    KeyFrames = [
                        new OrbitSource.KeyFrame() {
                             Time = 0,
                             XPosition = 0,
                             Width = 1280,
                             Colour = Colour4.Azure
                        },
                        new OrbitSource.KeyFrame() {
                             Time = 17000,
                             XPosition = 0,
                             Width = 1280,
                             Colour = Colour4.Purple
                        },
                    ],
                    HitObjects = [
                        new BlinkParticleSource(TimeSpan.FromSeconds(3).TotalMilliseconds),
                        new BlinkParticleSource(TimeSpan.FromSeconds(8).TotalMilliseconds),
                        new BlinkParticleSource(TimeSpan.FromSeconds(9).TotalMilliseconds),
                        new PressParticleSource(TimeSpan.FromSeconds(10).TotalMilliseconds, TimeSpan.FromSeconds(11).TotalMilliseconds),
                        new SlideParticleSource(TimeSpan.FromSeconds(12).TotalMilliseconds, SlidingDirection.Left),
                        new SlideParticleSource(TimeSpan.FromSeconds(13).TotalMilliseconds, SlidingDirection.Up),
                        new SlideParticleSource(TimeSpan.FromSeconds(14).TotalMilliseconds, SlidingDirection.Right),
                        new SlideParticleSource(TimeSpan.FromSeconds(15).TotalMilliseconds, SlidingDirection.Down),
                        new StrokeParticleSource(TimeSpan.FromSeconds(16).TotalMilliseconds),
                    ],
                },
            ],
            //Offset = 0,
        };
        FileInfo file = new FileInfo("./Resources/Schema/Track.txt");
        //this.screenStack.Push(new GameplayScreen(beatmap, file) { RelativeSizeAxes = Axes.Both });
        this.screenStack.Push(new GameLoader(() => {
            return new GameplayScreen(beatmap, null!) { RelativeSizeAxes = Axes.Both };
        }));
    }
}
