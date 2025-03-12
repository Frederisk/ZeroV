using System;
using System.Collections.Generic;

using NUnit.Framework;

using osu.Framework.Allocation;
using osu.Framework.Audio.Track;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics;
using osu.Framework.Input;
using osu.Framework.Testing;

using osuTK;

using ZeroV.Game.Elements.Particles;
using ZeroV.Game.Objects;
using ZeroV.Game.Scoring;
using ZeroV.Game.Screens.Gameplay;

namespace ZeroV.Game.Tests.Visual.Elements;

[TestFixture]
public partial class TestSceneParticles : ZeroVTestScene {

    private DependencyContainer? dependencies;

    protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent) =>
        this.dependencies = new DependencyContainer(base.CreateChildDependencies(parent));

    [BackgroundDependencyLoader]
    private void load() {
        this.dependencies!.CacheAs<IGameplayInfo>(new GameplayDemo());

        this.ChangeBackgroundColour(Colour4.White);
        PressParticle pressParticle = new PressParticle {
            Y = 0,
            X = 320,
        };
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
            //new PressParticle() {
            //    Y = 0,
            //    X = 320,
            //    //Height = 256,
            //    //Source = new PressParticleSource(0, 1024),
            //},
            pressParticle,
        ];
        particles.ForEach(this.Add);
        pressParticle.SetupLength(0, 256);
    }

    private class GameplayDemo : IGameplayInfo {
        public TrackInfo TrackInfo => throw new NotImplementedException();

        public MapInfo MapInfo => throw new NotImplementedException();

        public Track GameplayTrack => throw new NotImplementedException();

        public ScoringCalculator ScoringCalculator => throw new NotImplementedException();

        public Double ParticleFallingTime => 1024;

        public Double ParticleFadingTime => 1024;

        public IReadOnlyDictionary<TouchSource, Vector2> TouchPositions => throw new NotImplementedException();

        public event IGameplayInfo.TouchUpdateDelegate? TouchUpdate;
    }
}
