using System;
using System.Collections.Generic;

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;

using osuTK;

using ZeroV.Game.Graphics;

using ZeroV.Game.Graphics.Shapes;
using ZeroV.Game.Objects;
using ZeroV.Game.Screens.PlaySongSelect.ListItems;

namespace ZeroV.Game.Screens.PlaySongSelect;

public sealed partial class LeaderboardPanelCard : CompositeDrawable {
    private ScrollContainer<FillFlowContainer<ResultInfoListItem>> scoringRankListScroller = null!;

    private FillFlowContainer<ResultInfoListItem> scoringRankList = null!;

    private CardEmptyPlaceholder emptyLeaderboardPlaceholder = null!;

    public LeaderboardPanelCard() {
        this.Masking = true;
        this.BorderThickness = 1.5f;
        this.BorderColour = Colour4.Cyan;
    }

    [BackgroundDependencyLoader]
    private void load() {
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
            Size = new Vector2(0.95f, 0.85f),
            Child = this.scoringRankList,
        };
        this.emptyLeaderboardPlaceholder = new CardEmptyPlaceholder("NO RECORDS YET", "Play this chart to set your high score!");
        this.InternalChildren = [
            new Box {
                RelativeSizeAxes = Axes.Both,
                Colour = Colour4.White,
            },
            new FillFlowContainer {
                RelativeSizeAxes = Axes.Both,
                Direction = FillDirection.Vertical,
                Padding = new MarginPadding(16),
                Spacing = new Vector2(0, 10),
                // Leaderboard Header
                Children = [
                    new FillFlowContainer {
                        RelativeSizeAxes = Axes.X,
                        AutoSizeAxes = Axes.Y,
                        Direction = FillDirection.Horizontal,
                        //Spacing = new Vector2(8, 0),
                        Children = [
                            new Diamond {
                                Anchor = Anchor.CentreLeft,
                                Origin = Anchor.CentreLeft,
                                Size = new Vector2(10),
                                Colour = Colour4.Cyan,
                                Margin = new MarginPadding { Horizontal = 8 },
                            },
                            new ZeroVSpriteText {
                                Anchor = Anchor.CentreLeft,
                                Origin = Anchor.CentreLeft,
                                FontSize = 15,
                                Font = FontUsage.Default.With(weight: "Bold"),
                                Colour = Colour4.Black,
                                Text = "TOP RECORDS",
                            },
                        ],
                    },
                    // Divider
                    new Box {
                        RelativeSizeAxes = Axes.X,
                        Height = 1.5f,
                        Colour = Colour4.Cyan,
                    },
                    // List Scroll Container
                    new Container {
                        RelativeSizeAxes = Axes.Both,
                        Child = this.scoringRankListScroller,
                    },
                ],
            },
            // Empty state placeholder
            this.emptyLeaderboardPlaceholder,
        ];
    }

    public void ClearDisplay() {
        this.emptyLeaderboardPlaceholder.FadeIn(150, Easing.In);
        this.scoringRankList.Clear();
    }

    public void UpdateDisplay(IReadOnlyList<ResultInfo> resultInfoList) {
        this.scoringRankList.Clear();

        if (resultInfoList.Count > 0) {
            this.emptyLeaderboardPlaceholder.FadeOut(100, Easing.Out);

            for (Int32 i = 0; i < resultInfoList.Count; i++) {
                this.scoringRankList.Add(new ResultInfoListItem(resultInfoList[i], i + 1));
            }
            //foreach (ResultInfo resultInfo in topResultInfos) {
            //    this.scoringRankList.Add(new ResultInfoListItem(resultInfo));
            //}
        } else {
            this.emptyLeaderboardPlaceholder.FadeIn(150, Easing.In);
        }
    }
}
