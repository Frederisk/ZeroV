using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Screens;

using ZeroV.Game.Screens;

namespace ZeroV.Game;

public partial class ZeroVGame : ZeroVGameBase {
    private ScreenStack? screenStack;
    private DependencyContainer? dependencies;

    [BackgroundDependencyLoader]
    private void load() {
        // Add your top-level game components here.
        // A screen stack and sample screen has been provided for convenience, but you can replace it if you don't want to use screens.
        this.Child = this.screenStack = new ScreenStack { RelativeSizeAxes = Axes.Both };

        this.dependencies!.Cache(new ZeroVGameSettings());
    }

    protected override void LoadComplete() {
        base.LoadComplete();
        //this.screenStack.Push(new MainScreen());
        this.screenStack!.Push(new GameplayScreen());
    }

    protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent) =>
        this.dependencies = new DependencyContainer(base.CreateChildDependencies(parent));
}
