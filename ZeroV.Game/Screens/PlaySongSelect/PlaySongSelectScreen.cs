using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using osu.Framework.Allocation;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Rendering;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Screens;

using osuTK;

using ZeroV.Game.Data;
using ZeroV.Game.Elements.Buttons;
using ZeroV.Game.Graphics;
using ZeroV.Game.Graphics.Shapes;
using ZeroV.Game.Objects;
using ZeroV.Game.Screens.Gameplay;
using ZeroV.Game.Screens.PlaySongSelect.ListItems;
using ZeroV.Game.Utils.ExternalLoader;

namespace ZeroV.Game.Screens.PlaySongSelect;

[Cached]
public partial class PlaySongSelectScreen : BaseUserInterfaceScreen {
    private Box defaultBackground = null!;
    private Sprite background = null!;
    private FillFlowContainer<TrackInfoListItem> trackInfoListItemContainer = null!;
    private TextureLoader? textureLoader;

    private ScrollContainer<FillFlowContainer<ResultInfoListItem>> scoringRankListScroller = null!;

    private FillFlowContainer<ResultInfoListItem> scoringRankList = null!;
    //private Container miniInfoDisplay = null!;

    private Container emptyLeaderboardPlaceholder = null!;

    [Resolved]
    private TrackInfoProvider beatmapWrapperProvider { get; set; } = null!;

    [Resolved]
    private ResultInfoProvider resultInfoProvider { get; set; } = null!;

    [Resolved]
    private IRenderer renderer { get; set; } = null!;

