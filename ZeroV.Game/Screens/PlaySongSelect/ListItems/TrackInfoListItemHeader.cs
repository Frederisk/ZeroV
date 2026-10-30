using System;

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

namespace ZeroV.Game.Screens.PlaySongSelect.ListItems;

public sealed partial class TrackInfoListItemHeader : CompositeDrawable {
    private AutoScrollContainer titleScrollContainer = null!;
    private AutoScrollContainer subtitleScrollContainer = null!;
    private Box hoverOverlay = null!;


    [Resolved]
    private PlaySongSelectScreen songSelectScreen { get; set; } = null!;

    [Resolved]
    private TrackInfoListItem listItem { get; set; } = null!;

    public TrackInfoListItemHeader() {
        this.RelativeSizeAxes = Axes.X;
        this.AutoSizeAxes = Axes.Y;
        this.Masking = true;
        this.BorderThickness = 1.5f;
        this.BorderColour = Colour4.Cyan;
    }

    [BackgroundDependencyLoader]
    private void load() {
        TrackInfo trackInfo = this.listItem.TrackInfo;
        var artists = trackInfo.Artists ?? "Unknown Artist";
        var album = trackInfo.Album ?? "Unknown Album";
        var mapCount = trackInfo.MapInfos.Count;

        this.hoverOverlay = new Box {
            RelativeSizeAxes = Axes.Both,
            Colour = Colour4.Cyan.Opacity(0.1f),
            Alpha = 0,
        };
        this.titleScrollContainer = new AutoScrollContainer {
            RelativeSizeAxes = Axes.X,
            AutoSizeAxes = Axes.Y,
            Status = AutoScrollStatus.Stop,
            Child = new ZeroVSpriteText {
                Text = trackInfo.Title,
                Colour = Colour4.Black,
                Font = FontUsage.Default.With(size: 24, weight: "Bold"),
            },
        };
        this.subtitleScrollContainer = new AutoScrollContainer {
            RelativeSizeAxes = Axes.X,
            AutoSizeAxes = Axes.Y,
            Status = AutoScrollStatus.Stop,
            Child = new ZeroVSpriteText {
                Text = $"{artists} - {album}",
                Colour = Colour4.Black,
                Font = FontUsage.Default.With(size: 16),
            },
        };
        this.InternalChildren = [
            // Background
            new Box {
                RelativeSizeAxes = Axes.Both,
                Colour = Colour4.White,
            },
            this.hoverOverlay,
            // Left Bar
            new Box {
                Anchor = Anchor.CentreLeft,
                Origin = Anchor.CentreLeft,
                RelativeSizeAxes = Axes.Y,
                Width = 4,
                Colour = Colour4.Cyan,
                //Alpha = 0.6f,
            },
            // Content
            new Container {
                RelativeSizeAxes = Axes.X,
                AutoSizeAxes = Axes.Y,
                Padding = new MarginPadding { Left = 18, Right = 14, Top = 12, Bottom = 12 },
                Children = [
                    // Left Diamond and Title Container
                    new FillFlowContainer {
                        Anchor = Anchor.CentreLeft,
                        Origin = Anchor.CentreLeft,
                        RelativeSizeAxes = Axes.X,
                        AutoSizeAxes = Axes.Y,
                        Direction = FillDirection.Horizontal,
                        Spacing = new Vector2(12, 0),
                        Children = [
                            // Left Diamond
                            new Diamond {
                                Anchor = Anchor.CentreLeft,
                                Origin = Anchor.CentreLeft,
                                Size = new Vector2(10),
                                Colour = Colour4.Cyan,
                            },
                            new FillFlowContainer {
                                Anchor = Anchor.CentreLeft,
                                Origin = Anchor.CentreLeft,
                                RelativeSizeAxes = Axes.X,
                                AutoSizeAxes = Axes.Y,
                                Width = 0.8f,
                                Direction = FillDirection.Vertical,
                                Spacing = new Vector2(0, 3),
                                Children = [
                                    this.titleScrollContainer,
                                    this.subtitleScrollContainer,
                                ],
                            },
                        ],
                    },
                    // Map Count
                    new Container {
                        Anchor = Anchor.CentreRight,
                        Origin = Anchor.CentreRight,
                        AutoSizeAxes = Axes.Both,
                        //Masking = true,
                        //BorderThickness = 1,
                        //BorderColour = Colour4.Cyan,
                        Children = [
                            new Box {
                                RelativeSizeAxes = Axes.Both,
                                Colour = Colour4.Cyan.Opacity(0.1f),
                            },
                            new ZeroVSpriteText {
                                Padding = new MarginPadding { Left = 6, Right = 6, Top = 4, Bottom = 4 },
                                Text = $"{mapCount} Map{(mapCount is 1 ? "" : "s")}",
                                Colour = Colour4.Black,
                                FontSize = 18,
                            },
                        ],
                    },
                ],
            },
        ];
    }

    public void UpdateExpandedStatus(Boolean isExpanded) {
        // FIXME: Change the colour of the left bar based on the expanded status
        this.UpdateTitleScrollStatus(isExpanded);
    }

    public void UpdateTitleScrollStatus(Boolean start) {
        if (!start && (this.IsHovered || this.listItem.IsExpanded)) { return; }
        AutoScrollStatus status = start ? AutoScrollStatus.Start : AutoScrollStatus.Stop;
        this.titleScrollContainer.Status = status;
        this.subtitleScrollContainer.Status = status;
    }

    protected override Boolean OnHover(HoverEvent e) {
        this.hoverOverlay.FadeIn(150, Easing.OutQuint);
        this.UpdateTitleScrollStatus(true);
        return base.OnHover(e);
    }

    protected override void OnHoverLost(HoverLostEvent e) {
        this.hoverOverlay.FadeOut(150, Easing.InSine);
        this.UpdateTitleScrollStatus(false);
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
