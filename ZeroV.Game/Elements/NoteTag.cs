using System;

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;

using osuTK;

using ZeroV.Game.Graphics;

namespace ZeroV.Game.Elements;

public sealed partial class NoteTag : CompositeDrawable {
    private readonly string label;
    private readonly ColourInfo colourInfo;
    private Int32 noteCount;
    private ZeroVSpriteText countSpriteText = null!;

    public Int32 NoteCount {
        get => this.noteCount;
        set {
            if (this.noteCount == value) { return; }
            this.noteCount = value;
            if (this.countSpriteText is not null) {
                this.countSpriteText.Text = value.ToString();
            }
        }
    }

    public NoteTag(String label, ColourInfo colourInfo) {
        this.label = label;
        this.colourInfo = colourInfo;
        this.AutoSizeAxes = Axes.Both;
    }

    [BackgroundDependencyLoader]
    private void load() {
        this.countSpriteText = new ZeroVSpriteText {
            Anchor = Anchor.CentreLeft,
            Origin = Anchor.CentreLeft,
            Colour = Colour4.Black,
            FontSize = 26,
            Text = this.noteCount.ToString(),
        };

        this.InternalChild = new FillFlowContainer {
            AutoSizeAxes = Axes.Both,
            Direction = FillDirection.Horizontal,
            Spacing = new Vector2(4, 0),
            Children = [
                new ZeroVSpriteText {
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                    Colour = this.colourInfo,
                    Font = FontUsage.Default.With(size: 24,weight: "Bold"),
                    Text = this.label,
                },
                this.countSpriteText,
            ],
        };
    }
}
