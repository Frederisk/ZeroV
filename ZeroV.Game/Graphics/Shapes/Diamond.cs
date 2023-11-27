using System;

using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;

namespace ZeroV.Game;

public partial class Diamond : Box {
    public Diamond() {
        this.Origin = Anchor.Centre;
        this.Anchor = Anchor.Centre;
        this.Rotation = 45;
    }
}
