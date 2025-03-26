using System;

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Screens;

using ZeroV.Game.Screens;

namespace ZeroV.Game;

public partial class ZeroVGame : ZeroVGameBase {

    /// <summary>
    /// The screen stack that manages the game screens.
    /// </summary>
    /// <remarks>
    /// This field will never be <see langword="null"/> after <see cref="LoadComplete"/> has been called.
    /// </remarks>
    private ScreenStack screenStack = null!;

    [BackgroundDependencyLoader]
    private void load() {
        // Add your top-level game components here.
        // A screen stack and sample screen has been provided for convenience, but you can replace it if you don't want to use screens.
        this.screenStack = new() { RelativeSizeAxes = Axes.Both };
        this.Child = this.screenStack;
    }

    protected override void LoadComplete() {
        base.LoadComplete();
        // For test, the beatmap instance will deserialize after beatmap selected.
        this.screenStack.Push(new IntroScreen());
    }
}
