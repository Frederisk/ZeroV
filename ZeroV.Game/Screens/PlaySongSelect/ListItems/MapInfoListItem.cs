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
using ZeroV.Game.Utils;

namespace ZeroV.Game.Screens.PlaySongSelect.ListItems;

public partial class MapInfoListItem(MapInfo mapInfo) : CompositeDrawable {
    public Boolean IsSelected { get; private set; }

    public MapInfo MapInfo => mapInfo;

    [Resolved]
    private PlaySongSelectScreen songSelectScreen { get; set; } = null!;

    private Box backgroundBox = null!;
    private Box hoverOverlay = null!;
    private Box leftAccentBar = null!;
    private Container playBadge = null!;
    private ZeroVSpriteText selectPrompt = null!;
    private Colour4 diffColour;

    [BackgroundDependencyLoader]
    private void load() {
        this.diffColour = ZeroVColour.ForDifficulty(mapInfo.Difficulty);
        String tierName = ZeroVColour.DifficultyName(mapInfo.Difficulty);

        this.RelativeSizeAxes = Axes.X;
        this.Height = 54;
        this.Masking = true;
        this.CornerRadius = 0;
        this.BorderThickness = 1;
        this.BorderColour = ZeroVColour.BorderSubtle;
        this.Margin = new MarginPadding { Top = 2, Bottom = 2 };

        this.InternalChildren = [
            this.backgroundBox = new Box {
                RelativeSizeAxes = Axes.Both,
                Colour = ZeroVColour.BgCard,
            },
            this.hoverOverlay = new Box {
                RelativeSizeAxes = Axes.Both,
                Colour = this.diffColour.Opacity(0.08f),
                Alpha = 0,
            },
            this.leftAccentBar = new Box {
                Anchor = Anchor.CentreLeft,
                Origin = Anchor.CentreLeft,
                RelativeSizeAxes = Axes.Y,
                Width = 4,
                Colour = this.diffColour,
                Alpha = 0.7f,
            },
            new Container {
                RelativeSizeAxes = Axes.Both,
                Padding = new MarginPadding { Left = 16, Right = 16 },
                Children = [
                    // Left: Difficulty badge & Tier
                    new FillFlowContainer {
                        Anchor = Anchor.CentreLeft,
                        Origin = Anchor.CentreLeft,
                        AutoSizeAxes = Axes.Both,
                        Direction = FillDirection.Horizontal,
                        Spacing = new Vector2(10, 0),
                        Children = [
                            new Diamond {
                                Anchor = Anchor.CentreLeft,
                                Origin = Anchor.CentreLeft,
                                Size = new Vector2(10),
                                Colour = this.diffColour,
                            },
                            new ZeroVSpriteText {
                                Anchor = Anchor.CentreLeft,
                                Origin = Anchor.CentreLeft,
                                Text = $"LV. {mapInfo.Difficulty:0.#}",
                                Colour = this.diffColour,
                                Font = FontUsage.Default.With(size: 19, weight: "Bold"),
                            },
                            new Container {
                                Anchor = Anchor.CentreLeft,
                                Origin = Anchor.CentreLeft,
                                AutoSizeAxes = Axes.Both,
                                Masking = true,
                                CornerRadius = 0,
                                BorderThickness = 1,
                                BorderColour = this.diffColour.Opacity(0.5f),
                                Children = [
                                    new Box {
                                        RelativeSizeAxes = Axes.Both,
                                        Colour = this.diffColour.Opacity(0.12f),
                                    },
                                    new ZeroVSpriteText {
                                        Padding = new MarginPadding { Left = 6, Right = 6, Top = 2, Bottom = 2 },
                                        Text = tierName,
                                        Colour = this.diffColour,
                                        FontSize = 11,
                                        Font = FontUsage.Default.With(weight: "Bold"),
                                    },
                                ],
                            },
                        ],
                    },
                    // Middle: Note Counts Summary
                    new FillFlowContainer {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        AutoSizeAxes = Axes.Both,
                        Direction = FillDirection.Horizontal,
                        Spacing = new Vector2(14, 0),
                        Children = [
                            this.createNoteCountChip("PRESS", mapInfo.PressCount, ZeroVColour.NotePress),
                            this.createNoteCountChip("SLIDE", mapInfo.SlideCount, ZeroVColour.NoteSlide),
                            this.createNoteCountChip("STROKE", mapInfo.StrokeCount, ZeroVColour.NoteStroke),
                            this.createNoteCountChip("BLINK", mapInfo.BlinkCount, ZeroVColour.NoteBlink),
                        ],
                    },
                    // Right: Play / Select button prompt
                    new Container {
                        Anchor = Anchor.CentreRight,
                        Origin = Anchor.CentreRight,
                        AutoSizeAxes = Axes.Both,
                        Children = [
                            this.selectPrompt = new ZeroVSpriteText {
                                Anchor = Anchor.CentreRight,
                                Origin = Anchor.CentreRight,
                                Text = "SELECT",
                                Colour = ZeroVColour.TextLight,
                                FontSize = 12,
                                Font = FontUsage.Default.With(weight: "Bold"),
                            },
                            this.playBadge = new Container {
                                Anchor = Anchor.CentreRight,
                                Origin = Anchor.CentreRight,
                                AutoSizeAxes = Axes.Both,
                                Masking = true,
                                CornerRadius = 0,
                                BorderThickness = 1,
                                BorderColour = this.diffColour,
                                Alpha = 0,
                                Children = [
                                    new Box {
                                        RelativeSizeAxes = Axes.Both,
                                        Colour = this.diffColour,
                                    },
                                    new ZeroVSpriteText {
                                        Padding = new MarginPadding { Left = 10, Right = 10, Top = 4, Bottom = 4 },
                                        Text = "▶  PLAY",
                                        Colour = ZeroVColour.TextWhite,
                                        FontSize = 13,
                                        Font = FontUsage.Default.With(weight: "Bold"),
                                    },
                                ],
                            },
                        ],
                    },
                ],
            },
        ];
    }

