using System;
using System.Collections.Generic;
using System.Linq;

using osu.Framework.Allocation;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics;
using osu.Framework.Screens;

using osuTK;

using ZeroV.Game.Data;
using ZeroV.Game.Elements.ListItems;
using ZeroV.Game.Elements.Buttons;
using ZeroV.Game.Objects;
using ZeroV.Game.Screens.Gameplay;
using osu.Framework.Graphics.Rendering;
using System.IO;
using ZeroV.Game.Utils.ExternalLoader;

namespace ZeroV.Game.Screens;

[Cached]
[LongRunningLoad]
public partial class PlaySongSelectScreen : Screen {
    private Sprite background = null!;
    private FillFlowContainer container = null!;
    private TextureLoader? textureLoader;

    [Resolved]
    private TrackInfoProvider beatmapWrapperProvider { get; set; } = null!;

    [Resolved]
    private ResultInfoProvider resultInfoProvider { get; set; } = null!;

    //[Resolved]
    //private LargeTextureStore textureStore { get; set; } = null!;

    [Resolved]
    private IRenderer renderer { get; set; } = null!;

    [BackgroundDependencyLoader]
    private void load() {
        this.RelativeSizeAxes = Axes.Both;

        this.background = new Sprite() {
            RelativeSizeAxes = Axes.Both,
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            FillMode = FillMode.Fill
        };

        this.container = new FillFlowContainer() {
            RelativeSizeAxes = Axes.X,
            AutoSizeAxes = Axes.Y,
            Direction = FillDirection.Vertical,
            Spacing = new Vector2(0, 10),
            Padding = new MarginPadding(10)
        };

        IReadOnlyList<TrackInfo> trackInfos = this.beatmapWrapperProvider.Get() ?? [];
        IReadOnlyList<ResultInfo> resultInfos = this.resultInfoProvider.Get() ?? [];

        IEnumerable<TrackInfo> trackInfoSort = trackInfos.OrderBy(i => i.Title);

        foreach (TrackInfo item in trackInfoSort) {
            this.container.Add(new TrackInfoListItem(item));
        }

        this.InternalChildren = [
            this.background,
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
        if (this.expandedItem != item) {
            if (this.expandedItem is not null) {
                this.expandedItem.IsExpanded = false;
            }
            // TODO: TrackInfo Background
            FileInfo? file = item.TrackInfo.BackgroundFile;
            if (file is not null) {
                TextureLoader? old = this.textureLoader;
                this.textureLoader = new(file, this.renderer);
                this.background.Texture = this.textureLoader.Texture;
                old?.Dispose();
                //NativeStorage storage = new(file.Directory!.FullName);
                //using StorageBackedResourceStore store = new(storage);
                //using TextureLoaderStore a = new(store);
                ////using TextureStore b = new(this.renderer, a);
                //using LargeTextureStore c = new LargeTextureStore(this.renderer, a);
                //Texture? old = this.background.Texture;
                //this.background.Texture = c.Get(file.Name);
                //old?.Dispose();
                //old = null;
            } else {
                this.background.Texture = null;
                this.textureLoader?.Dispose();
                //this.background.Texture?.Dispose();
                //this.background.Texture = null;
            }
        }

        this.expandedItem = item;
    }

    public void ConfirmSelect() {
        TrackInfo trackInfo = this.expandedItem!.TrackInfo;
        MapInfo mapInfo = this.selectedItem!.MapInfo;
        this.Push(new GameLoader(() => {
            return new GameplayScreen(trackInfo, mapInfo);
        }));
    }

    protected override void Dispose(Boolean disposing) {
        base.Dispose(disposing);
        if (disposing) {
            this.textureLoader?.Dispose();
        }
    }
}
