using System;
using System.Linq;

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;

using osuTK;

using ZeroV.Game.Graphics;
using ZeroV.Game.Graphics.Shapes;
using ZeroV.Game.Objects;
using ZeroV.Game.Utils;

namespace ZeroV.Game.Screens.PlaySongSelect.ListItems;

public partial class TrackInfoListItemHeader : CompositeDrawable {
    private Container titleScrollContainer = null!;
    private FillFlowContainer titleContainer = null!;
    private SpriteText title = null!;
    private SpriteText subTitle = null!;

    private Box backgroundBox = null!;
    private Box hoverOverlay = null!;
    private Box leftAccentBar = null!;
    private Diamond expandDiamond = null!;
    private Container mapCountBadge = null!;

    [Resolved]
    private PlaySongSelectScreen songSelectScreen { get; set; } = null!;

    [Resolved]
    private TrackInfoListItem listItem { get; set; } = null!;

    [BackgroundDependencyLoader]
    private void load() {
        this.RelativeSizeAxes = Axes.X;
        this.AutoSizeAxes = Axes.Y;
        this.Masking = true;
        this.CornerRadius = 0;
        this.BorderThickness = 1.5f;
        this.BorderColour = ZeroVColour.BorderSubtle;

        TrackInfo trackInfo = this.listItem.TrackInfo;
        String artists = trackInfo.Artists ?? "Unknown Artist";
        String album = trackInfo.Album ?? "Unknown Album";
        Int32 mapCount = trackInfo.MapInfos?.Count ?? 0;

        this.InternalChildren = [
            this.backgroundBox = new Box {
                RelativeSizeAxes = Axes.Both,
                Colour = ZeroVColour.BgCard,
            },
            this.hoverOverlay = new Box {
                RelativeSizeAxes = Axes.Both,
                Colour = ZeroVColour.Cyan.Opacity(0.08f),
                Alpha = 0,
            },
            this.leftAccentBar = new Box {
                Anchor = Anchor.CentreLeft,
                Origin = Anchor.CentreLeft,
                RelativeSizeAxes = Axes.Y,
                Width = 4,
                Colour = ZeroVColour.Cyan,
                Alpha = 0.6f,
            },
            new Container {
                RelativeSizeAxes = Axes.X,
                AutoSizeAxes = Axes.Y,
                Padding = new MarginPadding { Left = 20, Right = 16, Top = 14, Bottom = 14 },
                Children = [
                    new FillFlowContainer {
                        Anchor = Anchor.CentreLeft,
                        Origin = Anchor.CentreLeft,
                        RelativeSizeAxes = Axes.X,
                        AutoSizeAxes = Axes.Y,
                        Direction = FillDirection.Horizontal,
                        Spacing = new Vector2(12, 0),
                        Children = [
                            new Diamond {
                                Anchor = Anchor.CentreLeft,
                                Origin = Anchor.CentreLeft,
                                Size = new Vector2(10),
                                Colour = ZeroVColour.Cyan,
                            },
                            this.titleScrollContainer = new Container {
                                Anchor = Anchor.CentreLeft,
                                Origin = Anchor.CentreLeft,
                                RelativeSizeAxes = Axes.X,
                                AutoSizeAxes = Axes.Y,
                                Width = 0.72f,
                                Masking = true,
                                Child = this.titleContainer = new FillFlowContainer {
                                    RelativeSizeAxes = Axes.X,
                                    AutoSizeAxes = Axes.Y,
                                    Direction = FillDirection.Vertical,
                                    Spacing = new Vector2(0, 3),
                                    Children = [
                                        this.title = new ZeroVSpriteText {
                                            Origin = Anchor.CentreLeft,
                                            Anchor = Anchor.CentreLeft,
                                            Text = trackInfo.Title,
                                            Colour = ZeroVColour.TextDark,
                                            Font = FontUsage.Default.With(size: 22, weight: "Bold"),
                                        },
                                        this.subTitle = new ZeroVSpriteText {
                                            Origin = Anchor.CentreLeft,
                                            Anchor = Anchor.CentreLeft,
                                            Text = $"{artists}  •  {album}",
                                            Colour = ZeroVColour.TextLight,
                                            FontSize = 14,
                                        },
                                    ],
                                },
                            },
                        ],
                    },
                    new FillFlowContainer {
                        Anchor = Anchor.CentreRight,
                        Origin = Anchor.CentreRight,
                        AutoSizeAxes = Axes.Both,
                        Direction = FillDirection.Horizontal,
                        Spacing = new Vector2(12, 0),
                        Children = [
                            this.mapCountBadge = new Container {
                                Anchor = Anchor.CentreLeft,
                                Origin = Anchor.CentreLeft,
                                AutoSizeAxes = Axes.Both,
                                Masking = true,
                                CornerRadius = 0,
                                BorderThickness = 1,
                                BorderColour = ZeroVColour.Purple.Opacity(0.6f),
                                Children = [
                                    new Box {
                                        RelativeSizeAxes = Axes.Both,
                                        Colour = ZeroVColour.Purple.Opacity(0.12f),
                                    },
                                    new ZeroVSpriteText {
                                        Padding = new MarginPadding { Left = 8, Right = 8, Top = 4, Bottom = 4 },
                                        Text = $"{mapCount} MAP{(mapCount == 1 ? "" : "S")}",
                                        Colour = ZeroVColour.Purple,
                                        FontSize = 12,
                                        Font = FontUsage.Default.With(weight: "Bold"),
                                    },
                                ],
                            },
                            this.expandDiamond = new Diamond {
                                Anchor = Anchor.CentreLeft,
                                Origin = Anchor.CentreLeft,
                                Size = new Vector2(12),
                                Colour = ZeroVColour.Cyan,
                            },
                        ],
                    },
                ],
            },
        ];
    }

