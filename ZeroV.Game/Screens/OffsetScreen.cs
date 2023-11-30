using System;

using osu.Framework.Allocation;
using osu.Framework.Audio.Track;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Screens;

using osuTK;
using osuTK.Graphics;

using ZeroV.Game.Elements.Particles;

namespace ZeroV.Game;

public partial class OffsetScreen : Screen {
    private Track? offsetBeatTrack;

    [BackgroundDependencyLoader]
    private void load(ITrackStore trackStore) {
        // FIXME: Add tracks
        // this.offsetBeatTrack = trackStore.Get("");
        // this.offsetBeatTrack.Looping = true;
        Drawable[] items = [
            new Container{
                RelativeSizeAxes = Axes.Both,
                Width = 0.8f,
                Height = 0.25f,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                // CornerRadius
                // Masking=
                Children = [
                    new Box {
                        RelativeSizeAxes = Axes.Both,
                        Colour = Color4.Green,
                    },
                    new BlinkParticle(null!) {
                        RelativePositionAxes = Axes.Both,
                        X = -0.25f,
                        Alpha = 0.5f,
                    },
                    new BlinkParticle(null!) {
                        RelativePositionAxes = Axes.Both,
                        X = 0.25f,
                        Alpha = 0.5f,
                    },
                    new BlinkParticle(null!) {
                        RelativePositionAxes = Axes.Both,
                        Alpha = 0.5f,
                    },
                ],
            }
        ];
        this.InternalChildren = items;
    }
}
