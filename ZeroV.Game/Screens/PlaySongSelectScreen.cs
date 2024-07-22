using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using osu.Framework.Allocation;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Graphics;
using osu.Framework.Screens;

using osuTK;

using ZeroV.Game.Data;
using ZeroV.Game.Elements.ListItems;
using ZeroV.Game.Elements.Buttons;
using ZeroV.Game.Objects;

namespace ZeroV.Game.Screens;

[Cached]
[LongRunningLoad]
public partial class PlaySongSelectScreen : Screen {
    private Sprite background = null!;
    private FillFlowContainer container = null!;

    [Resolved]
    private TrackInfoProvider beatmapWrapperProvider { get; set; } = null!;

    [Resolved]
    private LargeTextureStore textureStore { get; set; } = null!;

    [BackgroundDependencyLoader]
    private async Task load() {
        this.RelativeSizeAxes = Axes.Both;

        this.background = new Sprite() {
            RelativeSizeAxes = Axes.Both,
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            FillMode = FillMode.Fill
        };
        this.AddInternal(this.background);

        this.container = new FillFlowContainer() {
            RelativeSizeAxes = Axes.X,
            AutoSizeAxes = Axes.Y,
            Direction = FillDirection.Vertical,
            Spacing = new Vector2(0, 10),
            Padding = new MarginPadding(10)
        };

        IReadOnlyList<TrackInfo> trackInfos = await this.beatmapWrapperProvider.GetAsync() ?? [];
        IEnumerable<TrackInfo> trackInfoSort = trackInfos.OrderBy(i => i.Title);
        foreach (TrackInfo item in trackInfoSort) {
            this.container.Add(new TrackInfoListItem(item));
        }

        var child = new BasicScrollContainer() {
            RelativeSizeAxes = Axes.Both,
            Child = this.container
        };

        var grid = new GridContainer()
        {
            RelativeSizeAxes = Axes.Both,
            RowDimensions = [
               new Dimension(GridSizeMode.AutoSize),
               new Dimension(GridSizeMode.Relative, size: 1)
               ],
            Content = new Drawable[][] {
                [
                    new BackButton(this) {
                        Height = 100,
                        Width = 100,
                        Text = "< Back",
                    }
                ],
                [ child ]
            }
        };

        this.AddInternal(grid);
    }

    private MapInfoListItem? selectedItem;

    public void OnSelect(MapInfoListItem item) {
        this.selectedItem?.OnSelectCancel();
        this.selectedItem = item;
    }

    private TrackInfoListItem? expandedItem;

    public void OnExpanded(TrackInfoListItem item) {
        if (this.expandedItem != item && this.expandedItem != null) {
            this.expandedItem.IsExpanded = false;
        }
        this.expandedItem = item;

        //var trackInfo = item.TrackInfo;
        //TODO: TrackInfo Background
        Texture? texture = this.textureStore.Get("test-background.webp");
        this.background.Texture = texture;
    }

    public void ConfirmSelect() {
        //this.Push(new GameplayScreen(this.selectedItem.));
        //TODO: Confirm select
    }
}
