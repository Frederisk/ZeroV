using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using osu.Framework.Allocation;
using osu.Framework.Graphics.Containers;

using osuTK;
using osuTK.Graphics;

namespace ZeroV.Game.Graphics.Shapes;
internal partial class Particle : CompositeDrawable {

    public Single OuterDiameterSize { get; set; }
    public Single InnerDiameterSize { get; set; }

    [BackgroundDependencyLoader]
    private void load() {
        this.InternalChildren = [
            new Diamond {
                DiameterSize = this.OuterDiameterSize,
                Colour = Color4.Black,
            },
            new Diamond {
                DiameterSize = this.InnerDiameterSize,
                Colour = Color4.Red,
            },
        ];
    }


}
