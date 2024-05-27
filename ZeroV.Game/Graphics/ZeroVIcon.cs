using System;

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Rendering;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;

namespace ZeroV.Game.Graphics;

public partial class ZeroVIcon : Sprite {
    //[Resolved]
    //private TextureStore textureStore { get; set; }

    public override Texture Texture { get => base.Texture; set => throw new InvalidOperationException("The texture of a ZeroVIcons cannot be set."); }

    public required ZeroIconType IconType { get; init; }

    [BackgroundDependencyLoader]
    private void load(IRenderer renderer) {
        this.FillMode = FillMode.Fit;
        this.FillAspectRatio = 1;
        // TODO: load Texture by IconType;
        // this.Texture = textureStore.Get(@""/*From IconType*/);
        base.Texture = renderer.WhitePixel;
    }
}

public enum ZeroIconType {
    Exit,
    Pulse,
    Retry,
}
