using System;

using osu.Framework.Allocation;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics;
using osu.Framework.Input.Events;

using ZeroV.Game.Objects;
using ZeroV.Game.Screens;

namespace ZeroV.Game.Elements;

public partial class MapInfoListItem(MapInfo mapInfo) : CompositeDrawable {
    private Boolean selected;

    public MapInfo MapInfo => mapInfo;

    [Resolved]
    private PlaySongSelectScreen songSelect { get; set; } = null!;

    [BackgroundDependencyLoader]
    private void load() {
        this.Masking = true;
        this.BorderColour = Colour4.White;
        this.RelativeSizeAxes = Axes.X;
        this.Height = 50;
        this.Margin = new MarginPadding(0) {
            Top = 5
        };

        this.AddInternal(new Box() {
            RelativeSizeAxes = Axes.Both,
            Colour = Colour4.Green
        });
        this.AddInternal(new SpriteText() {
            Origin = Anchor.TopCentre,
            Anchor = Anchor.TopCentre,
            Text = $"Difficulty: {mapInfo.Difficulty}",
            Colour = Colour4.Black,
            Font = FontUsage.Default.With(size: 52)
        });
    }

    public void OnSelect() {
        if (!this.selected) {
            this.songSelect.OnSelect(this);

            this.selected = true;
            this.BorderThickness = 3;
        } else {
            this.songSelect.ConfirmSelect();
        }
    }

    public void OnSelectCancel() {
        this.selected = false;
        this.BorderThickness = 0;
    }

    protected override Boolean OnClick(ClickEvent e) {
        this.OnSelect();
        return base.OnClick(e);
    }
}
