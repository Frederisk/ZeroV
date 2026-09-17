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
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Screens;

using osuTK;

using ZeroV.Game.Data;
using ZeroV.Game.Elements.Buttons;
using ZeroV.Game.Graphics;
using ZeroV.Game.Graphics.Shapes;
using ZeroV.Game.Objects;
using ZeroV.Game.Screens.Gameplay;
using ZeroV.Game.Screens.PlaySongSelect.ListItems;
using ZeroV.Game.Utils;
using ZeroV.Game.Utils.ExternalLoader;

namespace ZeroV.Game.Screens.PlaySongSelect;

[Cached]
public partial class PlaySongSelectScreen : BaseUserInterfaceScreen {
    private Sprite background = null!;
    private Box defaultBackgroundBox = null!;
    private FillFlowContainer<TrackInfoListItem> container = null!;
    private TextureLoader? textureLoader;

    private BasicScrollContainer<FillFlowContainer<ResultInfoListItem>> scoringRankListScroller = null!;
    private FillFlowContainer<ResultInfoListItem> scoringRankList = null!;
    private Container emptyLeaderboardPlaceholder = null!;
    private SongDetailPreviewCard detailCard = null!;

    [Resolved]
    private TrackInfoProvider beatmapWrapperProvider { get; set; } = null!;

    [Resolved]
    private ResultInfoProvider resultInfoProvider { get; set; } = null!;

    [Resolved]
    private IRenderer renderer { get; set; } = null!;

    [BackgroundDependencyLoader]
    private void load() {
        this.RelativeSizeAxes = Axes.Both;

        this.background = new Sprite {
            RelativeSizeAxes = Axes.Both,
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            FillMode = FillMode.Fill,
        };

        this.container = new FillFlowContainer<TrackInfoListItem> {
            RelativeSizeAxes = Axes.X,
            AutoSizeAxes = Axes.Y,
            Direction = FillDirection.Vertical,
            Spacing = new Vector2(0, 6),
        };

        var trackList = this.beatmapWrapperProvider.Get()?.OrderBy(i => i.Title).ToList() ?? [];
        foreach (TrackInfo trackInfo in trackList) {
            this.container.Add(new TrackInfoListItem(trackInfo));
        }

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
            Child = this.scoringRankList,
        };

