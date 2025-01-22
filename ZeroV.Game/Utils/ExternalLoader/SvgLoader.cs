using System;
using System.IO;

using osu.Framework.Graphics.Rendering;
using osu.Framework.Graphics.Textures;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

using SkiaSharp;

using Svg.Skia;

namespace ZeroV.Game.Utils.ExternalLoader;
internal class SvgLoader : IDisposable {
    private Boolean disposedValue = false;
    private SKBitmap bitmap;
    private Image<Rgba32> image;
    private TextureUpload upload;

    public Texture? Texture { get; init; }

    public SvgLoader(FileInfo file, IRenderer renderer) {
        using SKSvg svg = new();
        svg.Load(file.FullName);
        if (svg.Picture is null) {
            throw new NullReferenceException(nameof(svg.Picture) + " is null."); ;
        }
        SKRect bounds = svg.Picture.CullRect;
        using SKBitmap bitmap = new((Int32)bounds.Width, (Int32)bounds.Height, SKColorType.Rgba8888, SKAlphaType.Premul);
        using SKCanvas canvas = new(bitmap);
        canvas.Clear(SKColors.Transparent);
        // canvas.Scale(1,1);
        // CullRect may not be at (0, 0)
        canvas.Translate(-bounds.Left, -bounds.Top);
        canvas.DrawPicture(svg.Picture);
        canvas.Flush();

        this.image = Image.LoadPixelData<Rgba32>(bitmap.GetPixelSpan(), bitmap.Width, bitmap.Height);
        this.upload = new TextureUpload(this.image);
        this.Texture = renderer.CreateTexture(this.image.Width, this.image.Height);
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
                this.image?.Dispose();
                this.bitmap?.Dispose();
            }
            this.disposedValue = true;
        }
    }
}