    [BackgroundDependencyLoader]
    private void load() {
        this.RelativeSizeAxes = Axes.Both;
        this.defaultBackground = new Box {
            RelativeSizeAxes = Axes.Both,
            Colour = Colour4.White,
        };
        this.background = new Sprite {
            RelativeSizeAxes = Axes.Both,
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            FillMode = FillMode.Fill,
        };
        this.trackInfoListItemContainer = new FillFlowContainer<TrackInfoListItem> {
            RelativeSizeAxes = Axes.X,
            AutoSizeAxes = Axes.Y,
            Direction = FillDirection.Vertical,
            Spacing = new Vector2(0, 6),
        };
        this.beatmapWrapperProvider.Get()?
            .OrderBy(i => i.Title)
            .ForEach(trackInfo => {
                this.trackInfoListItemContainer.Add(new TrackInfoListItem(trackInfo));
            });
        this.scoringRankList = new FillFlowContainer<ResultInfoListItem> {
            AutoSizeAxes = Axes.Y,
            RelativeSizeAxes = Axes.X,
            Direction = FillDirection.Vertical,
            Spacing = new Vector2(0, 2),
        };
        this.scoringRankListScroller = new BasicScrollContainer<FillFlowContainer<ResultInfoListItem>> {
            Anchor = Anchor.TopLeft,
            Origin = Anchor.TopLeft,
            RelativeSizeAxes = Axes.Both,
            Size = new Vector2(0.95f, 0.45f),
            Child = this.scoringRankList,
        };
        this.emptyLeaderboardPlaceholder = new Container {
            RelativeSizeAxes = Axes.Both,
            Child = new FillFlowContainer {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                AutoSizeAxes = Axes.Both,
                Direction = FillDirection.Vertical,
                //Spacing = new Vector2(8),
                Children = [
                    new Diamond {
                        Anchor = Anchor.TopCentre,
                        Origin = Anchor.TopCentre,
                        Size = new Vector2(16),
                        Margin = new MarginPadding { Horizontal = 8 },
                        Colour = Colour4.Cyan,
                    },
                    new ZeroVSpriteText {
                        Anchor = Anchor.TopCentre,
                        Origin = Anchor.TopCentre,
                        Colour = Colour4.Black,
                        FontSize = 14,
                        Font = FontUsage.Default.With(weight: "Bold"),
                        Text = "NO RECORDS YET",
                    },
                    new ZeroVSpriteText {
                        Anchor = Anchor.TopCentre,
                        Origin = Anchor.TopCentre,
                        Colour = Colour4.Black,
                        FontSize = 12,
                        Text = "Play this chart to set your high score!",
                    },
                ],
            },
        };

        this.InternalChildren = [
            // Background Base Layer
            this.defaultBackground,
            this.background,
            // Bright Frosted Ambient Gradient Overlay
            new Box {
                RelativeSizeAxes = Axes.Both,
                Colour = ColourInfo.GradientHorizontal(
                    Colour4.LightBlue.Opacity(0.25f),
                    Colour4.Cyan.Opacity(0.35f)
                ),
                //Colour = Colour4.White.Opacity(0.01f),
            },
            new Box {
                RelativeSizeAxes = Axes.Both,
                Colour = ColourInfo.GradientVertical(
                    Colour4.White.Opacity(0.4f),
                    Colour4.SeaGreen.Opacity(0.22f)
                ),
                //Colour = Colour4.White.Opacity(0.01f),
            },
            // Top Header Bar
            new Container {
                Anchor = Anchor.TopLeft,
                Origin = Anchor.TopLeft,
                RelativeSizeAxes = Axes.X,
                Height = 50,
                Children = [
                    new Box {
                        RelativeSizeAxes = Axes.Both,
                        Colour = Colour4.White.Opacity(0.7f),
                    },
                    new Box {
                        Anchor = Anchor.BottomLeft,
                        Origin = Anchor.BottomLeft,
                        Colour = Colour4.Cyan.Opacity(0.6f),
                        Height = 2f,
                        RelativeSizeAxes = Axes.X,
                    },
                    new BackButton(this) {
                        Anchor = Anchor.CentreLeft,
                        Origin = Anchor.CentreLeft,
                        Margin = new MarginPadding { Left = 24 },
                    },
                ],
            },
            // Main Content Area
            new Container {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                RelativeSizeAxes = Axes.Both,
                Padding = new MarginPadding { Top = 80, Bottom = 60, Left = 32, Right = 32 },
                Children = [
                    // Left Column: Song Details & Leaderboard
                    new Container {
                        Anchor = Anchor.CentreLeft,
                        Origin = Anchor.CentreLeft,
                        RelativeSizeAxes = Axes.Both,
                        Width = 0.46f,
                        Children = [
                            // Top: Detail Preview Card
                            //new Box {
                            //    // TODO: Detail Preview Card
                            //    Anchor = Anchor.TopCentre,
                            //    Origin = Anchor.TopCentre,
                            //    Colour = Colour4.Red,
                            //    RelativeSizeAxes = Axes.X,
                            //    Height = 300,
                            //},
                            new SongDetailPreviewCard {
                                Anchor = Anchor.TopCentre,
                                Origin = Anchor.TopCentre,
                                RelativeSizeAxes = Axes.X,
                                Height = 300,
                            },
                            // Bottom: Leaderboard Panel
                            new Container {
                                Anchor = Anchor.BottomCentre,
                                Origin = Anchor.BottomCentre,
                                RelativeSizeAxes = Axes.X,
                                Height = 300,
                                Masking = true,
                                BorderThickness = 1.5f,
                                BorderColour = Colour4.Cyan,
                                Children = [
                                    new Box {
                                        RelativeSizeAxes = Axes.Both,
                                        Colour = Colour4.White,
                                    },
                                    new FillFlowContainer {
                                        RelativeSizeAxes = Axes.Both,
                                        Direction = FillDirection.Vertical,
                                        Padding = new MarginPadding(16),
                                        Spacing = new Vector2(0, 10),
                                        // Leaderboard Header
                                        Children = [
                                            new FillFlowContainer {
                                                RelativeSizeAxes = Axes.X,
                                                AutoSizeAxes = Axes.Y,
                                                Direction = FillDirection.Horizontal,
                                                //Spacing = new Vector2(8, 0),
                                                Children = [
                                                    new Diamond {
                                                        Anchor = Anchor.CentreLeft,
                                                        Origin = Anchor.CentreLeft,
                                                        Size = new Vector2(10),
                                                        Colour = Colour4.Cyan,
                                                        Margin = new MarginPadding { Horizontal = 8 },
                                                    },
                                                    new ZeroVSpriteText {
                                                        Anchor = Anchor.CentreLeft,
                                                        Origin = Anchor.CentreLeft,
                                                        FontSize = 15,
                                                        Font = FontUsage.Default.With(weight: "Bold"),
                                                        Colour = Colour4.Black,
                                                        Text = "TOP RECORDS",
                                                    },
                                                ],
                                            },
                                            // Divider
                                            new Box {
                                                RelativeSizeAxes = Axes.X,
                                                Height = 1.5f,
                                                Colour = Colour4.Cyan,
                                            },
                                            // List Scroll Container
                                            new Container {
                                                RelativeSizeAxes = Axes.Both,
                                                Child = this.scoringRankListScroller,
                                            },
                                        ],
                                    },
                                    // Empty state placeholder
                                    this.emptyLeaderboardPlaceholder,
                                ],
                            },
                        ],
                    },
                    // Right Column: Track Collection List
                    new Container {
                        Anchor = Anchor.CentreRight,
                        Origin = Anchor.CentreRight,
                        RelativeSizeAxes = Axes.Both,
                        Width = 0.51f,
                        //Padding = new MarginPadding { Left = 16f },
                        //Padding = new MarginPadding(16),
                        Children = [
                            // Header Bar for Right Column
                            new Container {
                                Anchor = Anchor.TopLeft,
                                Origin = Anchor.TopLeft,
                                RelativeSizeAxes = Axes.X,
                                Height = 36,
                                Children = [
                                    new FillFlowContainer {
                                        Anchor = Anchor.CentreLeft,
                                        Origin = Anchor.CentreLeft,
                                        AutoSizeAxes = Axes.Both,
                                        Direction = FillDirection.Horizontal,
                                        Children = [
                                            new Diamond {
                                                Anchor = Anchor.CentreLeft,
                                                Origin = Anchor.CentreLeft,
                                                Size = new Vector2(10),
                                                Margin = new MarginPadding { Horizontal = 8 },
                                                Colour = Colour4.Cyan,
                                            },
                                            new ZeroVSpriteText {
                                                Anchor = Anchor.CentreLeft,
                                                Origin = Anchor.CentreLeft,
                                                Colour = Colour4.Black,
                                                FontSize = 16,
                                                Font = FontUsage.Default.With(weight: "Bold"),
                                                Text = $"TRACK COLLECTION",
                                            },
                                        ],
                                    },
                                    new Box {
                                        Anchor = Anchor.BottomLeft,
                                        Origin = Anchor.BottomLeft,
                                        RelativeSizeAxes = Axes.X,
                                        Height = 1.5f,
                                        Colour = Colour4.Cyan,
                                    },
                                ],
                            },
                            new BasicScrollContainer<FillFlowContainer<TrackInfoListItem>>(Direction.Vertical) {
                                Anchor = Anchor.BottomLeft,
                                Origin = Anchor.BottomLeft,
                                RelativeSizeAxes = Axes.Both,
                                Height = 0.93f,
                                Child = this.trackInfoListItemContainer,
                            },
                        ],
                    },
                ],
            },
            //new FlyoutButton() {
            //    Anchor = Anchor.BottomLeft,
            //    Origin = Anchor.BottomLeft,
            //    //AutoSizeAxes = Axes.Both,
            //    //Padding = new MarginPadding(32),
            //    Text = "More Action...",
            //    Direction = FlyoutButton.FlyoutDirection.Up,
            //    MenuItemsContainer = new FillFlowContainer() {
            //        AutoSizeAxes = Axes.Both,
            //        Direction = FillDirection.Vertical,
            //        Spacing = new Vector2(0, 8),
            //        Children = [
            //            new BasicButton() {
            //                AutoSizeAxes = Axes.Both,
            //                Text = "Open Song Folder",
            //                //Action = () => {
            //                //},
            //            },
            //            new BasicButton() {
            //                AutoSizeAxes = Axes.Both,
            //                Text = "View Online",
            //                //Action = () => {
            //                //},
            //            },
            //        ],
            //    },
            //    //Action = () => {
            //    //},
            //},
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
            this.emptyLeaderboardPlaceholder.FadeIn(150, Easing.In);
            this.scoringRankList.Clear();
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
        List<ResultInfo> topResultInfos = orderedResultList.Take(10).ToList();

        if (topResultInfos.Count > 0) {
            this.emptyLeaderboardPlaceholder.FadeOut(100, Easing.Out);

            for (Int32 i = 0; i < topResultInfos.Count; i++) {
                this.scoringRankList.Add(new ResultInfoListItem(topResultInfos[i], i + 1));
            }
            //foreach (ResultInfo resultInfo in topResultInfos) {
            //    this.scoringRankList.Add(new ResultInfoListItem(resultInfo));
            //}
        } else {
            this.emptyLeaderboardPlaceholder.FadeIn(150, Easing.In);
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
            this.textureLoader = new TextureLoader(file, this.renderer);
            this.background.Texture = this.textureLoader.Texture;
            old?.Dispose();
        } else {
            // file is null
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
        this.Push(new GameLoader(() => new GameplayScreen(trackInfo, mapInfo)));
    }

    protected override void Dispose(Boolean disposing) {
        base.Dispose(disposing);

        if (disposing) {
            this.textureLoader?.Dispose();
        }
    }
}
