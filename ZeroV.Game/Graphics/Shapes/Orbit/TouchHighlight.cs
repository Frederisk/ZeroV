using osu.Framework.Allocation;
using osu.Framework.Graphics.Shaders;
using osu.Framework.Graphics.Shapes;

namespace ZeroV.Game.Graphics.Shapes.Orbit;

public partial class TouchHighlight : Box {

    [BackgroundDependencyLoader]
    private void load(ShaderManager shaders) {
        this.TextureShader = shaders.Load(VertexShaderDescriptor.TEXTURE_2, "OrbitSelfLuminous");
    }
}
