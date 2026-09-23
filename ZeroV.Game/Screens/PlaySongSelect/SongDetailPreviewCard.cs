using System;

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;

using osuTK;

using ZeroV.Game.Graphics;
using ZeroV.Game.Graphics.Shapes;
using ZeroV.Game.Objects;
using ZeroV.Game.Utils;

namespace ZeroV.Game.Screens.PlaySongSelect;

public partial class SongDetailPreviewCard : CompositeDrawable {
    private Container contentContainer = null!;
    private ZeroVSpriteText titleSpriteText = null!;
    private ZeroVSpriteText metaSpriteText = null!;
    private ZeroVSpriteText authorSpriteText = null!;
    private DiffBadge diffBadgeContainer = null!;
    
    private NoteTag pressNoteTag = null!;
    private NoteTag slideNoteTag = null!;
    private NoteTag strokeNoteTag = null!;
    private NoteTag blinkNoteTag = null!;
    private CardEmptyPlaceholder emptyPlaceholder = null!;

    public SongDetailPreviewCard() {
        this.Masking = true;
        this.BorderColour = Colour4.Cyan;
        this.BorderThickness = 1.5f;
    }

    [BackgroundDependencyLoader]
    private void load() {
        this.titleSpriteText = new ZeroVSpriteText {
            Colour = Colour4.Black,
            FontSize = 24,
            Font = FontUsage.Default.With(weight: "Bold"),
            Text = "Song Title",
        };
        this.metaSpriteText = new ZeroVSpriteText {
            Colour = Colour4.DarkGray,
            FontSize = 14,
            Text = "Artist  -  Album",
        };
        this.authorSpriteText = new ZeroVSpriteText {
            Colour = Colour4.Gray,
            FontSize = 14,
            Text = "Map Author",
        };
        this.diffBadgeContainer = new DiffBadge {
            Anchor = Anchor.CentreLeft,
            Origin = Anchor.CentreLeft,
        };
        this.pressNoteTag = new NoteTag("P", Colour4.Pink) {
            Anchor = Anchor.CentreLeft,
            Origin = Anchor.CentreLeft,
        };
        this.slideNoteTag = new NoteTag("S", Colour4.GreenYellow) {
            Anchor = Anchor.CentreLeft,
            Origin = Anchor.CentreLeft,
        };
        this.strokeNoteTag = new NoteTag("St", Colour4.Gold) {
            Anchor = Anchor.CentreLeft,
            Origin = Anchor.CentreLeft,
        };
        this.blinkNoteTag = new NoteTag("B", Colour4.Red) {
            Anchor = Anchor.CentreLeft,
            Origin = Anchor.CentreLeft,
        };
        this.emptyPlaceholder = new CardEmptyPlaceholder("MUSIC SELECT", "Choose a track to play.");
        this.contentContainer = new Container {
            RelativeSizeAxes = Axes.Both,
            Padding = new MarginPadding(20),
            Children = [
                new FillFlowContainer {
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    Direction = FillDirection.Vertical,
                    Spacing = new Vector2(0, 8),
                    Children = [
                        this.titleSpriteText,
                        this.metaSpriteText,
                        this.authorSpriteText,
                        // Divider
                        new Box {
                            RelativeSizeAxes = Axes.X,
                            Height = 1,
                            Colour = Colour4.Cyan,
                            Margin = new MarginPadding(4),
                        },
                        new FillFlowContainer {
                            RelativeSizeAxes = Axes.X,
                            AutoSizeAxes = Axes.Y,
                            Direction = FillDirection.Horizontal,
                            Spacing = new Vector2(16, 0),
                            Children = [
                                this.diffBadgeContainer,
                                this.pressNoteTag,
                                this.slideNoteTag,
                                this.strokeNoteTag,
                                this.blinkNoteTag,
                            ],
                        }
                    ],
                },
            ],
        };
        this.contentContainer.Hide();
        this.InternalChildren = [
            // Background
            new Box {
                RelativeSizeAxes = Axes.Both,
                Colour = Colour4.White,
            },
            // Content
            this.contentContainer,
            // Empty state placeholder
            this.emptyPlaceholder,
        ];
    }

    public void ClearDisplay() {
        this.contentContainer.FadeOut(150, Easing.Out);
        this.emptyPlaceholder.FadeIn(150, Easing.In);
        this.BorderColour = Colour4.Cyan;
    }

