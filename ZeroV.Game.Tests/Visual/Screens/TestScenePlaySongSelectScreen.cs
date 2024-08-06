using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using NUnit.Framework;

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Platform;
using osu.Framework.Screens;
using osu.Framework.Testing;

using ZeroV.Game.Data;
using ZeroV.Game.Data.KeyValueStorage;
using ZeroV.Game.Objects;
using ZeroV.Game.Screens;

namespace ZeroV.Game.Tests.Visual.Screens;

[TestFixture]
public partial class TestScenePlaySongSelectScreen : ZeroVTestScene {
    private ScreenStack screenStack = default!;

    [BackgroundDependencyLoader]
    private void load(Storage storage) {
        IKeyValueStorage keyValueStorage = new JsonKeyValueStorage(storage);
        this.dependencies!.CacheAs<TrackInfoProvider>(new FakeTrackInfoProvider(keyValueStorage));
        this.Add(this.screenStack = new ScreenStack() { RelativeSizeAxes = Axes.Both });
    }

    [SetUpSteps]
    public void SetUpSteps() {
        this.AddStep("create screen", this.createScreen);
    }

    private void createScreen() {
        this.screenStack.Push(new PlaySongSelectScreen() { RelativeSizeAxes = Axes.Both });
    }

    private DependencyContainer? dependencies;
    protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent) =>
        this.dependencies = new DependencyContainer(base.CreateChildDependencies(parent));

    private class FakeTrackInfoProvider(IKeyValueStorage storage) : TrackInfoProvider(storage) {
        public override IReadOnlyList<TrackInfo>? Get() => this.trackInfoList;
        //public override Task<IReadOnlyList<TrackInfo>?> GetAsync() => Task.FromResult(this.trackInfoList);

        private IReadOnlyList<TrackInfo>? trackInfoList => [
            new() {
                Title = "A - Test Title",
                UUID = Guid.Empty,
                Album = "Test Album",
                TrackOrder = 0,
                Artists = "Test Artists",
                FileOffset = TimeSpan.FromSeconds(1),
                GameAuthor = "Test GameAuthor",
                Description = "Test Description",
                GameVersion = new Version(1,0,0),
                MapInfos = [
                    new MapInfo() {
                        Difficulty = 1,
                        //MapOffset = TimeSpan.FromSeconds(2),
                        Index = -114514,
                        BlinkCount = 1,
                        PressCount = 1,
                        SlideCount = 1,
                        StrokeCount = 1
                    },
                    new MapInfo() {
                        Difficulty = 2,
                        //MapOffset = TimeSpan.FromSeconds(2),
                        Index = 1919810,
                        BlinkCount = 1,
                        PressCount = 1,
                        SlideCount = 1,
                        StrokeCount = 1
                    }
                ],
                BeatmapFile = new System.IO.FileInfo("testFileName.xml"),
                TrackFile = new System.IO.FileInfo("testFileName.wav")
            },
            new() {
                Title = "C - Test Title3",
                UUID = Guid.Empty,
                Album = null,
                TrackOrder = 0,
                Artists = null,
                FileOffset = TimeSpan.FromSeconds(1),
                GameAuthor = "Test GameAuthor",
                Description = "Test Description",
                GameVersion = new Version(1,0,0),
                MapInfos = [
                    new MapInfo() {
                        Difficulty = 1,
                        //MapOffset = TimeSpan.FromSeconds(2),
                        Index = 0,
                        BlinkCount = 1,
                        PressCount = 1,
                        SlideCount = 1,
                        StrokeCount = 1
                    }],
                BeatmapFile = new System.IO.FileInfo("testFileName.xml"),
                TrackFile = new System.IO.FileInfo("testFileName.flac")
            },
            new() {
                Title = "B - Test Title2",
                UUID = Guid.Empty,
                Album = "Test Album",
                TrackOrder = 0,
                Artists = "Test Artists",
                FileOffset = TimeSpan.FromSeconds(1),
                GameAuthor = "Test GameAuthor",
                Description = "Test Description",
                GameVersion = new Version(1,0,0),
                MapInfos = [
                    new MapInfo() {
                        Difficulty = 1,
                        //MapOffset = TimeSpan.FromSeconds(2),
                        Index = 1,
                        BlinkCount = 1,
                        PressCount = 1,
                        SlideCount = 1,
                        StrokeCount = 1
                    }],
                BeatmapFile = new System.IO.FileInfo("testFileName.xml"),
                TrackFile = new System.IO.FileInfo("testFileName.flac")
            },
            new() {
                Title = "D - Test Looooooooooooooooooong Title2",
                UUID = Guid.Empty,
                Album = "Test Looooooooooooooooooong Album",
                TrackOrder = 0,
                Artists = "Test Looooooooooooooooooong Artists",
                FileOffset = TimeSpan.FromSeconds(1),
                GameAuthor = "Test GameAuthor",
                Description = "Test Description",
                GameVersion = new Version(1,0,0),
                MapInfos = [
                    new MapInfo() {
                        Difficulty = 1,
                        //MapOffset = TimeSpan.FromSeconds(2),
                        Index = -1,
                        BlinkCount = 1,
                        PressCount = 1,
                        SlideCount = 1,
                        StrokeCount = 1
                    }],
                BeatmapFile = new System.IO.FileInfo("testFileName.xml"),
                TrackFile = new System.IO.FileInfo("testFileName.flac")
            }
        ];
    }
}
