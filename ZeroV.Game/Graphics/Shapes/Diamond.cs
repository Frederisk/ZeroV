using System;

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;

using osuTK;

namespace ZeroV.Game;

/////// A diamond shape. Implemented by <see cref="Box"/> rotated 45 degrees.
public partial class Diamond : CompositeDrawable {
    private Box internalBox = new() {
        Origin = Anchor.Centre,
        Anchor = Anchor.Centre,
        Rotation = 45,
    };

    public Diamond() {
        this.AutoSizeAxes = Axes.Both;
        this.Origin = Anchor.Centre;
        this.Anchor = Anchor.Centre;
        this.InternalChild = this.internalBox;
    }

    //[BackgroundDependencyLoader]
    //private void load() {
    //}

    public new Vector2 Size {
        get => this.internalBox.Size;
        [Obsolete("Don't set Size manually. Use DiameterSize instead.")]
        set {
            if (value.X != value.Y) {
                throw new ArgumentException("Diamonds must be square.");
            }
            this.internalBox.Size = value;
        }
    }

    public Single DiameterSize {
        get => this.internalBox.Width * MathF.Sqrt(2);
        set => this.internalBox.Size = new Vector2(value / MathF.Sqrt(2));
    }

    // [Obsolete("Don't set Size manually. Use DiameterSize instead.")]
    // public new Single Width {
    //     get => base.Width;
    //     set {
    //         base.Width = value;
    //         base.Height = value;
    //     }
    // }

    // [Obsolete("Don't set Size manually. Use DiameterSize instead.")]
    // public new Single Height {
    //     get => base.Height;
    //     set {
    //         base.Width = value;
    //         base.Height = value;
    //     }
    // }
}
