using System;

using osu.Framework.Allocation;

using TriangleShape = osu.Framework.Graphics.Shapes.Triangle;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Rendering;
using osu.Framework.Graphics.Sprites;

using osuTK;

namespace ZeroV.Game.Graphics.Shapes;

/// <summary>
/// Represents a sprite that is drawn in a triangle shape, and its top direction can be specified.
/// </summary>
/// <remarks>
/// This class is a modified version of <see cref="TriangleShape"/> from osu.Framework.
/// TODO: Submit a PR to osu.Framework to modify <see cref="TriangleShape"/> to allow overriding <c>toTriangle</c> method.
/// </remarks>
internal partial class OrientedTriangle : Sprite {

    /// <summary>
    /// Creates a new triangle with a white pixel as texture.
    /// </summary>
    public OrientedTriangle(Orientation orientation) {
        // Setting the texture would normally set a size of (1, 1), but since the texture is set from BDL it needs to be set here instead.
        // RelativeSizeAxes may not behave as expected if this is not done.
        this.Size = Vector2.One;

        this.ToTriangle = orientation switch {
            Orientation.Up => q => new Triangle((q.TopLeft + q.TopRight) / 2, q.BottomLeft, q.BottomRight),
            Orientation.Down => q => new Triangle((q.BottomLeft + q.BottomRight) / 2, q.TopLeft, q.TopRight),
            Orientation.Left => q => new Triangle((q.TopLeft + q.BottomLeft) / 2, q.TopRight, q.BottomRight),
            Orientation.Right => q => new Triangle((q.TopRight + q.BottomRight) / 2, q.TopLeft, q.BottomLeft),
            _ => throw new ArgumentOutOfRangeException(nameof(orientation), orientation, null),
        };
    }

    [BackgroundDependencyLoader]
    private void load(IRenderer renderer) {
        this.Texture ??= renderer.WhitePixel;
    }

    public override RectangleF BoundingBox => this.ToTriangle(this.ToParentSpace(this.LayoutRectangle)).AABBFloat;

    protected Func<Quad, Triangle> ToTriangle;

    public override Boolean Contains(Vector2 screenSpacePos) => this.ToTriangle(this.ScreenSpaceDrawQuad).Contains(screenSpacePos);

    protected override DrawNode CreateDrawNode() => new TriangleDrawNode(this);

    private class TriangleDrawNode(OrientedTriangle source) : SpriteDrawNode(source) {

        protected override void Blit(IRenderer renderer) {
            if (this.DrawRectangle.Width == 0 || this.DrawRectangle.Height == 0) {
                return;
            }

            renderer.DrawTriangle(this.Texture, source.ToTriangle(this.ScreenSpaceDrawQuad), this.DrawColourInfo.Colour, null, null,
                new Vector2(this.InflationAmount.X / this.DrawRectangle.Width, InflationAmount.Y / this.DrawRectangle.Height), this.TextureCoords);
        }

        protected override void BlitOpaqueInterior(IRenderer renderer) {
            if (this.DrawRectangle.Width == 0 || this.DrawRectangle.Height == 0) {
                return;
            }

            Triangle triangle = source.ToTriangle(this.ConservativeScreenSpaceDrawQuad);

            if (renderer.IsMaskingActive) {
                renderer.DrawClipped(ref triangle, this.Texture, this.DrawColourInfo.Colour);
            } else {
                renderer.DrawTriangle(this.Texture, triangle, this.DrawColourInfo.Colour);
            }
        }
    }

    public enum Orientation {
        Up,
        Down,
        Left,
        Right,
    }
}
