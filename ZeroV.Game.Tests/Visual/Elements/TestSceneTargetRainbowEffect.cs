using System;

using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Pooling;

using NUnit.Framework;

using ZeroV.Game.Elements;
using osu.Framework.Allocation;
using ZeroV.Game.Graphics;
using ZeroV.Game.Scoring;

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

    private void addEffect(TargetResult result) {
        TargetSpinEffect target = this.rainbowPool.Get(t => t.SetUpTargetColour(result));
        this.container.Add(target);
        this.text.Text = this.container.Count.ToString();
    }

    [Test]
    public void TestRainbowEffect() {
        this.AddStep("Add MaxPerfect effect", () => {
            this.addEffect(TargetResult.MaxPerfect);
        });
        this.AddWaitStep("Wait for effect", 1);
        this.AddStep("Add Perfect effect", () => {
            this.addEffect(TargetResult.Perfect);
        });
        this.AddWaitStep("Wait for effect", 1);
        this.AddStep("Add Normal effect", () => {
            this.addEffect(TargetResult.Normal);
        });
    }
}
