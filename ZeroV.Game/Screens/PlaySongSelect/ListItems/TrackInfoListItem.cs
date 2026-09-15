using System;
using System.Linq;

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;

using osuTK;

using ZeroV.Game.Objects;

namespace ZeroV.Game.Screens.PlaySongSelect.ListItems;

[Cached]
public partial class TrackInfoListItem(TrackInfo trackInfo) : CompositeDrawable {
    private Boolean isExpanded;
    private FillFlowContainer<MapInfoListItem> container = null!;
    private TrackInfoListItemHeader header = null!;

    public TrackInfo TrackInfo => trackInfo;

    [BackgroundDependencyLoader]
    private void load() {
        this.RelativeSizeAxes = Axes.X;
        this.AutoSizeAxes = Axes.Y;

        this.container = new FillFlowContainer<MapInfoListItem>() {
            RelativeSizeAxes = Axes.X,
            Height = 0,
            Alpha = 0,
            Direction = FillDirection.Vertical,
            Spacing = new Vector2(0, 4),
            Margin = new MarginPadding {
                Left = 24,
                Top = 6,
                Bottom = 4,
            },
        };

        this.AddInternal(new FillFlowContainer() {
            RelativeSizeAxes = Axes.X,
            AutoSizeAxes = Axes.Y,
            Direction = FillDirection.Vertical,
            Children = [
                this.header = new TrackInfoListItemHeader(),
                this.container,
            ],
        });

        foreach (MapInfo mapInfo in trackInfo.MapInfos) {
            this.container.Add(new MapInfoListItem(mapInfo));
        }
    }

    public void SelectFirst() {
        if (this.container.Children.Count <= 0 || this.container.Children.Any(item => item.IsSelected)) {
            return;
        }

        if (this.container.Children.Count > 0) {
            this.container.Children[0].OnSelect();
        }
    }

    public Boolean IsExpanded {
        get => this.isExpanded;
        set {
            this.isExpanded = value;
            this.header.UpdateExpandedState(value);

            if (value) {
                this.container.AutoSizeAxes = Axes.Y;
                this.container.FadeIn(200, Easing.OutQuint);
                this.header.TryBeginLongTitleScroll();
            } else {
                this.container.FadeOut(150, Easing.InSine);
                this.container.AutoSizeAxes = Axes.None;
                this.container.Height = 0;
                this.header.TryEndLongTitleScroll();
            }
        }
    }
}