    private Drawable createNoteCountChip(String label, Int32 count, Colour4 colour) => new FillFlowContainer {
        AutoSizeAxes = Axes.Both,
        Direction = FillDirection.Horizontal,
        Spacing = new Vector2(4, 0),
        Children = [
            new ZeroVSpriteText {
                Anchor = Anchor.CentreLeft,
                Origin = Anchor.CentreLeft,
                Text = label,
                Colour = colour,
                FontSize = 10,
                Font = FontUsage.Default.With(weight: "Bold"),
            },
            new ZeroVSpriteText {
                Anchor = Anchor.CentreLeft,
                Origin = Anchor.CentreLeft,
                Text = count.ToString(),
                Colour = ZeroVColour.TextDark,
                FontSize = 12,
                Font = FontUsage.Default.With(weight: "Bold"),
            },
        ],
    };

    public void OnSelect() {
        if (!this.IsSelected) {
            this.songSelectScreen.OnSelect(this);

            this.IsSelected = true;
            this.BorderThickness = 2;
            this.BorderColour = this.diffColour;
            this.leftAccentBar.FadeTo(1f, 150, Easing.OutQuint);
            this.backgroundBox.FadeColour(ZeroVColour.BgCardSelected, 150, Easing.OutQuint);

            this.selectPrompt.FadeOut(100, Easing.OutQuint);
            this.playBadge.FadeIn(150, Easing.OutQuint);
        } else {
            this.songSelectScreen.ConfirmSelect();
        }
    }

    public void OnSelectCancel() {
        this.IsSelected = false;
        this.BorderThickness = 1;
        this.BorderColour = this.IsHovered ? this.diffColour.Opacity(0.5f) : ZeroVColour.BorderSubtle;
        this.leftAccentBar.FadeTo(this.IsHovered ? 0.8f : 0.6f, 150, Easing.OutQuint);
        this.backgroundBox.FadeColour(ZeroVColour.BgCard, 150, Easing.OutQuint);

        this.selectPrompt.FadeIn(100, Easing.OutQuint);
        this.playBadge.FadeOut(100, Easing.OutQuint);
    }

    protected override Boolean OnHover(HoverEvent e) {
        this.hoverOverlay.FadeIn(120, Easing.OutQuint);
        if (!this.IsSelected) {
            this.BorderColour = this.diffColour.Opacity(0.6f);
            this.leftAccentBar.FadeTo(0.9f, 120, Easing.OutQuint);
        }
        return base.OnHover(e);
    }

    protected override void OnHoverLost(HoverLostEvent e) {
        this.hoverOverlay.FadeOut(120, Easing.InSine);
        if (!this.IsSelected) {
            this.BorderColour = ZeroVColour.BorderSubtle;
            this.leftAccentBar.FadeTo(0.6f, 120, Easing.InSine);
        }
        base.OnHoverLost(e);
    }

    protected override Boolean OnClick(ClickEvent e) {
        this.OnSelect();
        return base.OnClick(e);
    }
}
