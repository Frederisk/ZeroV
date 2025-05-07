using System;

using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Pooling;

using NUnit.Framework;

using ZeroV.Game.Elements;
using osu.Framework.Allocation;
using ZeroV.Game.Graphics;

namespace ZeroV.Game.Tests.Visual.Elements;

[TestFixture]
public partial class TestSceneTargetRainbowEffect : ZeroVTestScene {
    private DrawablePool<TargetSpinEffect> rainbowPool = new(10, 15);
    private Container container = null!;
    private ZeroVSpriteText text = null!;

    [BackgroundDependencyLoader]
    private void load() {
        this.container = new Container() {
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            RelativeSizeAxes = Axes.Both,
        };
        this.text = new ZeroVSpriteText() {
            FontSize = 100,
            Anchor = Anchor.TopCentre,
            Origin = Anchor.TopCentre,
        };
        this.Children = [this.rainbowPool, this.text, this.container];
    }

    [Test]
    public void TestRainbowEffect() {
        this.AddStep("Add one effect", () => {
            TargetSpinEffect target = this.rainbowPool.Get(t => t.SetUpTargetColour(Game.Scoring.TargetResult.PerfectEarly));
            this.container.Add(target);
            this.text.Text = this.container.Count.ToString();
        });
    }
}
