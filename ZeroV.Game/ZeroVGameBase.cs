using System;

using osu.Framework.Allocation;
using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Textures;
using osu.Framework.IO.Stores;
using osu.Framework.Platform;

using osuTK;

using ZeroV.Game.Configs;
using ZeroV.Game.Data;
using ZeroV.Game.Data.KeyValueStorage;
using ZeroV.Game.Graphics.Textures;
using ZeroV.Game.Utils;
using ZeroV.Resources;

namespace ZeroV.Game;

/// <summary>
/// The most basic <see cref="Game"/> that can be used to host osu! components and systems.
/// This class will not load any kind of UI, allowing it to be used for provide dependencies to test cases without interfering with them.
/// </summary>
public partial class ZeroVGameBase : osu.Framework.Game {
    // Anything in this class is shared between the test browser and the game implementation.
    // It allows for caching global dependencies that should be accessible to tests, or changing
    // the screen scaling for all components including the test browser and framework overlays.

    protected override Container<Drawable> Content { get; }

    private DependencyContainer dependencies = null!;

    protected ZeroVGameBase() {
        // Ensure game and tests scale with window size and screen DPI.
        base.Content.Add(this.Content = new DrawSizePreservingFillContainer {
            // You may want to change TargetDrawSize to your "default" resolution, which will decide how things scale and position when using absolute coordinates.
            TargetDrawSize = new Vector2(ZeroVMath.SCREEN_DRAWABLE_X, ZeroVMath.SCREEN_DRAWABLE_Y),
        });
    }

    [BackgroundDependencyLoader]
    private void load(Storage storage, FrameworkConfigManager frameworkConfigManager) {
        this.Resources.AddStore(new DllResourceStore(ZeroVResources.ResourceAssembly));

        this.dependencies.CacheAs<ZeroVGameBase>(this);
        this.dependencies.CacheAs<ZeroVConfigManager>(new ZeroVConfigManager(storage));
        JsonKeyValueStorage jsonKeyValueStorage = new(storage);
        //this.dependencies.CacheAs<IKeyValueStorage>(jsonKeyValueStorage);
        this.dependencies.CacheAs<TrackInfoProvider>(new TrackInfoProvider(jsonKeyValueStorage));
        this.dependencies.CacheAs<ResultInfoProvider>(new ResultInfoProvider(jsonKeyValueStorage));

        LargeTextureStore largeStore = new(this.Host.Renderer, this.Host.CreateTextureLoaderStore(new NamespacedResourceStore<Byte[]>(this.Resources, @"Textures")), this.DefaultTextureFilteringMode);
        largeStore.AddTextureSource(this.Host.CreateTextureLoaderStore(new OnlineStore()));
        this.dependencies.CacheAs<LargeTextureStore>(largeStore);

        IResourceStore<TextureUpload> svgTextureLoaderStore = new SvgTextureLoaderStore(new NamespacedResourceStore<Byte[]>(this.Resources, @"SvgTexture"));
        this.Textures.AddTextureSource(svgTextureLoaderStore);
    }

    protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent) =>
        this.dependencies = new DependencyContainer(base.CreateChildDependencies(parent));
}
