using osu.Framework.Allocation;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Graphics;
using osu.Framework.Screens;

using osuTK;

using ZeroV.Game.Objects;
using ZeroV.Game.Data;
using ZeroV.Game.Elements.ListItems;

namespace ZeroV.Game.Screens;

[Cached]
public partial class PlaySongSelectScreen : Screen {
    private Sprite background = null!;
    private FillFlowContainer container = null!;

    [Resolved]
    private TrackInfoProvider beatmapWrapperProvider { get; set; } = null!;

    [Resolved]
    private LargeTextureStore textureStore { get; set; } = null!;

    [BackgroundDependencyLoader]
    private async void load() {
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

        foreach (TrackInfo item in await this.beatmapWrapperProvider.GetAsync() ?? []) {
            this.container.Add(new TrackInfoListItem(item));
        }

        var child = new BasicScrollContainer() {
            RelativeSizeAxes = Axes.Both,
            Child = this.container
        };

        // TODO: make sure this is the correct way to add a child
        this.Schedule(() => this.AddInternal(child));
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
        //TODO: Confirm select
    }
}
