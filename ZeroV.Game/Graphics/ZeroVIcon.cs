using System;

using osuTK;

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Rendering;
using osu.Framework.Graphics.Shapes;
using ZeroV.Game.Graphics.Shapes;
using ZeroV.Game.Utils;

namespace ZeroV.Game.Graphics;

public partial class ZeroIcon : CompositeDrawable {

    public ZeroIcon() {
        this.Anchor = Anchor.Centre;
        this.Origin = Anchor.Centre;
    }

    [BackgroundDependencyLoader]
    private void load(IRenderer renderer) {
        this.InternalChild
            //// Start
            //  = new OrientedTriangle(OrientedTriangle.Orientation.Right) {
            //        Anchor = Anchor.CentreRight,
            //        Origin = Anchor.CentreRight,
            //        Height = ZeroVMath.SQRT_3 / 2.0f,
            //        Width = 0.75f,
            //        RelativeSizeAxes = Axes.Both,
            //    };

            // Pause
            = new Container {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                RelativeSizeAxes = Axes.Both,
                Size = new Vector2(ZeroVMath.SQRT_2 / 2f),
                Children = [
                new Box {
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                    Width = 1f /3f,
                    // Margin = new MarginPadding(10f),
                    RelativeSizeAxes = Axes.Both,
                    //Colour = Colour4.Red,
                },
                new Box {
                    Anchor = Anchor.CentreRight,
                    Origin = Anchor.CentreRight,
                    Width = 1f / 3f,
                    // Margin = new MarginPadding(10f),
                    RelativeSizeAxes = Axes.Both,
                    //Colour = Colour4.Red,
                },
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
