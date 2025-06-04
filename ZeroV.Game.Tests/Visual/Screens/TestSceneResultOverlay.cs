using System;
using System.Collections.Generic;

using NUnit.Framework;

using osu.Framework.Allocation;
using osu.Framework.Audio.Track;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input;
using osu.Framework.Testing;

using osuTK;

using ZeroV.Game.Objects;
using ZeroV.Game.Scoring;
using ZeroV.Game.Screens.Gameplay;

namespace ZeroV.Game.Tests.Visual.Screens;

[TestFixture]
public partial class TestSceneResultOverlay : ZeroVTestScene {
    private ResultOverlay overlay = null!;

    private DependencyContainer? dependencies;

    protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent) =>
        this.dependencies = new DependencyContainer(base.CreateChildDependencies(parent));

    [BackgroundDependencyLoader]
    private void load() {
        this.dependencies!.CacheAs<IGameplayInfo>(new GameplayDemo());
    }

    [SetUpSteps]
    public void SetUpSteps() {
        this.AddStep("remove all", this.removeAll);
        this.AddStep("create overlay", this.createOverlay);
    }

    private void createOverlay() {
        this.Add(new Box() { RelativeSizeAxes = Axes.Both, Colour = Colour4.White });
        this.Add(this.overlay = new ResultOverlay { OnQuit = () => { }, OnRetry = () => { } });
        this.overlay.Show();
    }

    private void removeAll() {
        this.Clear();
    }

    private class GameplayDemo : IGameplayInfo {

        public TrackInfo TrackInfo => new() {
            UUID = Guid.Empty,
            GameVersion = new Version(1, 0, 0, 0),

            Album = default,
            Artists = default,
            BeatmapFile = default!,
            Description = default,
            FileOffset = default,
            GameAuthor = default!,
            MapInfos = default!,
            Title = "Sample Music",
            TrackFile = default!,
            TrackOrder = default,
            BackgroundFile = default,
        };

        public MapInfo MapInfo => new() {
            Index = 0,

            Difficulty = 3.14,
            BlinkCount = default,
            PressCount = default,
            SlideCount = default,
            StrokeCount = default
        };

        public Track GameplayTrack => throw new NotImplementedException();

        public ScoringCalculator ScoringCalculator {
            get {
                ScoringCalculator calculator = new(7);
                calculator.AddTarget(TargetResult.MaxPerfect);
                return calculator;
            }
        }

        public Double ParticleFallingTime => throw new NotImplementedException();

        public Double ParticleFadingTime => throw new NotImplementedException();

        public IReadOnlyDictionary<TouchSource, Vector2> TouchPositions => throw new NotImplementedException();

        public event IGameplayInfo.TouchUpdateDelegate? TouchUpdate;
    }
}
