using System;

using NUnit.Framework;

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shaders;
using osu.Framework.Graphics.Shapes;

using osuTK;

using ZeroV.Game.Graphics.Shapes;

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

    public TestSceneShaders() {
        //this.Add(new Box {
        //    RelativeSizeAxes = Axes.Both,
        //    Colour = Colour4.Red,
        //});
        this.Add(this.box);
        this.Add(new TapLight {
            Size = new Vector2(128, 512),
            X = -128,
            Origin = Anchor.Centre,
            Anchor = Anchor.Centre,
        });
    }

    protected override void Update() {
        base.Update();
        var time = (this.Time.Current / 1000 / 3) % 1;
        this.box.SizeRatio = (Single)time;
    }

    public partial class TapLight : Box, ITexturedShaderDrawable {

        [BackgroundDependencyLoader]
        private void load(ShaderManager shaders) {
            this.TextureShader = shaders.Load(VertexShaderDescriptor.TEXTURE_2, "OrbitSelfLuminous");
        }
    }
}
