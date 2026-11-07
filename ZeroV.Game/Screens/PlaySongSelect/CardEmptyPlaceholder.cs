using System;

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;

using osuTK;

using ZeroV.Game.Graphics;
using ZeroV.Game.Graphics.Shapes;

namespace ZeroV.Game.Screens.PlaySongSelect;

public sealed partial class CardEmptyPlaceholder : CompositeDrawable {
    private String title;
    private String message;

    public CardEmptyPlaceholder(String title, String message) {
        this.title = title;
        this.message = message;
        this.RelativeSizeAxes = Axes.Both;
    }

    [BackgroundDependencyLoader]
    private void load() {
        this.InternalChild = new FillFlowContainer {
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            AutoSizeAxes = Axes.Both,
            Direction = FillDirection.Vertical,
            //Spacing = new Vector2(8),
            Children = [
                //new FillFlowContainer {
                //    Anchor = Anchor.TopCentre,
                //    Origin = Anchor.TopCentre,
                //    AutoSizeAxes = Axes.Both,
                //    Direction = FillDirection.Horizontal,
                //    Children = [
                //        new Diamond {
                //            Anchor = Anchor.CentreLeft,
                //            Origin = Anchor.CentreLeft,
                //            Size = new Vector2(16),
                //            Margin = new MarginPadding { Horizontal = 8 },
                //            Colour = Colour4.Cyan,
                //        },
                //        new ZeroVSpriteText {
                //            Anchor = Anchor.CentreLeft,
                //            Origin = Anchor.CentreLeft,
                //            Colour = Colour4.Black,
                //            FontSize = 14,
                //            Font = FontUsage.Default.With(weight: "Bold"),
                //            Text = this.title,
                //        },
                //    ]
                //},
                new Diamond {
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopCentre,
                    Size = new Vector2(16),
                    Margin = new MarginPadding { Horizontal = 8 },
                    Colour = Colour4.Cyan,
                },
                new ZeroVSpriteText {
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopCentre,
                    Colour = Colour4.Black,
                    FontSize = 14,
                    Font = FontUsage.Default.With(weight: "Bold"),
                    Text = this.title,
                },
                new ZeroVSpriteText {
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopCentre,
                    Colour = Colour4.Black,
                    FontSize = 12,
                    Text = this.message,
                },
            ],
        };
    }
}
