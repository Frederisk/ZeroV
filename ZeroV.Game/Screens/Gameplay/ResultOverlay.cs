using System;
using System.Globalization;

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;

using ZeroV.Game.Scoring;

namespace ZeroV.Game.Screens.Gameplay;
public partial class ResultOverlay : OverlayContainer {

    protected const Int32 TRANSITION_DURATION = 1000;
    private const Single background_alpha = 0.75f;

    [Resolved]
    private ScoringCalculator scoringCalculator { get; set; } = null!;

    private ZeroVSpriteText scoringNumber = null!;

    [BackgroundDependencyLoader]
    private void load() {
        this.RelativeSizeAxes = Axes.Both;
        this.scoringNumber = new ZeroVSpriteText() {
            Origin = Anchor.CentreRight,
            Anchor = Anchor.CentreRight,
            Y = 64,
            X = -24,
            FontSize = 128,
            //Shadow = true,
            //ShadowColour = Colour4.Black,
            //ShadowOffset = new(16),
        };
        this.InternalChildren = [
            new Box() {
                RelativeSizeAxes = Axes.Both,
                Colour = Colour4.Black,
                Alpha = background_alpha,
            },
            this.scoringNumber,
        ];
    }

    protected override void PopIn() {
        this.scoringNumber.Text = this.scoringCalculator.DisplayScoring.ToString(new String('0', 7), CultureInfo.InvariantCulture);
        this.scoringNumber.Colour = this.scoringCalculator switch {
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

    //protected override Boolean OnClick(ClickEvent e) => true;
}
