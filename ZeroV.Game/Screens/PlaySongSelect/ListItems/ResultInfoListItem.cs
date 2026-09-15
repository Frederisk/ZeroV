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
using ZeroV.Game.Scoring;

namespace ZeroV.Game.Screens.PlaySongSelect.ListItems;

public partial class ResultInfoListItem : CompositeDrawable {
    private readonly ResultInfo result;
    private readonly Int32 rank;

    private Box hoverOverlay = null!;

    public ResultInfoListItem(ResultInfo result, Int32 rank = 0) {
        this.result = result;
        this.rank = rank;
    }

    private static Colour4 getRankColour(Int32 rank) => rank switch {
        1 => Colour4.FromHex("ffd700"), // Gold
        2 => Colour4.FromHex("e2e8f0"), // Silver
        3 => Colour4.FromHex("f59e0b"), // Bronze
        _ => Colour4.FromHex("64748b"), // Slate
    };

    [BackgroundDependencyLoader]
    private void load() {
        Colour4 resultColour = this.result.ToResultColour();
        Colour4 rankColour = getRankColour(this.rank);

        (String statusText, Colour4 statusColour) = this.result switch {
            { IsAllPerfect: true, IsAllDone: true } => ("ALL PERFECT", Colour4.FromHex("ffd700")),
            { IsFullCombo: true, IsAllDone: true } => ("FULL COMBO", Colour4.FromHex("00d2d3")),
            { IsAllDone: true } => ("CLEAR", Colour4.FromHex("2ed573")),
            _ => ("FAILED", Colour4.FromHex("ff4757")),
        };

        this.RelativeSizeAxes = Axes.X;
        this.Height = 46;
        this.Margin = new MarginPadding { Top = 3, Bottom = 3 };
        this.Masking = true;
        this.CornerRadius = 6;
        this.BorderThickness = 1;
        this.BorderColour = Colour4.White.Opacity(0.12f);

        this.InternalChildren = [
            new Box {
                RelativeSizeAxes = Axes.Both,
                Colour = Colour4.FromHex("0e121a").Opacity(0.88f),
            },
            this.hoverOverlay = new Box {
                RelativeSizeAxes = Axes.Both,
                Colour = resultColour.Opacity(0.1f),
                Alpha = 0,
            },
            new Box {
                Anchor = Anchor.CentreLeft,
                Origin = Anchor.CentreLeft,
                RelativeSizeAxes = Axes.Y,
                Width = 3,
                Colour = resultColour,
            },
            new Container {
                RelativeSizeAxes = Axes.Both,
                Padding = new MarginPadding { Left = 14, Right = 14 },
                Children = [
                    // Left: Rank & Score
                    new FillFlowContainer {
                        Anchor = Anchor.CentreLeft,
                        Origin = Anchor.CentreLeft,
                        AutoSizeAxes = Axes.Both,
                        Direction = FillDirection.Horizontal,
                        Spacing = new Vector2(10, 0),
                        Children = [
                            this.rank > 0 ? new Container {
                                Anchor = Anchor.CentreLeft,
                                Origin = Anchor.CentreLeft,
                                AutoSizeAxes = Axes.Both,
                                Children = [
                                    new Diamond {
                                        Anchor = Anchor.Centre,
                                        Origin = Anchor.Centre,
                                        Size = new Vector2(18),
                                        Colour = rankColour.Opacity(0.2f),
                                    },
                                    new ZeroVSpriteText {
                                        Anchor = Anchor.Centre,
                                        Origin = Anchor.Centre,
                                        Text = $"#{this.rank}",
                                        Colour = rankColour,
                                        FontSize = 12,
                                        Font = FontUsage.Default.With(weight: "Bold"),
                                    },
                                ],
                            } : Empty(),
                            new ZeroVSpriteText {
                                Anchor = Anchor.CentreLeft,
                                Origin = Anchor.CentreLeft,
                                Text = this.result.Scoring.ToDisplayScoring(),
                                Colour = resultColour,
                                FontSize = 19,
                                Font = FontUsage.Default.With(fixedWidth: true, weight: "Bold"),
                            },
                        ],
                    },
                    // Right: Status badge & Time
                    new FillFlowContainer {
                        Anchor = Anchor.CentreRight,
                        Origin = Anchor.CentreRight,
                        AutoSizeAxes = Axes.Both,
                        Direction = FillDirection.Horizontal,
                        Spacing = new Vector2(12, 0),
                        Children = [
                            new Container {
                                Anchor = Anchor.CentreRight,
                                Origin = Anchor.CentreRight,
                                AutoSizeAxes = Axes.Both,
                                Masking = true,
                                CornerRadius = 3,
                                Children = [
                                    new Box {
                                        RelativeSizeAxes = Axes.Both,
                                        Colour = statusColour.Opacity(0.18f),
                                    },
                                    new ZeroVSpriteText {
                                        Padding = new MarginPadding { Left = 6, Right = 6, Top = 2, Bottom = 2 },
                                        Text = statusText,
                                        Colour = statusColour,
                                        FontSize = 10,
                                        Font = FontUsage.Default.With(weight: "Bold"),
                                    },
                                ],
                            },
                            new ZeroVSpriteText {
                                Anchor = Anchor.CentreRight,
                                Origin = Anchor.CentreRight,
                                Text = this.result.FinishTime.ToString("yyyy/MM/dd HH:mm"),
                                Colour = Colour4.FromHex("64748b"),
                                FontSize = 11,
                            },
                        ],
                    },
                ],
            },
        ];
    }

    protected override Boolean OnHover(HoverEvent e) {
        this.hoverOverlay.FadeIn(120, Easing.OutQuint);
        this.BorderColour = this.result.ToResultColour().Opacity(0.6f);
        return base.OnHover(e);
    }

    protected override void OnHoverLost(HoverLostEvent e) {
        this.hoverOverlay.FadeOut(120, Easing.InSine);
        this.BorderColour = Colour4.White.Opacity(0.12f);
        base.OnHoverLost(e);
    }
}