    public void UpdateDisplay(TrackInfo trackInfo, MapInfo mapInfo) {
        this.contentContainer.FadeIn(150, Easing.In);
        this.emptyPlaceholder.FadeOut(150, Easing.Out);
        this.titleSpriteText.Text = trackInfo.Title;
        this.metaSpriteText.Text = $"{trackInfo.Artists ?? "Unknown Artist"}  -  {trackInfo.Album ?? "Unknown Album"}";
        this.authorSpriteText.Text = $"Mapped by: {trackInfo.GameAuthor}  |  (v{trackInfo.GameVersion})";
        this.BorderColour = ZeroVColour.FromDifficulty(mapInfo.Difficulty);
        this.diffBadgeContainer.UpdateDisplay(mapInfo.Difficulty);
        this.pressNoteTag.UpdateCount(mapInfo.PressCount);
        this.slideNoteTag.UpdateCount(mapInfo.SlideCount);
        this.strokeNoteTag.UpdateCount(mapInfo.StrokeCount);
        this.blinkNoteTag.UpdateCount(mapInfo.BlinkCount);
    }

    private sealed partial class NoteTag : CompositeDrawable {
        private readonly string label;
        private readonly ColourInfo colourInfo;

        private ZeroVSpriteText countSpriteText = null!;

        public NoteTag(String label, ColourInfo colourInfo) {
            this.label = label;
            //this.count = count;
            this.colourInfo = colourInfo;
            this.AutoSizeAxes = Axes.Both;
        }

        public void UpdateCount(Int32 count) {
            this.countSpriteText.Text = count.ToString();
        }

        [BackgroundDependencyLoader]
        private void load() {
            this.countSpriteText = new ZeroVSpriteText {
                Anchor = Anchor.CentreLeft,
                Origin = Anchor.CentreLeft,
                Colour = Colour4.Black,
                FontSize = 14,
                Text = "0",
            };

            this.InternalChild = new FillFlowContainer {
                AutoSizeAxes = Axes.Both,
                Direction = FillDirection.Horizontal,
                Spacing = new Vector2(4, 0),
                Children = [
                    new ZeroVSpriteText {
                        Anchor = Anchor.CentreLeft,
                        Origin = Anchor.CentreLeft,
                        Colour = this.colourInfo,
                        FontSize = 12,
                        Font = FontUsage.Default.With(weight: "Bold"),
                        Text = this.label,
                    },
                    this.countSpriteText,
                ],
            };
        }
    }

    private sealed partial class DiffBadge : CompositeDrawable {
        private ZeroVSpriteText diffSpriteText = null!;
        private Box background = null!;
        private Diamond diamond = null!;

        public DiffBadge() {
            this.AutoSizeAxes = Axes.Both;
            this.Masking = true;
            this.BorderColour = Colour4.Cyan;
            this.BorderThickness = 1.5f;
        }

        [BackgroundDependencyLoader]
        private void load() {
            this.background = new Box {
                RelativeSizeAxes = Axes.Both,
                Colour = Colour4.Cyan.Opacity(0.05f),
            };
            this.diamond = new Diamond {
                Anchor = Anchor.CentreLeft,
                Origin = Anchor.CentreLeft,
                Size = new Vector2(6),
                Colour = Colour4.Cyan,
                //Margin = new MarginPadding { Horizontal = 4 },
            };
            this.diffSpriteText = new ZeroVSpriteText {
                Anchor = Anchor.CentreLeft,
                Origin = Anchor.CentreLeft,
                Colour = Colour4.Cyan,
                FontSize = 16,
                Text = $"Lv. {1.0:##.0}", // + " " + "Easy",
            };
            this.InternalChildren = [
                this.background,
                // Content
                new FillFlowContainer {
                    AutoSizeAxes = Axes.Both,
                    Direction = FillDirection.Horizontal,
                    Padding = new MarginPadding { Horizontal = 8, Vertical = 2 },
                    Spacing = new Vector2(4, 0),
                    Children = [
                        this.diamond,
                        this.diffSpriteText,
                    ],
                }
            ];
        }

        public void UpdateDisplay(Double diff) {
            this.diffSpriteText.Text = $"Lv. {diff: ##.0}";
            Colour4 diffColour = ZeroVColour.FromDifficulty(diff);
            this.diffSpriteText.Colour = diffColour;
            this.BorderColour = diffColour;
            this.diamond.Colour = diffColour;
            this.background.Colour = diffColour.Opacity(0.05f);
        }
    }
}
