using System;

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;

using osuTK;

using ZeroV.Game.Utils;

namespace ZeroV.Game.Elements.Buttons;

public partial class DiamondButton : ClickableContainer {

    public DiamondButton() {
        this.Origin = Anchor.Centre;
        //this.Anchor = Anchor.Centre;
    }

    public required String Text { get; init; }

    private Diamond outerDiamond = null!;

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
                Padding = new MarginPadding(10),
                Child = new Diamond {
                    RelativeSizeAxes = Axes.Both,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Colour = Colour4.White,
                },
            },
            new ZeroVSpriteText{
                Text = this.Text,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                FontSize = 64,
                Colour= Colour4.Black
            }
        ];
    }

    public override Boolean Contains(Vector2 screenSpacePos) {
        return this.outerDiamond.Contains(screenSpacePos);
    }
}
