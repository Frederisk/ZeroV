using System;
using System.IO;

using osu.Framework.Graphics.Rendering;
using osu.Framework.Graphics.Textures;

using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using osu.Framework.Logging;

namespace ZeroV.Game.Utils.ExternalLoader;

public class TextureLoader : IDisposable {
    private Boolean disposedValue = false;
    private Image<Rgba32>? image;
    private TextureUpload? upload;
    public Texture? Texture { get; private set; }

    public TextureLoader(FileInfo? file, IRenderer renderer) {
        try {
            ArgumentNullException.ThrowIfNull(file);
            this.image = Image.Load<Rgba32>(file.FullName);
            this.upload = new TextureUpload(this.image);
            this.Texture = renderer.CreateTexture(this.image.Width, this.image.Height);
            this.Texture.SetData(this.upload);
        } catch (Exception e) {
            //Console.WriteLine(e);
            Logger.Log($"Failed to load texture from {file?.FullName ?? nameof(file)}. Message: {e.Message}", LoggingTarget.Runtime, LogLevel.Important);
            this.Texture = null;
        }
    }

    public void Dispose() {
        this.Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    public void Dispose(Boolean disposing) {
        if (!this.disposedValue) {
            if (disposing) {
                this.Texture?.Dispose();
                this.upload?.Dispose();
                this.image?.Dispose();
            }
            this.disposedValue = true;
        }
    }
}

//public static Texture LoadTexture(FileInfo file, IRenderer renderer, Boolean largeTexture) {
//    NativeStorage storage = new(file.Directory!.FullName);
//    using StorageBackedResourceStore store = new(storage);
//    using TextureLoaderStore a = new(store);
//    using TextureStore c = largeTexture ? new LargeTextureStore(renderer, a) : new TextureStore(renderer, a);
//    return c.Get(file.Name);
//}
