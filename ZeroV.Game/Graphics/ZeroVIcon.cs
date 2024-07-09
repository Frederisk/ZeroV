using System;

using osuTK;

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Rendering;
using osu.Framework.Graphics.Shapes;

namespace ZeroV.Game.Graphics;

public partial class ZeroIcon : CompositeDrawable {
    public ZeroIcon() {
        this.Anchor = Anchor.Centre;
        this.Origin = Anchor.Centre;
        // this.AutoSizeAxes = Axes.Both;
    }

    [BackgroundDependencyLoader]
    private void load(IRenderer renderer) {
        this.InternalChild = new Container {
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            RelativeSizeAxes = Axes.Both,
            Size = new Vector2(0.95f),
            Children = [
                new Box {
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,

                    // Margin = new MarginPadding(10f),
                    RelativeSizeAxes = Axes.Both,
                    Colour = Colour4.Red,
                }
            ],
        };
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
