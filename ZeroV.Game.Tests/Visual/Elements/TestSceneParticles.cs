using osu.Framework.Allocation;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics;

using osuTK.Graphics;

using ZeroV.Game.Elements.Particles;

namespace ZeroV.Game.Tests.Visual.Elements;

public partial class TestSceneParticles : ZeroVTestScene {

    [BackgroundDependencyLoader]
    private void load() {
        this.ChangeBackgroundColour(Color4.White);
        Drawable[] particles = [
            new BlinkParticle(null!) {
                Y = -128,
                X = 0,
            },
            new SlideParticle(null!) {
                Y = 0,
                X = -192,
                Direction = SlideParticle.SlidingDirection.Up,
            },
            new SlideParticle(null!) {
                Y = 0,
                X = -64,
                Direction = SlideParticle.SlidingDirection.Right,
            },
            new SlideParticle(null!) {
                Y = 0,
                X = 64,
                Direction = SlideParticle.SlidingDirection.Down,
            },
            new SlideParticle(null!) {
                Y = 0,
                X = 192,
                Direction = SlideParticle.SlidingDirection.Left,
            },
        ];
        particles.ForEach(this.Add);
    }
}
