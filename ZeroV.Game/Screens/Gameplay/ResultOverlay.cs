using System;
using System.Collections.Generic;
using System.Globalization;

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;

using ZeroV.Game.Data;
using ZeroV.Game.Objects;
using ZeroV.Game.Scoring;

namespace ZeroV.Game.Screens.Gameplay;

public partial class ResultOverlay : OverlayContainer {
    public required Action OnQuit { get; init; }
    public required Action OnRetry { get; init; }

    protected const Int32 TRANSITION_DURATION = 1000;
    private const Single background_alpha = 0.75f;

    [Resolved]
    private IGameplayInfo screen { get; set; } = null!;

    [Resolved]
    private ResultInfoProvider resultInfoProvider { get; set; } = null!;

    private ZeroVSpriteText scoringNumber = null!;
    private BasicButton quitButton = null!;
    private BasicButton retryButton = null!;

    [BackgroundDependencyLoader]
    private void load() {
        this.RelativeSizeAxes = Axes.Both;
        this.scoringNumber = new ZeroVSpriteText() {
            Origin = Anchor.CentreRight,
            Anchor = Anchor.CentreRight,
            Y = 64,
            X = -24,
            FontSize = 128,
        };
        this.quitButton = new BasicButton {
            Origin = Anchor.CentreRight,
            Anchor = Anchor.CentreRight,
            Y = 180,
            X = -50,
            Height = 50,
            Width = 120,
            Text = "Next",
            Action = this.OnQuit,
        };
        this.retryButton = new BasicButton {
            Origin = Anchor.CentreRight,
            Anchor = Anchor.CentreRight,
            Y = 180,
            X = -170,
            Height = 50,
            Width = 120,
            Text = "Retry",
            Action = this.OnRetry,
        };

        this.InternalChildren = [
            new Box() {
                RelativeSizeAxes = Axes.Both,
                Colour = Colour4.Black,
                Alpha = background_alpha,
            },
            this.scoringNumber,
            this.quitButton,
            this.retryButton,
        ];
    }

    protected override void PopIn() {
        ScoringCalculator scoringCalculator = this.screen.ScoringCalculator;
        ResultInfo result = new() {
            UUID = this.screen.TrackInfo.UUID,
            GameVersion = this.screen.TrackInfo.GameVersion,
            Index = this.screen.MapInfo.Index,
            IsAllDone = scoringCalculator.IsAllDone,
            IsAllPerfect = scoringCalculator.IsAllPerfect,
            IsFullCombo = scoringCalculator.IsFullCombo,
            Scoring = scoringCalculator.Scoring,
        };
        List<ResultInfo> infoList = this.resultInfoProvider.Get() ?? [];
        infoList.Add(result);
        this.resultInfoProvider.Set(infoList);

        this.scoringNumber.Text = scoringCalculator.DisplayScoring.ToString(new String('0', 7), CultureInfo.InvariantCulture);
        this.scoringNumber.Colour = result switch {
            { IsAllPerfect: true } and { IsAllDone: true } => Colour4.Gold,
            { IsFullCombo: true } and { IsAllDone: true } => Colour4.Blue,
            { IsAllDone: false } => Colour4.Red,
            _ => Colour4.Wheat,
        };

        //String formatString = new ('0', 7);
        //LocalisableString formattedCount = this.ScoringCalculator.DisplayScoring.ToLocalisableString(formatString);
        //this.scoringNumber.Text = formattedCount;
        this.FadeIn(TRANSITION_DURATION, Easing.In);
    }

    protected override void PopOut() => this.FadeOut(TRANSITION_DURATION, Easing.Out);

    protected override Boolean OnTouchDown(TouchDownEvent e) => true;

    protected override Boolean OnClick(ClickEvent e) => true;
}
