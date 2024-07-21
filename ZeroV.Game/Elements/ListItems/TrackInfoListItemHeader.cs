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
        this.AutoSizeAxes = Axes.Y;

        this.AddInternal(new Box() {
            RelativeSizeAxes = Axes.Both,
            Colour = Colour4.Blue
        });

        TrackInfo trackInfo = this.listItem.TrackInfo;

        var artists = trackInfo.Artists ?? "[No artists]";
        var album = trackInfo.Album ?? "[No album]";

        this.AddInternal(new FillFlowContainer() {
            RelativeSizeAxes = Axes.X,
            AutoSizeAxes = Axes.Y,
            Direction = FillDirection.Vertical,
            Spacing = new osuTK.Vector2(0, 5),
            Padding = new MarginPadding(5),
            Children = [
                new SpriteText() {
                    Origin = Anchor.CentreLeft,
                    Anchor = Anchor.CentreLeft,
                    Text = trackInfo.Title,
                    Colour = Colour4.Black,
                    Font = FontUsage.Default.With(size: 52)
                },
                new SpriteText() {
                    Origin = Anchor.CentreLeft,
                    Anchor = Anchor.CentreLeft,
                    Text = $"{artists} - {album}",
                    Colour = Colour4.Black,
                    Font = FontUsage.Default.With(size: 32)
                }
             ]
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
