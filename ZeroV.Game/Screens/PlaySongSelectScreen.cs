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
    private void load() {
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

        //IReadOnlyList<TrackInfo> trackInfos = await this.beatmapWrapperProvider.GetAsync() ?? [];
        //this.beatmapWrapperProvider.GetAsync
        IReadOnlyList<TrackInfo> trackInfos =  this.beatmapWrapperProvider.Get() ?? new List<TrackInfo>();


        IEnumerable<TrackInfo> trackInfoSort = trackInfos.OrderBy(i => i.Title);
        foreach (TrackInfo item in trackInfoSort) {
            this.container.Add(new TrackInfoListItem(item));
        }

        this.InternalChildren = [
            new BackButton(this) {
                Anchor = Anchor.TopLeft,
                Origin = Anchor.TopLeft,
                Height = 48,
                Width = 96,
                Text = "< Back",
            },
            new BasicScrollContainer() {
                Anchor = Anchor.CentreRight,
                Origin = Anchor.CentreRight,
                RelativeSizeAxes = Axes.Both,
                Width = 0.5f,
                Child = this.container
            },
        ];
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
        var wrapper = BeatmapWrapper.Create(this.expandedItem!.TrackInfo.BeatmapFile);
        Beatmap beatmap = wrapper.GetBeatmapAt(this.selectedItem!.Index);
        var playScreen = new GameplayScreen(beatmap, this.expandedItem!.TrackInfo.TrackFile);
        this.Push(playScreen);
    }
}