    public void UpdateExpandedState(Boolean expanded) {
        if (expanded) {
            this.BorderColour = ZeroVColour.Cyan;
            this.leftAccentBar.FadeTo(1f, 200, Easing.OutQuint);
            this.expandDiamond.RotateTo(45, 250, Easing.OutQuint);
            this.expandDiamond.FadeColour(ZeroVColour.CyanBright, 200);
            this.backgroundBox.FadeColour(ZeroVColour.BgCardSelected, 200, Easing.OutQuint);
        } else {
            this.BorderColour = this.IsHovered ? ZeroVColour.Cyan.Opacity(0.7f) : ZeroVColour.BorderSubtle;
            this.leftAccentBar.FadeTo(this.IsHovered ? 0.8f : 0.6f, 200, Easing.OutQuint);
            this.expandDiamond.RotateTo(0, 250, Easing.OutQuint);
            this.expandDiamond.FadeColour(ZeroVColour.Cyan, 200);
            this.backgroundBox.FadeColour(ZeroVColour.BgCard, 200, Easing.OutQuint);
        }
    }

    private const Single padding = 5;
    private const Double long_title_scroll_speed = 0.05;

    public void TryBeginLongTitleScroll() {
        void scroll(Drawable drawable) {
            if (drawable.Transforms.Any() || drawable.DrawWidth < this.titleScrollContainer.DrawWidth) {
                return;
            }
            Single offset = drawable.DrawWidth - this.titleScrollContainer.DrawWidth + (padding * 2);
            Double duration = offset / long_title_scroll_speed;

            MarginPadding toMargin = new() { Left = -offset };
            drawable.TransformTo(nameof(drawable.Margin), toMargin, duration).Then()
                    .Delay(1000).Then()
                    .TransformTo(nameof(drawable.Margin), new MarginPadding(), duration).Then()
                    .Delay(1000).Then()
                    .Loop();
        }

        if (this.IsHovered || this.listItem.IsExpanded) {
            scroll(this.title);
            scroll(this.subTitle);
        }
    }

    public void TryEndLongTitleScroll() {
        if (!this.IsHovered && !this.listItem.IsExpanded) {
            this.title.ClearTransforms();
            this.title.Margin = new MarginPadding(0);
            this.subTitle.ClearTransforms();
            this.subTitle.Margin = new MarginPadding(0);
        }
    }

    protected override Boolean OnHover(HoverEvent e) {
        this.hoverOverlay.FadeIn(150, Easing.OutQuint);
        if (!this.listItem.IsExpanded) {
            this.BorderColour = Colour4.FromHex("00d2d3").Opacity(0.7f);
            this.leftAccentBar.FadeTo(0.8f, 150, Easing.OutQuint);
        }
        this.TryBeginLongTitleScroll();
        return base.OnHover(e);
    }

    protected override void OnHoverLost(HoverLostEvent e) {
        this.hoverOverlay.FadeOut(150, Easing.InSine);
        if (!this.listItem.IsExpanded) {
            this.BorderColour = Colour4.White.Opacity(0.15f);
            this.leftAccentBar.FadeTo(0.5f, 150, Easing.InSine);
        }
        this.TryEndLongTitleScroll();
        base.OnHoverLost(e);
    }

    protected override Boolean OnClick(ClickEvent e) {
        this.listItem.IsExpanded = !this.listItem.IsExpanded;
        if (this.listItem.IsExpanded) {
            this.songSelectScreen.OnExpanded(this.listItem);
        }
        return base.OnClick(e);
    }
}
