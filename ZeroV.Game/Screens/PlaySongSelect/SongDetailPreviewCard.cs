using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;

using osuTK;

using ZeroV.Game.Graphics;
using ZeroV.Game.Graphics.Shapes;

namespace ZeroV.Game.Screens.PlaySongSelect;

public partial class SongDetailPreviewCard : CompositeDrawable {
    private ZeroVSpriteText titleSpriteText = null!;
    private ZeroVSpriteText metaSpriteText = null!;
    private ZeroVSpriteText authorSpriteText = null!;

    private NoteTag pressNoteTag = null!;
    private NoteTag slideNoteTag = null!;
    private NoteTag strokeNoteTag = null!;
    private NoteTag blinkNoteTag = null!;
    private Container diffBadgeContainer = null!;

    public SongDetailPreviewCard(/*TrackInfo trackInfo, MapInfo mapInfo*/) {
        this.Masking = true;
        this.BorderColour = Colour4.Cyan;
        this.BorderThickness = 1.5f;
        //this.trackInfo = trackInfo;
        //this.mapInfo = mapInfo;
    }

    [BackgroundDependencyLoader]
    private void load() {
        this.pressNoteTag = new NoteTag("P", Colour4.Pink);
        this.slideNoteTag = new NoteTag("S", Colour4.GreenYellow);
        this.strokeNoteTag = new NoteTag("St", Colour4.Gold);
        this.blinkNoteTag = new NoteTag("B", Colour4.Red);
        this.diffBadgeContainer = new Container {
            Anchor = Anchor.CentreLeft,
            Origin = Anchor.CentreLeft,
            AutoSizeAxes = Axes.Both,
            Masking = true,
            BorderColour = Colour4.Cyan, // TODO: Auto colour.
            BorderThickness = 1,
            Children = [
                // Background
                new Box {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Colour4.Cyan.Opacity(0.4f), // TODO: auto colour.
                },
                // Content
                new FillFlowContainer {
                    AutoSizeAxes = Axes.Both,
                    Direction = FillDirection.Horizontal,
                    //Padding =
                    Spacing = new Vector2(4, 0),
                    Children = [
                        new Diamond {
                            Anchor = Anchor.CentreLeft,
                            Origin = Anchor.CentreLeft,
                            Size = new Vector2(8),
                            Colour = Colour4.Cyan, // TODO: auto colour.
                            Margin = new MarginPadding { Horizontal = 4 },
                        },
                        new ZeroVSpriteText {
                            Anchor = Anchor.CentreLeft,
                            Origin = Anchor.CentreLeft,
                            Colour = Colour4.Cyan, // TODO: auto colour.
                            FontSize = 12,
                            Text = $"Lv. {1.0:##.#}" + " " + "Easy", // FIXME: update.
                        },
                        this.pressNoteTag,
                        this.slideNoteTag,
                        this.strokeNoteTag,
                        this.blinkNoteTag,
                    ],
                }
            ],
        };

        this.InternalChildren = [
            // Background
            new Box {
                RelativeSizeAxes = Axes.Both,
                Colour = Colour4.White,
            },
            // Content
            new Container {
                RelativeSizeAxes = Axes.Both,
                Padding = new MarginPadding(20),
                Children = [
                    new FillFlowContainer {
                        RelativeSizeAxes = Axes.X,
                        AutoSizeAxes = Axes.Y,
                        Direction = FillDirection.Vertical,
                        Spacing = new Vector2(0, 2),
                        Children = [

                        ],
                    },
                    
                ],
            },
            
        ];



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
                FontSize = 12,
                Text = "0",
            };

            this.InternalChild = new FillFlowContainer {
                AutoSizeAxes = Axes.Both,
                Direction = FillDirection.Horizontal,

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
}
