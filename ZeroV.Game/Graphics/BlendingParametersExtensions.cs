using osu.Framework.Graphics;

namespace ZeroV.Game.Graphics;

public static class BlendingParametersExtensions {

    public static BlendingParameters TransparentAlphaMinus => new() {
        // Don't change the destination colour.
        RGBEquation = BlendingEquation.Add, // +
        Source = BlendingType.Zero, // 0
        Destination = BlendingType.One, // 1
        // Subtract the cover's alpha from the destination (points with alpha 1 should make the destination completely transparent).
        AlphaEquation = BlendingEquation.Add, // +
        SourceAlpha = BlendingType.Zero, // 0
        DestinationAlpha = BlendingType.OneMinusSrcAlpha, // 1-source
    };

    public static BlendingParameters TransparentAlphaAddWithColour => new() {
        RGBEquation = BlendingEquation.Add, // +
        Source = BlendingType.One, // 0
        Destination = BlendingType.Zero, // 1
        AlphaEquation = BlendingEquation.Add, // +
        SourceAlpha = BlendingType.Zero, // 0
        DestinationAlpha = BlendingType.SrcAlpha, // source
    };
}
