using System;
using System.IO;

using osu.Framework.Graphics.Textures;
using osu.Framework.IO.Stores;

using SixLabors.ImageSharp;

using SkiaSharp;

using Svg.Skia;

namespace ZeroV.Game.Graphics.Textures;

public class SvgTextureLoaderStore : TextureLoaderStore {

    public SvgTextureLoaderStore(IResourceStore<Byte[]> store) : base(store) {
    }

    protected override Image<TPixel> ImageFromStream<TPixel>(Stream stream) {
        using SKSvg svg = new();
        svg.Load(stream);
        SKRect bounds = svg.Picture!.CullRect;
        using SKBitmap bitmap = new((Int32)bounds.Width, (Int32)bounds.Height, SKColorType.Rgba8888, SKAlphaType.Premul);
        using SKCanvas canvas = new(bitmap);
        canvas.Clear(SKColors.Transparent);
        canvas.Translate(-bounds.Left, -bounds.Top);
        canvas.DrawPicture(svg.Picture);
        canvas.Flush();
        // The PixelSpan of the bitmap will be copied to the Image, so it's safe to dispose the bitmap.
        return Image.LoadPixelData<TPixel>(bitmap.GetPixelSpan(), bitmap.Width, bitmap.Height);
    }
}
