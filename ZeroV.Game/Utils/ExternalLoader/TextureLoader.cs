using System;
using System.IO;

using osu.Framework.Graphics.Rendering;
using osu.Framework.Graphics.Textures;
using osu.Framework.IO.Stores;
using osu.Framework.Platform;

namespace ZeroV.Game.Utils.ExternalLoader;

public class TextureLoader : IDisposable {
    private Boolean disposedValue = false;
    private StorageBackedResourceStore backedStore;
    private TextureLoaderStore loaderStore;
    private TextureStore store;

    public Texture? Texture { get; private set; }

    public TextureLoader(FileInfo file, IRenderer renderer, Boolean largeTexture) {
        NativeStorage storage = new(file.Directory!.FullName);
        this.backedStore = new(storage);
        this.loaderStore = new(this.backedStore);
        this.store = largeTexture ? new LargeTextureStore(renderer, this.loaderStore) : new TextureStore(renderer, this.loaderStore);
        // FIXME: Note, Texture may be null.
        this.Texture = this.store.Get(file.Name);
    }

    protected virtual void Dispose(Boolean disposing) {
        if (!this.disposedValue) {
            if (disposing) {
                this.Texture?.Dispose();
                this.store.Dispose();
                this.loaderStore.Dispose();
                this.backedStore.Dispose();
            }
            this.disposedValue = true;
        }
    }

    public void Dispose() {
        this.Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}

//public static Texture LoadTexture(FileInfo file, IRenderer renderer, Boolean largeTexture) {
//    NativeStorage storage = new(file.Directory!.FullName);
//    using StorageBackedResourceStore store = new(storage);
//    using TextureLoaderStore a = new(store);
//    using TextureStore c = largeTexture ? new LargeTextureStore(renderer, a) : new TextureStore(renderer, a);
//    return c.Get(file.Name);
//}
