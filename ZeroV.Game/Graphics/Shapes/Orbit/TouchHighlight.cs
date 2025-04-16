using System;

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shaders;
using osu.Framework.Graphics.Shapes;

using osuTK;

namespace ZeroV.Game.Graphics.Shapes.Orbit;

public partial class TouchHighlight : Box {
    private Boolean isHidden;

    private HighlightPosition position;

    public TouchHighlight(HighlightPosition position) : base() {
        this.position = position;
    }

    [BackgroundDependencyLoader]
    private void load(ShaderManager shaders) {
        String fragment = this.position switch {
            HighlightPosition.Middle => "OrbitSelfLuminous",
            HighlightPosition.Right => "OrbitSelfLuminousRight",
            HighlightPosition.Left => "OrbitSelfLuminousLeft",
            _ => throw new ArgumentOutOfRangeException(nameof(this.position), this.position, null),
        };
        this.TextureShader = shaders.Load(VertexShaderDescriptor.TEXTURE_2, fragment);
    }

    protected override void LoadComplete() {
        base.LoadComplete();
        this.FadeOut();
        this.isHidden = true;
    }

    public override void Hide() {
        if (this.isHidden) { return; }
        this.isHidden = true;
        this.FadeOut(100);
    }

    public override void Show() {
        if (!this.isHidden) { return; }
        this.isHidden = false;
        this.FadeIn(0);
    }

    public enum HighlightPosition {
        Left,
        Right,
        Middle,
    }
}
