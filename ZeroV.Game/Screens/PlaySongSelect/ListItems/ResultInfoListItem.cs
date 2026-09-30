using System;

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;

using osuTK;

using ZeroV.Game.Graphics;
using ZeroV.Game.Graphics.Shapes;
using ZeroV.Game.Objects;
using ZeroV.Game.Scoring;
using ZeroV.Game.Utils;

namespace ZeroV.Game.Screens.PlaySongSelect.ListItems;

public sealed partial class ResultInfoListItem : CompositeDrawable {
    private readonly ResultInfo result;

    private readonly Int32 rank;

    public ResultInfoListItem(ResultInfo result, Int32 rank) {
        this.result = result;
        this.rank = rank;
        this.RelativeSizeAxes = Axes.X;
        this.Height = 48;
        this.Masking = true;
        this.BorderThickness = 1.5f;
        this.BorderColour = Colour4.DarkCyan;
    }

    [BackgroundDependencyLoader]
    private void load() {
        Colour4 resultColour = ZeroVColour.FromResult(this.result);
        Colour4 rankColour = ZeroVColour.FromRank(this.rank);
        this.InternalChildren = [
            new Box {
                RelativeSizeAxes = Axes.Both,
                Colour = Colour4.White,
            },
            //this.hoverOverlay = new Box {
            //    RelativeSizeAxes = Axes.Both,
            //    Colour = resultColour.Opacity(0.08f),
            //    Alpha = 0,
            //},
            new Box {
                Anchor = Anchor.CentreLeft,
                Origin = Anchor.CentreLeft,
                RelativeSizeAxes = Axes.Y,
                Width = 3,
                Colour = resultColour,
            },
            new Container {
                RelativeSizeAxes = Axes.Both,
                Padding = new MarginPadding(14),
                Children = [
                    // Left: Rank & Score
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
                                Size = new Vector2(18),
                                Colour = rankColour,
                            },
                            new ZeroVSpriteText {
                                Anchor = Anchor.CentreLeft,
                                Origin = Anchor.CentreLeft,
                                Colour = rankColour,
                                FontSize = 12,
                                Font = FontUsage.Default.With("Bold"),
                                Text = $"#{this.rank}",
                            },
                            new ZeroVSpriteText {
                                Anchor = Anchor.CentreLeft,
                                Origin = Anchor.CentreLeft,
                                Colour = rankColour,
                                FontSize = 18,
                                Font = FontUsage.Default.With("Bold"),
                                Text = this.result.Scoring.ToDisplayScoring(),
                            }
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
                            new ZeroVSpriteText {
                                Anchor = Anchor.CentreRight,
                                Origin = Anchor.CentreRight,
                                Colour = Colour4.DarkGray,
                                FontSize = 12,
                                Text = this.result.FinishTime.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                            },
                        ],
                    },
                ],
            },
        ];
    }
}
