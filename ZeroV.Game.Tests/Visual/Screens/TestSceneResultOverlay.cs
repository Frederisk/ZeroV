using NUnit.Framework;

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Testing;

using ZeroV.Game.Scoring;
using ZeroV.Game.Screens.Gameplay;

namespace ZeroV.Game.Tests.Visual.Screens;

[TestFixture]
public partial class TestSceneResultOverlay : ZeroVTestScene {
    private ResultOverlay overlay = null!;

    [SetUpSteps]
    public void SetUpSteps() {
        this.AddStep("create overlay", this.createOverlay);
    }

    private DependencyContainer? dependencies;
    protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent) =>
        this.dependencies = new DependencyContainer(base.CreateChildDependencies(parent));

    private void createOverlay() {
        //ScoringCalculator calculator = new(7);
        //calculator.AddTarget(TargetResult.MaxPerfect);
        //this.dependencies!.CacheAs(calculator);
        // FIXME: Cache
        this.Add(new Box() { RelativeSizeAxes = Axes.Both, Colour = Colour4.White });
        this.Add(this.overlay = new ResultOverlay());
        this.overlay.Show();
    }
}
