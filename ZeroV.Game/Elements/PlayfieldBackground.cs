using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;

namespace ZeroV.Game.Elements;

internal partial class PlayfieldBackground : Box {
    public PlayfieldBackground() {
        this.RelativeSizeAxes = Axes.Both;
        this.Colour = Colour4.LightBlue;
    }

    [BackgroundDependencyLoader]
    private void load() {
    }
}
