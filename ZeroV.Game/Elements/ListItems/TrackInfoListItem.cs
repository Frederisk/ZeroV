using System;
using System.Linq;

using osu.Framework.Allocation;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics;

using osuTK;

using ZeroV.Game.Objects;

namespace ZeroV.Game.Elements.ListItems;

[Cached]
public partial class TrackInfoListItem(TrackInfo trackInfo) : CompositeDrawable {
    private Boolean isExpanded;
    private FillFlowContainer<MapInfoListItem> container = null!;
    private TrackInfoListItemHeader header = null!;

    public TrackInfo TrackInfo => trackInfo;

    [BackgroundDependencyLoader]
    private void load() {
        this.Masking = true;
        this.BorderColour = Colour4.White;
        this.RelativeSizeAxes = Axes.X;
        this.AutoSizeAxes = Axes.Y;

        this.container = new FillFlowContainer<MapInfoListItem>() {
            RelativeSizeAxes = Axes.X,
            Height = 0,
            Direction = FillDirection.Vertical,
            Spacing = new Vector2(0, 0),
            Margin = new MarginPadding(0) {
                Left = 100
            }
        };

        this.AddInternal(new FillFlowContainer() {
            RelativeSizeAxes = Axes.X,
            AutoSizeAxes = Axes.Y,
            Children = [
                this.header = new TrackInfoListItemHeader(),
                this.container
            ]
        });

        foreach (MapInfo mapInfo in trackInfo.MapInfos) {
            this.container.Add(new MapInfoListItem(mapInfo));
        }
    }

    public void SelectFirst() {
        if (this.container.Children.Count <= 0 || this.container.Children.Any(item => item.IsSelected)) {
            return;
        }

        //if (this.container.Children.Count > 0) {
        // This method is called only when the item is exoanded for the first time.
        // foreach(MapInfoListItem item in this.container.Children) {
        //     if (item.IsSelected) {
        //         return;
        //     }
        // }
        this.container.Children.FirstOrDefault()?.OnSelect();
        //}
    }

    public Boolean IsExpanded {
        get => this.isExpanded;
        set {
            this.isExpanded = value;

            if (value) {
                this.container.AutoSizeAxes = Axes.Y;
                this.header.TryBeginLongTitleScroll();
            } else {
                this.container.AutoSizeAxes = Axes.None;
                this.container.Height = 0;
                this.header.TryEndLongTitleScroll();
            }
        }
    }
}
