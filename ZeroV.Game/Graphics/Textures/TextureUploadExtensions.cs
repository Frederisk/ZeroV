using System;
using System.IO;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

using SkiaSharp;

using Svg.Skia;

namespace ZeroV.Game.Graphics.Textures;

public static class TextureUploadExtensions {

    public static Image<TPixel> LoadFromSvgStream<TPixel>(Stream stream, osuTK.Vector2? size = null) where TPixel : unmanaged, IPixel<TPixel> {
        using SKSvg svg = new();
        svg.Load(stream);
        //if (svg.Picture is null) {
        //    throw new NullReferenceException(nameof(svg.Picture) + " is null."); ;
        //}
        SKRect bounds = svg.Picture!.CullRect;
        (Single realSizeX, Single realSizeY) = size is null ? (bounds.Width, bounds.Height) : (size.Value.X, size.Value.Y);
        (Single scaleX, Single scaleY) = size is null ? (1, 1) : (size.Value.X / bounds.Width, size.Value.Y / bounds.Height);
        using SKBitmap bitmap = new((Int32)realSizeX, (Int32)realSizeY, SKColorType.Rgba8888, SKAlphaType.Premul);
        using SKCanvas canvas = new(bitmap);
        canvas.Clear(SKColors.Transparent);
        // CullRect may not be at (0, 0)
        canvas.Translate(-bounds.Left, -bounds.Top);
        canvas.Scale(scaleX, scaleY);
        canvas.DrawPicture(svg.Picture);
        canvas.Flush();
        // The PixelSpan of the bitmap will be copied to the Image, so it's safe to dispose the bitmap.
        return Image.LoadPixelData<TPixel>(bitmap.GetPixelSpan(), bitmap.Width, bitmap.Height);
    }
}
