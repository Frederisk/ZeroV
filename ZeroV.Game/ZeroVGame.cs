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
            Orbits = new[] {
                new OrbitSource() {
                    KeyFrames = new[] {
                        new OrbitSource.KeyFrame() {
                             Time = 0,
                             Position = 0,
                             Width = 128,
                             Color = Color4.Green
                        },
                        new OrbitSource.KeyFrame() {
                             Time = 4000,
                             Position = 0,
                             Width = 128,
                             Color = Color4.Green
                        },
                        new OrbitSource.KeyFrame() {
                             Time = 5000,
                             Position = 256,
                             Width = 256,
                             Color = Color4.Green
                        },
                        new OrbitSource.KeyFrame() {
                             Time = 6000,
                             Position = 256,
                             Width = 256,
                             Color = Color4.Green
                        },
                        new OrbitSource.KeyFrame() {
                             Time = 7000,
                             Position = 0,
                             Width = 128,
                             Color = Color4.Green
                        },
                        new OrbitSource.KeyFrame() {
                             Time = 8000,
                             Position = -60,
                             Width = 256,
                             Color = Color4.Green
                        },
                        new OrbitSource.KeyFrame() {
                             Time = 9000,
                             Position = -30,
                             Width = 162,
                             Color = Color4.Green
                        },
                        new OrbitSource.KeyFrame() {
                             Time = 10000,
                             Position = 0,
                             Width = 128,
                             Color = Color4.Green
                        }
                    },
                    HitObjects = Array.Empty<ZeroVObjectSourceWithHit>()
                }
            }
        };
        this.screenStack.Push(new GameplayScreen(beatmap));
    }
}
