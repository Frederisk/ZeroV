using System;

using osu.Framework.Allocation;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics;
using osu.Framework.Input.Events;

using osuTK;

using ZeroV.Game.Elements;
using ZeroV.Game.Graphics;
using ZeroV.Game.Graphics.Shapes;
using ZeroV.Game.Objects;
using ZeroV.Game.Utils;

namespace ZeroV.Game.Screens.PlaySongSelect.ListItems;

public sealed partial class MapInfoListItem : CompositeDrawable {
    private readonly Colour4 defaultBorderColour;
    private readonly Colour4 diffColour;
    private Box hoverOverlay = null!;

    public Boolean IsSelected { get; private set; }

    //public Int32 Index => index;
    public MapInfo MapInfo { get; }

    [Resolved]
    private PlaySongSelectScreen songSelectScreen { get; set; } = null!;

    public MapInfoListItem(MapInfo mapInfo) {
        this.MapInfo = mapInfo;
        this.RelativeSizeAxes = Axes.X;
        this.AutoSizeAxes = Axes.Y;
        this.Masking = true;
        this.BorderThickness = 1;
        this.defaultBorderColour = Colour4.LightPink;
        this.BorderColour = this.defaultBorderColour;
        this.Margin = new MarginPadding(4);
        this.diffColour = ZeroVColour.FromDifficulty(mapInfo.Difficulty);
    }

    [BackgroundDependencyLoader]
    private void load() {
        this.hoverOverlay = new Box {
            RelativeSizeAxes = Axes.Both,
            Colour = this.diffColour.Opacity(0.1f),
            Alpha = 0,
        };
        this.InternalChildren = [
            // Background
            new Box {
                RelativeSizeAxes = Axes.Both,
                Colour = Colour4.White,
            },
            this.hoverOverlay,
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
                Padding = new MarginPadding { Vertical = 16, Horizontal = 32 },
                Children = [
                    // Left
                    new FillFlowContainer {
                        Anchor = Anchor.CentreLeft,
                        Origin = Anchor.CentreLeft,
                        AutoSizeAxes = Axes.Both,
                        Direction = FillDirection.Horizontal,
                        Spacing = new Vector2(16, 0),
                        Children = [
                            new Diamond {
                                Anchor = Anchor.CentreLeft,
                                Origin = Anchor.CentreLeft,
                                Size = new Vector2(12),
                                Colour = this.diffColour,
                            },
                            new ZeroVSpriteText {
                                Anchor = Anchor.CentreLeft,
                                Origin = Anchor.CentreLeft,
                                Colour = this.diffColour,
                                FontSize = 32,
                                Text = $"Lv. {this.MapInfo.Difficulty: 0.0}",
                            },
                            new NoteTag("Press", ZeroVColour.NOTE_PRESS) {
                                Anchor = Anchor.CentreLeft,
                                Origin = Anchor.CentreLeft,
                                NoteCount = this.MapInfo.PressCount,
                            },
                            new NoteTag("Slide", ZeroVColour.NOTE_SLIDE) {
                                Anchor = Anchor.CentreLeft,
                                Origin = Anchor.CentreLeft,
                                NoteCount = this.MapInfo.SlideCount,
                            },
                            new NoteTag("Stroke", ZeroVColour.NOTE_STROKE) {
                                Anchor = Anchor.CentreLeft,
                                Origin = Anchor.CentreLeft,
                                NoteCount = this.MapInfo.StrokeCount,
                            },
                            new NoteTag("Blink", ZeroVColour.NOTE_BLINK) {
                                Anchor = Anchor.CentreLeft,
                                Origin = Anchor.CentreLeft,
                                NoteCount = this.MapInfo.BlinkCount,
                            },
                        ],
                    },
                    // Right
                ],
            }
        ];
    }

    public void OnSelect() {
        if (!this.IsSelected) {
            this.songSelectScreen.OnSelect(this);

            this.IsSelected = true;
            this.BorderThickness = 3;
            this.BorderColour = this.diffColour;

            // this.playBadge.FadeIn(150, Easing.OutQuint);
        } else {
            this.songSelectScreen.ConfirmSelect();
        }
    }

    public void OnSelectCancel() {
        this.IsSelected = false;
        this.BorderThickness = 1;
        this.BorderColour = this.defaultBorderColour;

        // this.playBadge.FadeOut(100, Easing.OutQuint);
    }

    protected override Boolean OnHover(HoverEvent e) {
        this.hoverOverlay.FadeIn(120, Easing.OutQuint);
        return base.OnHover(e);
    }

    protected override void OnHoverLost(HoverLostEvent e) {
        this.hoverOverlay.FadeOut(120, Easing.InSine);
        base.OnHoverLost(e);
    }

    protected override Boolean OnClick(ClickEvent e) {
        this.OnSelect();
        return base.OnClick(e);
    }
}
