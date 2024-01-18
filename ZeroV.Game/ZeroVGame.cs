using System;
using System.Diagnostics;

using osu.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Logging;
using osu.Framework.Platform;
using osu.Framework.Screens;

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

    //public ZeroVGame() { this.Storage = this.Host.Storage; }

    [BackgroundDependencyLoader]
    private void load() {
        // Add your top-level game components here.
        // A screen stack and sample screen has been provided for convenience, but you can replace it if you don't want to use screens.
        this.Child = this.screenStack = new ScreenStack { RelativeSizeAxes = Axes.Both };
    }

    protected override void LoadComplete() {
        base.LoadComplete();
        this.screenStack.Push(new GameplayScreen());
    }
}
