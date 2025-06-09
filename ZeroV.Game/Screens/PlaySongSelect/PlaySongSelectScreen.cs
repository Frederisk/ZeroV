using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using osu.Framework.Allocation;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Rendering;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Screens;

using osuTK;

using ZeroV.Game.Data;
using ZeroV.Game.Elements.Buttons;
using ZeroV.Game.Objects;
using ZeroV.Game.Screens.Gameplay;
using ZeroV.Game.Screens.PlaySongSelect.ListItems;
using ZeroV.Game.Utils.ExternalLoader;

namespace ZeroV.Game.Screens.PlaySongSelect;

[Cached]
public partial class PlaySongSelectScreen : BaseUserInterfaceScreen {
    private Sprite background = null!;
    private FillFlowContainer<TrackInfoListItem> container = null!;
    private TextureLoader? textureLoader;

    private BasicScrollContainer<FillFlowContainer<ResultInfoListItem>> scoringRankListScroller = null!;
    private FillFlowContainer<ResultInfoListItem> scoringRankList = null!;
    //private Container miniInfoDisplay = null!;

    [Resolved]
    private TrackInfoProvider beatmapWrapperProvider { get; set; } = null!;

    [Resolved]
    private ResultInfoProvider resultInfoProvider { get; set; } = null!;

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
        this.container = new FillFlowContainer<TrackInfoListItem>() {
            RelativeSizeAxes = Axes.X,
            AutoSizeAxes = Axes.Y,
            Direction = FillDirection.Vertical,
            Spacing = new Vector2(0, 10),
        };
        this.beatmapWrapperProvider.Get()?
            .OrderBy(i => i.Title)
            .ForEach(trackInfo => {
                this.container.Add(new TrackInfoListItem(trackInfo));
            });

        this.scoringRankList = new() {
            AutoSizeAxes = Axes.Y,
            RelativeSizeAxes = Axes.X,
        };
        this.scoringRankListScroller = new() {
            Anchor = Anchor.BottomCentre,
            Origin = Anchor.BottomCentre,
            RelativeSizeAxes = Axes.Both,
            Size = new(0.95f, 0.45f),
            Child = this.scoringRankList,
        };

        this.InternalChildren = [
            this.background,
            new BackButton(this) {
                Anchor = Anchor.TopLeft,
                Origin = Anchor.TopLeft,
            },
            new BasicScrollContainer<FillFlowContainer<TrackInfoListItem>>(Direction.Vertical) {
                Anchor = Anchor.CentreRight,
                Origin = Anchor.CentreRight,
                RelativeSizeAxes = Axes.Both,
                Width = 0.5f,
                Padding = new MarginPadding(32),
                Child = this.container,
            },
            new Container{
                Anchor = Anchor.CentreLeft,
                Origin = Anchor.CentreLeft,
                RelativeSizeAxes = Axes.Both,
                Width = 0.5f,
                Padding = new MarginPadding(32),
                Children = [
                    this.scoringRankListScroller,
                    // TODO: add a bulletin board.
                ],
            },
        ];
    }

    public override void OnResuming(ScreenTransitionEvent e) {
        base.OnResuming(e);
        this.updateInfoDisplay();
    }

    private TrackInfoListItem? expandedItem;
    private MapInfoListItem? selectedItem;

    public void OnSelect(MapInfoListItem item) {
        if (this.selectedItem == item) {
            return;
        }
        this.selectedItem?.OnSelectCancel();
        this.selectedItem = item;
        this.updateInfoDisplay();
    }

    private void updateInfoDisplay() {
        TrackInfo? trackInfo = this.expandedItem?.TrackInfo;
        MapInfo? mapInfo = this.selectedItem?.MapInfo;
        if (trackInfo is null || mapInfo is null) {
            return;
        }

        IReadOnlyList<ResultInfo> resultList = this.resultInfoProvider.Get() ?? [];
        IOrderedEnumerable<ResultInfo> orderedResultList =
            from result in resultList
            where result.UUID == trackInfo.UUID
               && result.GameVersion == trackInfo.GameVersion
               && result.Index == mapInfo.Index
            orderby result.Scoring descending
            select result;

        this.scoringRankList.Clear();
        foreach (ResultInfo resultInfo in orderedResultList.Take(10)) {
            this.scoringRankList.Add(new ResultInfoListItem(resultInfo));
        }
    }

    public void OnExpanded(TrackInfoListItem item) {
        if (this.expandedItem == item) {
            return;
        }
        if (this.expandedItem is not null) {
            this.expandedItem.IsExpanded = false;
        }
        // TODO: Load a simple icon instead of a background
        FileInfo? file = item.TrackInfo.BackgroundFile;
        if (file is not null) {
            TextureLoader? old = this.textureLoader;
            this.textureLoader = new(file, this.renderer);
            this.background.Texture = this.textureLoader.Texture;
            old?.Dispose();
        } else { // file is null
            this.background.Texture = null;
            this.textureLoader?.Dispose();
        }
        this.expandedItem = item;
        // TODO: Which one to select?
        item.SelectFirst();
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