        this.InternalChildren = [
            // Background Base Layer (VOEZ Bright Aesthetic)
            this.defaultBackgroundBox = new Box {
                RelativeSizeAxes = Axes.Both,
                Colour = ZeroVColour.BgBase,
            },
            this.background,
            // Bright Frosted Ambient Gradient Overlay
            new Box {
                RelativeSizeAxes = Axes.Both,
                Colour = ColourInfo.GradientHorizontal(
                    ZeroVColour.BgLight.Opacity(0.85f),
                    ZeroVColour.BgBase.Opacity(0.65f)
                ),
            },
            new Box {
                RelativeSizeAxes = Axes.Both,
                Colour = ColourInfo.GradientVertical(
                    Colour4.White.Opacity(0.30f),
                    ZeroVColour.BgBase.Opacity(0.50f)
                ),
            },

            // Top Header Bar
            new Container {
                Anchor = Anchor.TopLeft,
                Origin = Anchor.TopLeft,
                RelativeSizeAxes = Axes.X,
                Height = 72,
                Children = [
                    new Box {
                        RelativeSizeAxes = Axes.Both,
                        Colour = ZeroVColour.BgHeader,
                    },
                    new Box {
                        Anchor = Anchor.BottomLeft,
                        Origin = Anchor.BottomLeft,
                        RelativeSizeAxes = Axes.X,
                        Height = 2,
                        Colour = ZeroVColour.Cyan.Opacity(0.6f),
                    },
                    new BackButton(this) {
                        Anchor = Anchor.CentreLeft,
                        Origin = Anchor.CentreLeft,
                        Margin = new MarginPadding { Left = 24 },
                    },
                    new FillFlowContainer {
                        Anchor = Anchor.CentreLeft,
                        Origin = Anchor.CentreLeft,
                        AutoSizeAxes = Axes.Both,
                        Direction = FillDirection.Horizontal,
                        Spacing = new Vector2(12, 0),
                        Margin = new MarginPadding { Left = 190 },
                        Children = [
                            new Diamond {
                                Anchor = Anchor.CentreLeft,
                                Origin = Anchor.CentreLeft,
                                Size = new Vector2(14),
                                Colour = ZeroVColour.Cyan,
                            },
                            new FillFlowContainer {
                                Anchor = Anchor.CentreLeft,
                                Origin = Anchor.CentreLeft,
                                AutoSizeAxes = Axes.Both,
                                Direction = FillDirection.Vertical,
                                Spacing = new Vector2(0, 2),
                                Children = [
                                    new ZeroVSpriteText {
                                        Text = "MUSIC SELECT",
                                        Colour = ZeroVColour.TextDark,
                                        FontSize = 22,
                                        Font = FontUsage.Default.With(weight: "Bold"),
                                    },
                                    new ZeroVSpriteText {
                                        Text = "CHOOSE A TRACK TO PLAY",
                                        Colour = ZeroVColour.TextLight,
                                        FontSize = 11,
                                        Font = FontUsage.Default.With(weight: "Bold"),
                                    },
                                ],
                            },
                        ],
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
                            this.detailCard = new SongDetailPreviewCard {
                                Anchor = Anchor.TopCentre,
                                Origin = Anchor.TopCentre,
                                RelativeSizeAxes = Axes.X,
                                Height = 220,
                            },
                            // Bottom: Leaderboard Panel
                            new Container {
                                Anchor = Anchor.BottomCentre,
                                Origin = Anchor.BottomCentre,
                                RelativeSizeAxes = Axes.X,
                                Height = 320,
                                Masking = true,
                                CornerRadius = 0,
                                BorderThickness = 1.5f,
                                BorderColour = ZeroVColour.Cyan.Opacity(0.5f),
                                Children = [
                                    new Box {
                                        RelativeSizeAxes = Axes.Both,
                                        Colour = ZeroVColour.BgPanel,
                                    },
                                    new FillFlowContainer {
                                        RelativeSizeAxes = Axes.Both,
                                        Direction = FillDirection.Vertical,
                                        Padding = new MarginPadding(16),
                                        Spacing = new Vector2(0, 10),
                                        Children = [
                                            // Leaderboard Header
                                            new FillFlowContainer {
                                                RelativeSizeAxes = Axes.X,
                                                AutoSizeAxes = Axes.Y,
                                                Direction = FillDirection.Horizontal,
                                                Spacing = new Vector2(8, 0),
                                                Children = [
                                                    new Diamond {
                                                        Anchor = Anchor.CentreLeft,
                                                        Origin = Anchor.CentreLeft,
                                                        Size = new Vector2(10),
                                                        Colour = ZeroVColour.Cyan,
                                                    },
                                                    new ZeroVSpriteText {
                                                        Anchor = Anchor.CentreLeft,
                                                        Origin = Anchor.CentreLeft,
                                                        Text = "TOP RECORDS",
                                                        Colour = ZeroVColour.TextDark,
                                                        FontSize = 15,
                                                        Font = FontUsage.Default.With(weight: "Bold"),
                                                    },
                                                ],
                                            },
                                            // Divider
                                            new Box {
                                                RelativeSizeAxes = Axes.X,
                                                Height = 1,
                                                Colour = ZeroVColour.BorderLight,
                                            },
                                            // List Scroll Container
                                            new Container {
                                                RelativeSizeAxes = Axes.Both,
                                                Child = this.scoringRankListScroller,
                                            },
                                        ],
                                    },
                                    // Empty state placeholder
                                    this.emptyLeaderboardPlaceholder = new Container {
                                        RelativeSizeAxes = Axes.Both,
                                        Children = [
                                            new FillFlowContainer {
                                                Anchor = Anchor.Centre,
                                                Origin = Anchor.Centre,
                                                AutoSizeAxes = Axes.Both,
                                                Direction = FillDirection.Vertical,
                                                Spacing = new Vector2(0, 8),
                                                Children = [
                                                    new Diamond {
                                                        Anchor = Anchor.TopCentre,
                                                        Origin = Anchor.TopCentre,
                                                        Size = new Vector2(16),
                                                        Colour = ZeroVColour.Cyan.Opacity(0.4f),
                                                    },
                                                    new ZeroVSpriteText {
                                                        Anchor = Anchor.TopCentre,
                                                        Origin = Anchor.TopCentre,
                                                        Text = "NO RECORDS YET",
                                                        Colour = ZeroVColour.TextMedium,
                                                        FontSize = 14,
                                                        Font = FontUsage.Default.With(weight: "Bold"),
                                                    },
                                                    new ZeroVSpriteText {
                                                        Anchor = Anchor.TopCentre,
                                                        Origin = Anchor.TopCentre,
                                                        Text = "Play this chart to set your high score!",
                                                        Colour = ZeroVColour.TextLight,
                                                        FontSize = 12,
                                                    },
                                                ],
                                            },
                                        ],
                                    },
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
                        Padding = new MarginPadding { Left = 16 },
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
                                        Spacing = new Vector2(8, 0),
                                        Children = [
                                            new Diamond {
                                                Anchor = Anchor.CentreLeft,
                                                Origin = Anchor.CentreLeft,
                                                Size = new Vector2(10),
                                                Colour = ZeroVColour.Cyan,
                                            },
                                            new ZeroVSpriteText {
                                                Anchor = Anchor.CentreLeft,
                                                Origin = Anchor.CentreLeft,
                                                Text = $"TRACK COLLECTION  ({trackList.Count})",
                                                Colour = ZeroVColour.TextDark,
                                                FontSize = 15,
                                                Font = FontUsage.Default.With(weight: "Bold"),
                                            },
                                        ],
                                    },
                                    new Box {
                                        Anchor = Anchor.BottomLeft,
                                        Origin = Anchor.BottomLeft,
                                        RelativeSizeAxes = Axes.X,
                                        Height = 1,
                                        Colour = ZeroVColour.BorderLight,
                                    },
                                ],
                            },
                            // Track List Scroller
                            new BasicScrollContainer<FillFlowContainer<TrackInfoListItem>>(Direction.Vertical) {
                                Anchor = Anchor.BottomLeft,
                                Origin = Anchor.BottomLeft,
                                RelativeSizeAxes = Axes.Both,
                                Height = 0.93f,
                                Child = this.container,
                            },
                        ],
                    },
                ],
            },

            // Bottom Left: Flyout Actions
            new Container {
                Anchor = Anchor.BottomLeft,
                Origin = Anchor.BottomLeft,
                AutoSizeAxes = Axes.Both,
                Padding = new MarginPadding { Left = 32, Bottom = 16 },
                Child = new FlyoutButton {
                    Text = "⚙  MORE ACTIONS...",
                    Direction = FlyoutButton.FlyoutDirection.Up,
                    MenuItemsContainer = new FillFlowContainer {
                        AutoSizeAxes = Axes.Both,
                        Direction = FillDirection.Vertical,
                        Spacing = new Vector2(0, 6),
                        Children = [
                            this.createActionButton("Open Song Folder"),
                            this.createActionButton("View Online"),
                        ],
                    },
                },
            },
        ];
    }

    private BasicButton createActionButton(String text, Action? action = null) => new() {
        Size = new Vector2(180, 38),
        Masking = true,
        CornerRadius = 0,
        BorderThickness = 1,
        BorderColour = ZeroVColour.Cyan.Opacity(0.5f),
        BackgroundColour = ZeroVColour.BgCard,
        HoverColour = ZeroVColour.Cyan.Opacity(0.2f),
        FlashColour = ZeroVColour.CyanBright,
        Text = text,
        Action = action,
    };

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
            this.detailCard.ClearDisplay();
            this.emptyLeaderboardPlaceholder.FadeIn(150, Easing.OutQuint);
            this.scoringRankList.Clear();
            return;
        }

        this.detailCard.UpdateDisplay(trackInfo, mapInfo);

        IReadOnlyList<ResultInfo> resultList = this.resultInfoProvider.Get() ?? [];
        IOrderedEnumerable<ResultInfo> orderedResultList =
            from result in resultList
            where result.UUID == trackInfo.UUID
               && result.GameVersion == trackInfo.GameVersion
               && result.Index == mapInfo.Index
            orderby result.Scoring descending
            select result;

        this.scoringRankList.Clear();
        Int32 rank = 1;
        List<ResultInfo> topResults = orderedResultList.Take(10).ToList();
        if (topResults.Count > 0) {
            this.emptyLeaderboardPlaceholder.FadeOut(100, Easing.OutQuint);
            foreach (ResultInfo resultInfo in topResults) {
                this.scoringRankList.Add(new ResultInfoListItem(resultInfo, rank++));
            }
        } else {
            this.emptyLeaderboardPlaceholder.FadeIn(150, Easing.OutQuint);
        }
    }

    public void OnExpanded(TrackInfoListItem item) {
        if (this.expandedItem == item) {
            return;
        }
        if (this.expandedItem is not null) {
            this.expandedItem.IsExpanded = false;
        }

        FileInfo? file = item.TrackInfo.BackgroundFile;
        if (file is not null) {
            TextureLoader? old = this.textureLoader;
            this.textureLoader = new(file, this.renderer);
            this.background.Texture = this.textureLoader.Texture;
            this.background.FadeIn(300, Easing.OutQuint);
            old?.Dispose();
        } else {
            this.background.Texture = null;
            this.textureLoader?.Dispose();
        }
        this.expandedItem = item;
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

    private partial class SongDetailPreviewCard : CompositeDrawable {
        private ZeroVSpriteText titleText = null!;
        private ZeroVSpriteText metaText = null!;
        private ZeroVSpriteText authorText = null!;
        private Container diffBadgeContainer = null!;
        private ZeroVSpriteText diffText = null!;
        private Diamond diffDiamond = null!;
        private FillFlowContainer notesContainer = null!;
        private Container emptyPlaceholder = null!;
        private Container contentContainer = null!;

        public SongDetailPreviewCard() {
            this.Masking = true;
            this.CornerRadius = 0;
            this.BorderThickness = 1.5f;
            this.BorderColour = ZeroVColour.Cyan.Opacity(0.4f);
        }

        [BackgroundDependencyLoader]
        private void load() {
            this.InternalChildren = [
                new Box {
                    RelativeSizeAxes = Axes.Both,
                    Colour = ZeroVColour.BgCard,
                },
                this.contentContainer = new Container {
                    RelativeSizeAxes = Axes.Both,
                    Padding = new MarginPadding(20),
                    Alpha = 0,
                    Children = [
                        new FillFlowContainer {
                            RelativeSizeAxes = Axes.X,
                            AutoSizeAxes = Axes.Y,
                            Direction = FillDirection.Vertical,
                            Spacing = new Vector2(0, 8),
                            Children = [
                                this.titleText = new ZeroVSpriteText {
                                    Text = "Song Title",
                                    Colour = ZeroVColour.TextDark,
                                    FontSize = 24,
                                    Font = FontUsage.Default.With(weight: "Bold"),
                                },
                                this.metaText = new ZeroVSpriteText {
                                    Text = "Artist • Album",
                                    Colour = ZeroVColour.TextMedium,
                                    FontSize = 14,
                                },
                                this.authorText = new ZeroVSpriteText {
                                    Text = "Chart by Author",
                                    Colour = ZeroVColour.TextLight,
                                    FontSize = 12,
                                },
                                new Box {
                                    RelativeSizeAxes = Axes.X,
                                    Height = 1,
                                    Colour = ZeroVColour.BorderLight,
                                    Margin = new MarginPadding { Top = 4, Bottom = 4 },
                                },
                                new FillFlowContainer {
                                    RelativeSizeAxes = Axes.X,
                                    AutoSizeAxes = Axes.Y,
                                    Direction = FillDirection.Horizontal,
                                    Spacing = new Vector2(16, 0),
                                    Children = [
                                        this.diffBadgeContainer = new Container {
                                            Anchor = Anchor.CentreLeft,
                                            Origin = Anchor.CentreLeft,
                                            AutoSizeAxes = Axes.Both,
                                            Masking = true,
                                            CornerRadius = 0,
                                            BorderThickness = 1,
                                            BorderColour = ZeroVColour.Cyan,
                                            Children = [
                                                new Box {
                                                    RelativeSizeAxes = Axes.Both,
                                                    Colour = ZeroVColour.Cyan.Opacity(0.15f),
                                                },
                                                new FillFlowContainer {
                                                    AutoSizeAxes = Axes.Both,
                                                    Direction = FillDirection.Horizontal,
                                                    Spacing = new Vector2(6, 0),
                                                    Padding = new MarginPadding { Left = 8, Right = 8, Top = 4, Bottom = 4 },
                                                    Children = [
                                                        this.diffDiamond = new Diamond {
                                                            Anchor = Anchor.CentreLeft,
                                                            Origin = Anchor.CentreLeft,
                                                            Size = new Vector2(8),
                                                            Colour = ZeroVColour.Cyan,
                                                        },
                                                        this.diffText = new ZeroVSpriteText {
                                                            Anchor = Anchor.CentreLeft,
                                                            Origin = Anchor.CentreLeft,
                                                            Text = "LV. 1.0 EASY",
                                                            Colour = ZeroVColour.Cyan,
                                                            FontSize = 12,
                                                            Font = FontUsage.Default.With(weight: "Bold"),
                                                        },
                                                    ],
                                                },
                                            ],
                                        },
                                        this.notesContainer = new FillFlowContainer {
                                            Anchor = Anchor.CentreLeft,
                                            Origin = Anchor.CentreLeft,
                                            AutoSizeAxes = Axes.Both,
                                            Direction = FillDirection.Horizontal,
                                            Spacing = new Vector2(12, 0),
                                        },
                                    ],
                                },
                            ],
                        },
                    ],
                },
                this.emptyPlaceholder = new Container {
                    RelativeSizeAxes = Axes.Both,
                    Children = [
                        new FillFlowContainer {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            AutoSizeAxes = Axes.Both,
                            Direction = FillDirection.Vertical,
                            Spacing = new Vector2(0, 8),
                            Children = [
                                new Diamond {
                                    Anchor = Anchor.TopCentre,
                                    Origin = Anchor.TopCentre,
                                    Size = new Vector2(20),
                                    Colour = ZeroVColour.Cyan.Opacity(0.4f),
                                },
                                new ZeroVSpriteText {
                                    Anchor = Anchor.TopCentre,
                                    Origin = Anchor.TopCentre,
                                    Text = "SELECT A TRACK",
                                    Colour = ZeroVColour.TextMedium,
                                    FontSize = 15,
                                    Font = FontUsage.Default.With(weight: "Bold"),
                                },
                            ],
                        },
                    ],
                },
            ];
        }

        public void ClearDisplay() {
            this.contentContainer.FadeOut(150, Easing.OutQuint);
            this.emptyPlaceholder.FadeIn(150, Easing.OutQuint);
            this.BorderColour = ZeroVColour.Cyan.Opacity(0.4f);
        }

        public void UpdateDisplay(TrackInfo trackInfo, MapInfo mapInfo) {
            this.emptyPlaceholder.FadeOut(150, Easing.OutQuint);
            this.contentContainer.FadeIn(200, Easing.OutQuint);

            this.titleText.Text = trackInfo.Title;
            String artist = trackInfo.Artists ?? "Unknown Artist";
            String album = trackInfo.Album ?? "Unknown Album";
            this.metaText.Text = $"{artist}  •  {album}";
            this.authorText.Text = $"Mapped by: {trackInfo.GameAuthor}  |  v{trackInfo.GameVersion}";

            Colour4 diffColour = ZeroVColour.ForDifficulty(mapInfo.Difficulty);
            String tierName = ZeroVColour.DifficultyName(mapInfo.Difficulty);

            this.BorderColour = diffColour.Opacity(0.6f);
            this.diffBadgeContainer.BorderColour = diffColour;
            this.diffDiamond.Colour = diffColour;
            this.diffText.Colour = diffColour;
            this.diffText.Text = $"LV. {mapInfo.Difficulty:0.#}  {tierName}";

            this.notesContainer.Clear();
            this.notesContainer.AddRange([
                this.createNoteTag("P", mapInfo.PressCount, ZeroVColour.NotePress),
                this.createNoteTag("S", mapInfo.SlideCount, ZeroVColour.NoteSlide),
                this.createNoteTag("St", mapInfo.StrokeCount, ZeroVColour.NoteStroke),
                this.createNoteTag("B", mapInfo.BlinkCount, ZeroVColour.NoteBlink),
            ]);
        }

        private Drawable createNoteTag(String label, Int32 count, Colour4 colour) => new FillFlowContainer {
            AutoSizeAxes = Axes.Both,
            Direction = FillDirection.Horizontal,
            Spacing = new Vector2(4, 0),
            Children = [
                new ZeroVSpriteText {
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                    Text = label,
                    Colour = colour,
                    FontSize = 12,
                    Font = FontUsage.Default.With(weight: "Bold"),
                },
                new ZeroVSpriteText {
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                    Text = count.ToString(),
                    Colour = ZeroVColour.TextDark,
                    FontSize = 13,
                    Font = FontUsage.Default.With(weight: "Bold"),
                },
            ],
        };
    }
}
