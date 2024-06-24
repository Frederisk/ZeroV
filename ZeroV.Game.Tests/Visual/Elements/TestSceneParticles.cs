using NUnit.Framework;

using osu.Framework.Allocation;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics;

using ZeroV.Game.Elements.Particles;

namespace ZeroV.Game.Tests.Visual.Elements;

[TestFixture]
public partial class TestSceneParticles : ZeroVTestScene {

    [BackgroundDependencyLoader]
    private void load() {
        this.ChangeBackgroundColour(Colour4.White);
        Drawable[] particles = [
            new BlinkParticle() {
                Y = -128,
                X = 0,
            },
            new SlideParticle() {
                Y = 0,
                X = -192,
                Direction = SlidingDirection.Up,
            },
            new SlideParticle() {
                Y = 0,
                X = -64,
                Direction = SlidingDirection.Right,
            },
            new SlideParticle() {
                Y = 0,
                X = 64,
                Direction = SlidingDirection.Down,
            },
            new SlideParticle() {
                Y = 0,
                X = 192,
                Direction = SlidingDirection.Left,
            },
            new StrokeParticle() {
                Y = 128,
                X = 0,
            },
            // FIXME: PressParticle need a GameScreen dependency.
            //new PressParticle() {
            //    Y = 0,
            //    X = 320,
            //    Height = 256,
            //},
        ];
        particles.ForEach(this.Add);
    }
}
