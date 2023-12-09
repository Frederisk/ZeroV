using System;

using osu.Framework.Allocation;
using osu.Framework.Audio.Track;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Screens;

using osuTK.Graphics;

using ZeroV.Game.Elements.Particles;

namespace ZeroV.Game;

public partial class OffsetScreen : Screen {
    private Track? offsetBeatTrack;
    private BindableDouble offset = new(0);

    private BlinkParticle? line;

    [BackgroundDependencyLoader]
    private void load(ITrackStore trackStore) {
        // FIXME: Add tracks
        // this.offsetBeatTrack = trackStore.Get("");
        this.offsetBeatTrack = trackStore.GetVirtual();
        this.offsetBeatTrack.Looping = true;

        this.line = new BlinkParticle(null!) {
            RelativePositionAxes = Axes.Both,
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
                Masking= true,
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
                        X = 00.00f,
                        Alpha = 0.75f,
                    },
                    this.line,
                ],
            }
            // User interface
            // TODO: Add user interface
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

    private const Double bpm = 128;
    private const Double beat_length = 60000 / bpm;
    private const Double fourth_beat = beat_length * 4;
    private Double time => (this.offsetBeatTrack?.CurrentTime ?? 0) - this.offset.Value - 50;
    private Double timeSinceLastBeat => (this.time % fourth_beat);
    private Double progress => this.timeSinceLastBeat / fourth_beat;
    protected override void Update() {
        this.line!.X = Convert.ToSingle(this.progress < .5 ? this.progress : progress - 1);

    }
    private void offsetChanged(ValueChangedEvent<Double> e) {
        // TODD: display offset
        // this.line?.MoveToX((Single) e.NewValue, 100, Easing.OutQuint);
    }

    protected override void Dispose(Boolean isDisposing) {
        this.offsetBeatTrack?.Dispose();
        this.offset.ValueChanged -= this.offsetChanged;
        base.Dispose(isDisposing);
    }

}
