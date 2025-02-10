using System;
using System.IO;

using osu.Framework.Graphics.Textures;
using osu.Framework.IO.Stores;

using SixLabors.ImageSharp;

namespace ZeroV.Game.Graphics.Textures;

public class SvgTextureLoaderStore(IResourceStore<Byte[]> store) : TextureLoaderStore(store) {
    protected override Image<TPixel> ImageFromStream<TPixel>(Stream stream) =>
        TextureUploadExtensions.LoadFromSvgStream<TPixel>(stream);
}
