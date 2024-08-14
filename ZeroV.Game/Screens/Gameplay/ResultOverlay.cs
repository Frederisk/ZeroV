using System;

using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;

namespace ZeroV.Game.Screens.Gameplay;
public partial class ResultOverlay : OverlayContainer {

    protected const Int32 TRANSITION_DURATION = 200;

    protected override void PopIn() => this.FadeIn(TRANSITION_DURATION, Easing.In);

    protected override void PopOut() => this.FadeOut(TRANSITION_DURATION, Easing.In);
}
