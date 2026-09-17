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
using ZeroV.Game.Utils;

namespace ZeroV.Game.Screens.PlaySongSelect.ListItems;

public partial class ResultInfoListItem : CompositeDrawable {
    private readonly ResultInfo result;
    private readonly Int32 rank;

    private Box hoverOverlay = null!;

    public ResultInfoListItem(ResultInfo result, Int32 rank = 0) {
        this.result = result;
        this.rank = rank;
    }

    [BackgroundDependencyLoader]
    private void load() {
        Colour4 resultColour = this.result.ToResultColour();
        Colour4 rankColour = ZeroVColour.ForRank(this.rank);

        (String statusText, Colour4 statusColour) = this.result switch {
            { IsAllPerfect: true, IsAllDone: true } => ("ALL PERFECT", ZeroVColour.AllPerfect),
            { IsFullCombo: true, IsAllDone: true } => ("FULL COMBO", ZeroVColour.FullCombo),
            { IsAllDone: true } => ("CLEAR", ZeroVColour.Clear),
            _ => ("FAILED", ZeroVColour.Failed),
        };

        this.RelativeSizeAxes = Axes.X;
        this.Height = 46;
        this.Margin = new MarginPadding { Top = 2, Bottom = 2 };
        this.Masking = true;
        this.CornerRadius = 0;
        this.BorderThickness = 1;
        this.BorderColour = ZeroVColour.BorderSubtle;

        this.InternalChildren = [
            new Box {
                RelativeSizeAxes = Axes.Both,
                Colour = ZeroVColour.BgCard,
            },
            this.hoverOverlay = new Box {
                RelativeSizeAxes = Axes.Both,
                Colour = resultColour.Opacity(0.08f),
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
                                CornerRadius = 0,
                                BorderThickness = 1,
                                BorderColour = statusColour.Opacity(0.5f),
                                Children = [
                                    new Box {
                                        RelativeSizeAxes = Axes.Both,
                                        Colour = statusColour.Opacity(0.15f),
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
                                Colour = ZeroVColour.TextLight,
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
        this.BorderColour = ZeroVColour.BorderSubtle;
        base.OnHoverLost(e);
    }
}
