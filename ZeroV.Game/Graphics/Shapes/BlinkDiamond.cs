using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;

using osuTK;
using osuTK.Graphics;

namespace ZeroV.Game.Graphics.Shapes;

public partial class BlinkDiamond : CompositeDrawable {

    public BlinkDiamond() {
        this.Anchor = Anchor.Centre;
        this.Origin = Anchor.Centre;
    }

    public Single OuterDiameterSize { get; init; } = 74;

    public Single InnerDiameterSize { get; init; } = 40;

    public Color4 OuterColor { get; init; } = Color4.Black;

    public Color4 InnerColor { get; init; } = Color4.Red;

    [BackgroundDependencyLoader]
    private void load() {
        this.InternalChildren = [
            new Diamond {
                Size = new Vector2(this.OuterDiameterSize),
                Colour = this.OuterColor,
            },
            new Diamond {
                Size = new Vector2(this.InnerDiameterSize),
                Colour = this.InnerColor,
            },
        ];
    }
}
