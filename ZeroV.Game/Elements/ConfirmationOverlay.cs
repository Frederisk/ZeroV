using System;

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;

using osuTK;

using ZeroV.Game.Graphics;

namespace ZeroV.Game.Elements;

public partial class ConfirmationOverlay : CompositeDrawable {

    private String title;
    private String message;

    private ZeroVSpriteText titleSpriteText = null!;
    private ZeroVSpriteText messageSpriteText = null!;
    private Button okButton = null!;
    private Button cancelButton = null!;

    public ConfirmationOverlay(String title, String message) {
        this.title = title;
        this.message = message;
        this.RelativeSizeAxes = Axes.Both;
    }

    [BackgroundDependencyLoader]
    private void load() {
        this.titleSpriteText = new ZeroVSpriteText {
            FontSize = 32,
            Text = this.title,
        };
        this.messageSpriteText = new ZeroVSpriteText {
            FontSize = 16,
            Text = this.message,
        };
        this.okButton = new BasicButton {
            Size = new Vector2(90, 32),
            Text = "OK",
        };
        this.cancelButton = new BasicButton {
            Size = new Vector2(90, 32),
            Text = "Cancel",
        };
        this.InternalChild = new Container {
            new Box {
                RelativeSizeAxes = Axes.Both,
                Colour = Colour4.Black,
                Alpha = 0.75f,
            },
            new FillFlowContainer {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                RelativeSizeAxes = Axes.X,
                AutoSizeAxes = Axes.Y,
                Direction = FillDirection.Vertical,
                Children = [
                    this.titleSpriteText,
                    this.messageSpriteText,
                    new FillFlowContainer {
                        //Anchor
                    }
                ],
            }
        };
    }

    protected override Boolean OnClick(ClickEvent e) {
        // base.OnClick(e);
        return true;
    }
}
