using System;
using System.Drawing;

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;

using osuTK;

using ZeroV.Game.Graphics;
using ZeroV.Game.Graphics.Shapes;

namespace ZeroV.Game.Elements.Buttons;

public partial class DiamondButton : ClickableContainer {
    private ColourInfo innerColour;

    public required ColourInfo InnerColour {
        get => this.innerColour;
        set {
            this.innerColour = value;
            if (this.innerDiamond is not null) {
                this.innerDiamond.Colour = this.innerColour;
            }
        }
    }

    private ColourInfo outerColour;

    public required ColourInfo OuterColour {
        get => this.outerColour;
        set {
            this.outerColour = value;
            if (this.outerDiamond is not null) {
                this.outerDiamond.Colour = this.outerColour;
            }
        }
    }

    private MarginPadding diamondPadding;

    public required Single DiamondPadding {
        get => this.diamondPadding.Top;
        set {
            this.diamondPadding = new MarginPadding(value);
            if (this.diamondContainer is not null) {
                this.diamondContainer.Padding = this.diamondPadding;
            }
        }
    }

    //public required String Text { get; init; }
    public required ZeroVSpriteText Text { get; init; }

    //private ZeroVSpriteText spriteText = null!;

    private Diamond outerDiamond = null!;

    private Diamond innerDiamond = null!;

    private Container<Diamond> diamondContainer = null!;

    public DiamondButton() {
        this.Origin = Anchor.Centre;
        // this.Anchor = Anchor.Centre;
    }

    [BackgroundDependencyLoader]
    private void load() {
        this.outerDiamond = new Diamond {
            RelativeSizeAxes = Axes.Both,
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            Colour = this.OuterColour,
        };
        this.innerDiamond = new Diamond {
            RelativeSizeAxes = Axes.Both,
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            Colour = this.innerColour,
        };
        //this.spriteText = new ZeroVSpriteText {
        //    Text = this.Text,
        //    Anchor = Anchor.Centre,
        //    Origin = Anchor.Centre,
        //    FontSize = this.fontSize,
        //    Colour = Colour4.Black
        //};
        this.diamondContainer = new Container<Diamond> {
            RelativeSizeAxes = Axes.Both,
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            FillMode = FillMode.Fit,
            Padding = this.diamondPadding,
            Child = this.innerDiamond,
            Blending = BlendingParametersExtensions.TransparentAlphaAddWithColour,
        };
        this.Children = [
            new BufferedContainer {
                RelativeSizeAxes = Axes.Both,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Children = [
                    this.outerDiamond,
                    this.diamondContainer,
                ],
            },
            this.Text,
        ];
    }

    public override Boolean Contains(Vector2 screenSpacePos) {
        return this.outerDiamond.Contains(screenSpacePos);
    }

    protected override Boolean OnTouchDown(TouchDownEvent e) {
        if (this.Enabled.Value) {
            this.Action?.Invoke();
        }

        return true;
    }
}
