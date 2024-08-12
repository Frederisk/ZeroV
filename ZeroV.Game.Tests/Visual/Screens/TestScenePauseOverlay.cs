using System;

using NUnit.Framework;

using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Testing;

using ZeroV.Game.Screens.Gameplay;

namespace ZeroV.Game.Tests.Visual.Screens;

[TestFixture]
public partial class TestScenePauseOverlay : ZeroVTestScene {
    private PauseOverlay overlay = default!;

    [SetUpSteps]
    public void SetUpSteps() {
        this.AddStep("create overlay", this.createOverlay);
    }

    private void createOverlay() {
        this.Add(new Box() { RelativeSizeAxes = Axes.Both, Colour = Colour4.White });
        this.Add(this.overlay = new PauseOverlay() {
            OnQuit = ()=>{ },
            OnResume = () => { },
            OnRetry = () => { },
        });
        this.overlay.Show();
    }
}
