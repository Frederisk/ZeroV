using System;

using osu.Framework.Allocation;
using osu.Framework.Audio.Track;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osu.Framework.Screens;

using osuTK.Graphics;
using osuTK.Input;

using ZeroV.Game.Elements.Particles;

namespace ZeroV.Game;

public partial class OffsetScreen : Screen {
    private Track? offsetBeatTrack;
    private BindableDouble offset = new(0);

    private BlinkParticle? line;
    private BlinkParticle? tempDisplayLine;
    private ZeroVSpriteText? offsetText;

    [BackgroundDependencyLoader]
    private void load(ITrackStore trackStore) {
        // FIXME: Add tracks
        // this.offsetBeatTrack = trackStore.Get("");
        this.offsetBeatTrack = trackStore.GetVirtual();
        this.offsetBeatTrack.Looping = true;

        this.line = new BlinkParticle(null!) {
            RelativePositionAxes = Axes.Both,
        };
        this.tempDisplayLine = new BlinkParticle(null!) {
            RelativePositionAxes = Axes.Both,
        };

        this.offsetText = new ZeroVSpriteText {
            Anchor = Anchor.Centre,
            Origin = Anchor.TopCentre,
            RelativePositionAxes = Axes.Both,
            Y = 0.25f,
            FontSize = 64,
            Text = this.offset.Value + "ms",
        };

        Drawable[] items = [
            // Visual offset
            new Container {
                RelativeSizeAxes = Axes.Both,
                Width = 0.8f,
                Height = 0.25f,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                CornerRadius = 32,
                Masking = true,
                Children = [
                    new Box {
                        RelativeSizeAxes = Axes.Both,
                        Colour = Color4.LightBlue,
                    },
                    new BlinkParticle(null!) {
                        RelativePositionAxes = Axes.Both,
                        X = -0.25f,
                        Alpha = 0.5f,
                    },
                    new BlinkParticle(null!) {
                        RelativePositionAxes = Axes.Both,
                        X = +0.25f,
                        Alpha = 0.5f,
                    },
                    new BlinkParticle(null!) {
                        RelativePositionAxes = Axes.Both,
                        X = 0.00f,
                        Alpha = 0.75f,
                    },
                    this.tempDisplayLine,
                    this.line,
                ],
            },
            // User interface
            // TODO: Add user interface
            this.offsetText,
        ];
        this.InternalChildren = items;
    }

    protected override void LoadComplete() {
        this.offset!.ValueChanged += this.offsetChanged;
        base.LoadComplete();
    }

    public override void OnEntering(ScreenTransitionEvent e) {
        this.offsetBeatTrack?.Start();
    }

    public override Boolean OnExiting(ScreenExitEvent e) {
        this.offsetBeatTrack?.Stop();
        return base.OnExiting(e);
    }

    /// <summary>
    /// Beat per minute.
    /// </summary>
    private const Double bpm = 128; // 128 bpm

    /// <summary>
    /// One beat length in milliseconds.
    /// </summary>
    private const Double beat_length = 60_000 / bpm; // 468.75 ms

    /// <summary>
    /// One bar length in milliseconds. Its value is 4 * <see cref="beat_length"/>.
    /// </summary>
    private const Double four_beats_bar = beat_length * 4; // 1875 ms

    /// <summary>
    /// Track delay in milliseconds. This value indicates when the first beat of the entire track starts.
    /// </summary>
    private const Double track_delay = 50; // 50 ms

    /// <summary>
    /// Real track beats time total.
    /// </summary>
    /// <remarks>
    /// track.time - offset - track_delay
    /// </remarks>
    private Double time => (this.offsetBeatTrack?.CurrentTime ?? 0) - this.offset.Value - track_delay;

    private Double timeSinceLastBeat => (this.time % four_beats_bar);

    /// <summary>
    /// 
    /// </summary>
    private Double relativePosition {
        get {
            // range: 0 ~ 1.0
            var relativePositionOfBar = this.timeSinceLastBeat / four_beats_bar;
            // range: -0.5 ~ +0.5
            return (relativePositionOfBar < 0.5) ? relativePositionOfBar : (relativePositionOfBar - 1);
        }
    }

    protected override void Update() {
        this.line!.X = Convert.ToSingle(this.relativePosition < 0.5 ? this.relativePosition : this.relativePosition - 1);
    }

    private void offsetChanged(ValueChangedEvent<Double> e) {
        // TODD: display offset
        this.offsetText!.Text = e.NewValue + "ms";
    }

    //protected override Boolean OnKeyDown(KeyDownEvent e) {
    //    if (e.Key == Key.Space) {
    //        BlinkParticle tempLine = new();
    //    }
    //}

    protected override void Dispose(Boolean isDisposing) {
        this.offsetBeatTrack?.Dispose();
        this.offset.ValueChanged -= this.offsetChanged;
        base.Dispose(isDisposing);
    }
}
