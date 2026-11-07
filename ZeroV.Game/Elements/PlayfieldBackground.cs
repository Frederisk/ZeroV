using System;

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;

using osuTK;

namespace ZeroV.Game.Elements;

internal partial class PlayfieldBackground : Container {
    private Texture? texture;
    private BufferedContainer? backSprite;
    private Sprite? foreSprite;

    public PlayfieldBackground(Texture? texture) {
        this.RelativeSizeAxes = Axes.Both;
        this.texture = texture;
    }

    [BackgroundDependencyLoader]
    private void load() {
        if (this.texture is null) {
            this.foreSprite = new Box {
                RelativeSizeAxes = Axes.Both,
                Colour = Colour4.LightBlue,
                FillMode = FillMode.Fill,
            };
            this.InternalChildren = [
                this.foreSprite,
            ];
        } else {
            // Use a cached framebuffer so the full-screen gaussian blur is rendered only once (or on resize).
            // The default (non-cached) BufferedContainer re-renders and re-applies the blur every single frame,
            // which is prohibitively expensive on mobile GPUs.
            this.backSprite = new BufferedContainer(cachedFrameBuffer: true) {
                Origin = Anchor.Centre,
                Anchor = Anchor.Centre,
                RelativeSizeAxes = Axes.Both,
                BlurSigma = new Vector2(10),
                Child = new Sprite {
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    Texture = this.texture,
                    FillMode = FillMode.Fill,
                },
            };
            this.foreSprite = new Sprite {
                Origin = Anchor.BottomCentre,
                Anchor = Anchor.BottomCentre,
                RelativeSizeAxes = Axes.Both,
                Texture = this.texture,
                FillMode = FillMode.Fit,
            };
            this.InternalChildren = [
                this.backSprite,
                this.foreSprite,
            ];
        }
    }

    protected override void Dispose(Boolean isDisposing) {
        if (isDisposing) {
            this.texture?.Dispose();
        }
        base.Dispose(isDisposing);
    }
}
