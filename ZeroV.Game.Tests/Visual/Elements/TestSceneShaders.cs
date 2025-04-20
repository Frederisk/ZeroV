using System;

using NUnit.Framework;

using osu.Framework.Graphics;

using osuTK;

using ZeroV.Game.Graphics.Shapes;
using ZeroV.Game.Graphics.Shapes.Orbit;

namespace ZeroV.Game.Tests.Visual.Elements;

[TestFixture]
public partial class TestSceneShaders : ZeroVTestScene {

    private RainbowDiamond box = new() {
        Size = new Vector2(256),
        X = 128,
        Origin = Anchor.Centre,
        Anchor = Anchor.Centre,
        HsvaColour = new Vector4(-1, 0.5f, 1, 0.7f),
        SizeRatio = 1,
        BorderRatio = 0.04f,
    };

    private TouchHighlight highlight = new(TouchHighlight.HighlightPosition.Left) {
        Size = new Vector2(128, 512),
        X = -128,
        Origin = Anchor.Centre,
        Anchor = Anchor.Centre,
    };

    public TestSceneShaders() {
        //this.Add(new Box {
        //    RelativeSizeAxes = Axes.Both,
        //    Colour = Colour4.Red,
        //});
        this.Add(this.box);
        this.Add(this.highlight);
        this.highlight.OnLoadComplete += (o) => {
            o.FadeIn();
        };
    }

    protected override void Update() {
        base.Update();
        var time = (this.Time.Current / 1000 / 3) % 1;
        this.box.SizeRatio = (Single)time;
    }
}
