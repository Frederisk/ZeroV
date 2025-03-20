

using NUnit.Framework;

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shaders;
using osu.Framework.Graphics.Shapes;
using osuTK;

namespace ZeroV.Game.Tests.Visual.Elements;

[TestFixture]
public partial class TestSceneShaders : ZeroVTestScene {

    public TestSceneShaders() {
        this.Add(new Box {
            RelativeSizeAxes = Axes.Both,
            Colour = Colour4.Red,
        });
        this.Add(new RainbowBox {
            Size = new Vector2(256),
            X = 128,
            Origin = Anchor.Centre,
            Anchor = Anchor.Centre,
        });
        this.Add(new TapLight {
            Size = new Vector2(128, 512),
            X = -128,
            Origin = Anchor.Centre,
            Anchor = Anchor.Centre,
        });
    }

    public partial class TapLight : Box, ITexturedShaderDrawable {
        [BackgroundDependencyLoader]
        private void load(ShaderManager shaders) {
            this.TextureShader = shaders.Load(VertexShaderDescriptor.TEXTURE_2, "OrbitSelfLuminous");
        }
    }

    public partial class RainbowBox : Box, ITexturedShaderDrawable {
        [BackgroundDependencyLoader]
        private void load(ShaderManager shaders) {
            this.TextureShader = shaders.Load(VertexShaderDescriptor.TEXTURE_2, "SpinRainbow");
        }
    }

}
