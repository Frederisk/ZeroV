using System;

using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;

using osuTK;

namespace ZeroV.Game;

/// <summary>
/// A diamond shape. Implemented by <see cref="Box"/> rotated 45 degrees.
/// </summary>
public partial class Diamond : Box {

    public Diamond() {
        this.Origin = Anchor.Centre;
        this.Anchor = Anchor.Centre;
        this.Rotation = 45;
    }

    public new Vector2 Size {
        get => base.Size;
        [Obsolete("Don't set Size manually. Use DiameterSize instead.")]
        set {
            if (value.X != value.Y) {
                throw new ArgumentException("Diamonds must be square.");
            }
            base.Size = value;
        }
    }

    public override Single Width {
        get => base.Width;
        set {
            base.Width = value;
            base.Height = value;
        }
    }

    public override Single Height {
        get => base.Height;
        set {
            base.Width = value;
            base.Height = value;
        }
    }

    public Single DiameterSize {
        get => base.Width;
        set => base.Size = new Vector2(value);
    }
}
