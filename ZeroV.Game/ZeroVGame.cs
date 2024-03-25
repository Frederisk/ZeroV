using System;
using System.Diagnostics;

using osu.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Logging;
using osu.Framework.Platform;
using osu.Framework.Screens;

using osuTK.Graphics;

using ZeroV.Game.Elements;
using ZeroV.Game.Elements.Particles;
using ZeroV.Game.Objects;
using ZeroV.Game.Screens;

namespace ZeroV.Game;

public partial class ZeroVGame : ZeroVGameBase {

    /// <summary>
    /// The screen stack that manages the game screens.
    /// </summary>
    /// <remarks>
    /// This field will never be null after <see cref="LoadComplete"/> has been called.
    /// </remarks>
    private ScreenStack screenStack = null!;

    [BackgroundDependencyLoader]
    private void load() {
        // Add your top-level game components here.
        // A screen stack and sample screen has been provided for convenience, but you can replace it if you don't want to use screens.
        this.Child = this.screenStack = new ScreenStack { RelativeSizeAxes = Axes.Both };
    }

    protected override void LoadComplete() {
        base.LoadComplete();
        // For test, the beatmap instance will deserialize after beatmap selected.
        var beatmap = new Beatmap() {
            OrbitSources = new[] {
                new OrbitSource() {
                    KeyFrames = new[] {
                        new OrbitSource.KeyFrame() {
                             Time = 0,
                             XPosition = 0,
                             Width = 128,
                             Color = Color4.Azure
                        },
                        new OrbitSource.KeyFrame() {
                             Time = 4000,
                             XPosition = 0,
                             Width = 128,
                             Color = Color4.Red
                        },
                        new OrbitSource.KeyFrame() {
                             Time = 5000,
                             XPosition = 256,
                             Width = 256,
                             Color = Color4.Orange
                        },
                        new OrbitSource.KeyFrame() {
                             Time = 6000,
                             XPosition = 256,
                             Width = 256,
                             Color = Color4.Yellow
                        },
                        new OrbitSource.KeyFrame() {
                             Time = 7000,
                             XPosition = 0,
                             Width = 128,
                             Color = Color4.Green
                        },
                        new OrbitSource.KeyFrame() {
                             Time = 8000,
                             XPosition = -60,
                             Width = 256,
                             Color = Color4.Cyan
                        },
                        new OrbitSource.KeyFrame() {
                             Time = 9000,
                             XPosition = -30,
                             Width = 162,
                             Color = Color4.Blue
                        },
                        new OrbitSource.KeyFrame() {
                             Time = 10000,
                             XPosition = 0,
                             Width = 128,
                             Color = Color4.Purple
                        },
                        new OrbitSource.KeyFrame() {
                             Time = 16000,
                             XPosition = 0,
                             Width = 128,
                             Color = Color4.Purple
                        }
                    },
                    HitObjects = new TimeSourceWithHit[] {
                        new BlinkParticleSource(TimeSpan.FromSeconds(3).TotalMilliseconds),
                        new BlinkParticleSource(TimeSpan.FromSeconds(8).TotalMilliseconds),
                        new BlinkParticleSource(TimeSpan.FromSeconds(9).TotalMilliseconds),
                        new BlinkParticleSource(TimeSpan.FromSeconds(10).TotalMilliseconds),
                        // new PressParticleSource(TimeSpan.FromSeconds(10).TotalMilliseconds, TimeSpan.FromSeconds(11).TotalMilliseconds),
                        new SlideParticleSource(TimeSpan.FromSeconds(12).TotalMilliseconds, SlidingDirection.Left),
                        new SlideParticleSource(TimeSpan.FromSeconds(13).TotalMilliseconds, SlidingDirection.Up),
                        new SlideParticleSource(TimeSpan.FromSeconds(14).TotalMilliseconds, SlidingDirection.Right),
                        new SlideParticleSource(TimeSpan.FromSeconds(15).TotalMilliseconds, SlidingDirection.Down),
                        new StrokeParticleSource(TimeSpan.FromSeconds(16).TotalMilliseconds)
                    }
                }
            }
        };
        this.screenStack.Push(new GameplayScreen(beatmap));
    }
}
