using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Platform;

using ZeroV.Game.Data;
using ZeroV.Game.Objects;
using ZeroV.Game.Overlays;

namespace ZeroV.Game.Tests.Visual.Overlays;

[TestFixture]
public partial class TestNowPlayingOverlay : ZeroVTestScene {
    private DependencyContainer? dependencies;
    private NowPlayingOverlay nowPlayingOverlay = null!;

    [BackgroundDependencyLoader]
    private void load(Storage storage) {
        this.dependencies!.CacheAs<TrackInfoProvider>(new FakeTrackInfoProvider(storage));

        this.nowPlayingOverlay = new() {
            RelativeSizeAxes = Axes.Both
        };
        this.Add(this.nowPlayingOverlay);
    }

    protected override void LoadComplete() {
        base.LoadComplete();
        this.nowPlayingOverlay.Show();
    }

    protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent) =>
        this.dependencies = new DependencyContainer(base.CreateChildDependencies(parent));

    private class FakeTrackInfoProvider(Storage storage) : TrackInfoProvider(storage) {
        public override IReadOnlyList<TrackInfo> TrackInfoList => [
            new() {
                Title = "Test Title",
                Album = "Test Album",
                TrackOrder = 0,
                Artists = "Test Artists",
                FileOffset = TimeSpan.FromSeconds(1),
                GameAuthor = "Test GameAuthor",
                Description = "Test Description",
                GameVersion = new Version(1,0,0),
                Maps = [],
                File = new System.IO.FileInfo("testFileName.xml")
            }
        ];
    }
}
