using System;

using osuTK;

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Rendering;
using osu.Framework.Graphics.Shapes;
using ZeroV.Game.Graphics.Shapes;
using ZeroV.Game.Utils;

namespace ZeroV.Game.Graphics.Icons;

public abstract partial class IconBase : CompositeDrawable {

    public IconBase() {
        this.Anchor = Anchor.Centre;
        this.Origin = Anchor.Centre;
    }
}

// public partial class ZeroVIcon : Sprite {
//     //[Resolved]
//     //private TextureStore textureStore { get; set; }

//     // private in

//     public override Texture Texture { get => base.Texture; set => throw new InvalidOperationException("The texture of a ZeroVIcons cannot be set."); }

//     public required ZeroIconType IconType { get; init; }

//     [BackgroundDependencyLoader]
//     private void load(IRenderer renderer) {
//         this.FillMode = FillMode.Fit;
//         this.FillAspectRatio = 1;
//         // TODO: load Texture by IconType;
//         // this.Texture = textureStore.Get(@""/*From IconType*/);
//         // base.Texture = renderer.WhitePixel;
//     }
// }

// public enum ZeroIconType {
//     Exit,
//     Pulse,
//     Retry,
// }
