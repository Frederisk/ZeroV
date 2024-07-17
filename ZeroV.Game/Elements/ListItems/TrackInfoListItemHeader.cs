using System;

using osu.Framework.Allocation;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics;
using osu.Framework.Input.Events;

using ZeroV.Game.Objects;
using ZeroV.Game.Screens;

namespace ZeroV.Game.Elements.ListItems;

public partial class TrackInfoListItemHeader : CompositeDrawable {

    [Resolved]
    private PlaySongSelectScreen songSelect { get; set; } = null!;

    [Resolved]
    private TrackInfoListItem listItem { get; set; } = null!;

    [BackgroundDependencyLoader]
    private void load() {
        this.RelativeSizeAxes = Axes.X;
        this.Height = 100;

        this.AddInternal(new Box() {
            RelativeSizeAxes = Axes.Both,
            Colour = Colour4.Blue
        });

        TrackInfo trackInfo = this.listItem.TrackInfo;
        this.AddInternal(new SpriteText() {
            Origin = Anchor.Centre,
            Anchor = Anchor.Centre,
            Text = $"{trackInfo.Title} - {trackInfo.Artists} - {trackInfo.Album}",
            Colour = Colour4.Black,
            Font = FontUsage.Default.With(size: 52)
        });
    }

    protected override Boolean OnClick(ClickEvent e) {
        this.listItem.IsExpanded = !this.listItem.IsExpanded;
        if (this.listItem.IsExpanded) {
            this.songSelect.OnExpanded(this.listItem);
        }
        return base.OnClick(e);
    }
}
