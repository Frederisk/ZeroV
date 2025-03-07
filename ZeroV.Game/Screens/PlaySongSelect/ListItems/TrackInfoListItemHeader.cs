using System;
using System.Linq;

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;

using ZeroV.Game.Objects;

namespace ZeroV.Game.Screens.PlaySongSelect.ListItems;

public partial class TrackInfoListItemHeader : CompositeDrawable {
    private FillFlowContainer titleContainer = null!;
    private SpriteText title = null!;
    private SpriteText subTitle = null!;

    [Resolved]
    private PlaySongSelectScreen songSelectScreen { get; set; } = null!;

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
        this.titleContainer = new FillFlowContainer() {
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
        };
        this.AddInternal(this.titleContainer);
    }

    private const Single padding = 5;
    private const Double long_title_scroll_speed = 0.1;

    public void TryBeginLongTitleScroll() {
        void scroll(Drawable drawable) {
            if (drawable.Transforms.Any() || drawable.DrawWidth < this.DrawWidth) {
                return;
            }
            var offset = drawable.DrawWidth - this.DrawWidth + padding * 2;
            var duration = offset / long_title_scroll_speed;

            var toMargin = new MarginPadding { Left = -offset };
            drawable.TransformTo(nameof(drawable.Margin), toMargin, duration).Then()
                    .Delay(1000).Then()
                    .TransformTo(nameof(drawable.Margin), new MarginPadding(), duration).Then()
                    .Delay(1000).Then()
                    .Loop();
        }

        if (this.IsHovered || this.listItem.IsExpanded) {
            scroll(this.title);
            scroll(this.subTitle);
        }
    }

    public void TryEndLongTitleScroll() {
        if (!this.IsHovered && !this.listItem.IsExpanded) {
            this.title.ClearTransforms();
            this.title.Margin = new MarginPadding(0);
            this.subTitle.ClearTransforms();
            this.subTitle.Margin = new MarginPadding(0);
        }
    }

    protected override Boolean OnClick(ClickEvent e) {
        this.listItem.IsExpanded = !this.listItem.IsExpanded;
        if (this.listItem.IsExpanded) {
            this.songSelectScreen.OnExpanded(this.listItem);
        }
        return base.OnClick(e);
    }
}
