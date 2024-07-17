using System;

using osu.Framework.Allocation;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics;

using osuTK;

using ZeroV.Game.Objects;

namespace ZeroV.Game.Elements;

[Cached]
public partial class TrackInfoListItem(TrackInfo info) : CompositeDrawable {
    private Boolean isExpanded;
    private FillFlowContainer container = null!;

    public TrackInfo TrackInfo => info;

    [BackgroundDependencyLoader]
    private void load() {
        this.Masking = true;
        this.BorderColour = Colour4.White;
        this.RelativeSizeAxes = Axes.X;
        this.AutoSizeAxes = Axes.Y;

        this.container = new FillFlowContainer() {
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
                new TrackInfoListItemHeader(),
                    this.container
            ]
        });

        foreach (MapInfo item in info.Maps) {
            this.container.Add(new MapInfoListItem(item));
        }
    }

    public Boolean IsExpanded {
        get => this.isExpanded;
        set {
            if (value) {
                this.container.AutoSizeAxes = Axes.Y;
            } else {
                this.container.AutoSizeAxes = Axes.None;
                this.container.Height = 0;
            }
            this.isExpanded = value;
        }
    }
}
