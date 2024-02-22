using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Graphics.Shapes;

using osuTK;

namespace ZeroV.Game.Elements;

public partial class SwitchButton : Checkbox {

    public SwitchButton() {
        this.Size = new Vector2(50, 25);
    }

    [BackgroundDependencyLoader]
    private void Load() {
        this.InternalChild = new Container {
            RelativeSizeAxes = Axes.Both,
            Children = [
                new Box{
                    RelativeSizeAxes = Axes.Both,
                    Colour = Colour4.Yellow,
                },
                // Background
                new Diamond {
                    Origin = Anchor.CentreLeft,
                    Anchor = Anchor.CentreLeft,
                    DiameterSize = this.Size.Y,
                    Colour = Colour4.Black,
                },
                new Diamond {
                    Origin = Anchor.CentreRight,
                    Anchor = Anchor.CentreRight,
                    DiameterSize = this.Size.Y,
                    Colour = Colour4.Black,
                }
            ],
        };
    }
}
