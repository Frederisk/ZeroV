using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;

using ZeroV.Game.Elements.Buttons;

namespace ZeroV.Game.Screens;

public partial class DirectorySelectorScreen : BaseUserInterfaceScreen {

    [BackgroundDependencyLoader]
    private void load() {
        this.InternalChild = new FillFlowContainer {
            Direction = FillDirection.Vertical,
            RelativeSizeAxes = Axes.X,
            AutoSizeAxes = Axes.Y,
            Children = [
                new BackButton(this){
                    Anchor = Anchor.TopLeft,
                    Origin = Anchor.TopLeft,
                },
            ]
        };
    }
}
