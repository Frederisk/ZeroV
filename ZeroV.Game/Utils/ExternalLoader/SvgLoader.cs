using System;
using System.IO;

using osu.Framework.Graphics.Rendering;
using osu.Framework.Graphics.Textures;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

using ZeroV.Game.Graphics.Textures;

namespace ZeroV.Game.Utils.ExternalLoader;

internal class SvgLoader : IDisposable {
    private Boolean disposedValue = false;

    //private SKBitmap bitmap;
    private Image<Rgba32> image;

    private TextureUpload upload;

    public Texture? Texture { get; init; }

    /// <summary>
    /// Load an SVG file and convert it to a Texture.
    /// </summary>
    /// <param name="file">The SVG file to load.</param>
    /// <param name="renderer">The renderer to create the Texture.</param>
    /// <param name="size">The Rasterization size. When it's <see langword="null"/>, the size specified by SVG itself will be used.</param>
    public SvgLoader(FileInfo file, IRenderer renderer, osuTK.Vector2? size = null) {
        this.image = TextureUploadExtensions.LoadFromSvgStream<Rgba32>(file.OpenRead(), size);
        // Image will be disposed after TextureUpload is disposed.
        this.upload = new TextureUpload(this.image);
        this.Texture = renderer.CreateTexture(this.image.Width, this.image.Height);
        // The provided upload will be disposed after the upload is completed.
        this.Texture.SetData(this.upload);
    }

    public void Dispose() {
        this.Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(Boolean disposing) {
        if (!this.disposedValue) {
            if (disposing) {
                this.Texture?.Dispose();
                //this.upload?.Dispose();
                //this.image?.Dispose();
            }
            this.disposedValue = true;
        }
    }
}
