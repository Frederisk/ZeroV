using System;

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;

using osuTK;

using ZeroV.Game.Utils;

namespace ZeroV.Game.Elements;

public partial class DiamondButton : ClickableContainer {

    public DiamondButton() {
        this.Origin = Anchor.Centre;
        //this.Anchor = Anchor.Centre;
    }

    // public String Text { get; set; }

    private Diamond outerDiamond;

    [BackgroundDependencyLoader]
    private void load() {
        this.outerDiamond = new Diamond {
            RelativeSizeAxes = Axes.Both,
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            Colour = Colour4.Red,
        };
        this.Children = [
            this.outerDiamond,
            new Container {
                RelativeSizeAxes = Axes.Both,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                FillMode = FillMode.Fit,
                Padding = new MarginPadding(10 * ZeroVMath.SQRT_2),
                Child = new Diamond {
                    RelativeSizeAxes = Axes.Both,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Colour = Colour4.White,
                },
            },
            new ZeroVSpriteText{
                // FIXME: Text = this.Text,
                Text = "Default",
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                FontSize = 72,
                Colour= Colour4.Black
            }
        ];
    }

    public override Boolean Contains(Vector2 screenSpacePos) {
        return this.outerDiamond.Contains(screenSpacePos);
    }
}
