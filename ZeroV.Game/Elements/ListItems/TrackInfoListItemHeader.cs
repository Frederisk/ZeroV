using System;

using osu.Framework.Allocation;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics;
using osu.Framework.Input.Events;

using ZeroV.Game.Objects;
using ZeroV.Game.Screens;
using System.Linq;

namespace ZeroV.Game.Elements.ListItems;

public partial class TrackInfoListItemHeader : CompositeDrawable {
    private FillFlowContainer titleContainer = null!;
    private SpriteText title = null!;
    private SpriteText subTitle = null!;

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

        this.AddInternal(this.titleContainer = new FillFlowContainer() {
            RelativeSizeAxes = Axes.X,
            AutoSizeAxes = Axes.Y,
            Direction = FillDirection.Vertical,
            Spacing = new osuTK.Vector2(0, 5),
            Padding = new MarginPadding(padding),
            Children = [
                this.title = new SpriteText() {
                    Origin = Anchor.CentreLeft,
                    Anchor = Anchor.CentreLeft,
                    Text = trackInfo.Title,
                    Colour = Colour4.Black,
                    Font = FontUsage.Default.With(size: 52)
                },
                this.subTitle = new SpriteText() {
                    Origin = Anchor.CentreLeft,
                    Anchor = Anchor.CentreLeft,
                    Text = $"{artists} - {album}",
                    Colour = Colour4.Black,
                    Font = FontUsage.Default.With(size: 32)
                }
             ]
        });
    }

    private const Single padding = 5;
    private const Double long_title_scroll_speed = 0.2;
    
    public void TryBeginLongTitleScroll() {
        if(this.IsHovered || this.listItem.IsExpanded) {
            if (!this.title.Transforms.Any() && this.title.DrawWidth > this.DrawWidth) {
                var offset = this.title.DrawWidth - this.DrawWidth + (padding * 2);
                var duration = offset / long_title_scroll_speed;

                var toMargin = new MarginPadding(0) { Left = -offset };
                this.title.TransformTo(nameof(this.Margin), toMargin, duration);
            }
            if (!this.subTitle.Transforms.Any() && this.subTitle.DrawWidth > this.DrawWidth) {
                var offset = this.subTitle.DrawWidth - this.DrawWidth + (padding * 2);
                var duration = offset / long_title_scroll_speed;

                var toMargin = new MarginPadding(0) { Left = -offset };
                this.subTitle.TransformTo(nameof(this.Margin), toMargin, duration);
            }
        }
    }
    public void TryEndLongTitleScroll() {
        if(!this.IsHovered && !this.listItem.IsExpanded) {
            this.title.ClearTransforms();
            this.title.Margin = new MarginPadding(0);
            this.subTitle.ClearTransforms();
            this.subTitle.Margin = new MarginPadding(0);
        }
    }

    protected override Boolean OnClick(ClickEvent e) {
        this.listItem.IsExpanded = !this.listItem.IsExpanded;
        if (this.listItem.IsExpanded) {
            this.songSelect.OnExpanded(this.listItem);
        }
        return base.OnClick(e);
    }

    protected override Boolean OnHover(HoverEvent e) {
        this.TryBeginLongTitleScroll();
        return base.OnHover(e);
    }
    protected override void OnHoverLost(HoverLostEvent e) {
        this.TryEndLongTitleScroll();
        base.OnHoverLost(e);
    }
}
