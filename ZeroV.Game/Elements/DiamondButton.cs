using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;

namespace ZeroV.Game.Elements;
public partial class DiamondButton: ClickableContainer {
    public DiamondButton() {
        this.Origin = Anchor.Centre;
        //this.Anchor = Anchor.Centre;
    }

    public SpriteText? Text { get; set; }

    [BackgroundDependencyLoader]
    private void load() {
        this.Children = [
            new Diamond {
                RelativeSizeAxes = Axes.Both,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Colour = Colour4.Red,
            },
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
            }
        ];
    }


}
