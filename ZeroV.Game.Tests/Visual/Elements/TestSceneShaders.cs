

using System;
using System.Runtime.InteropServices;

using NUnit.Framework;

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Rendering;
using osu.Framework.Graphics.Shaders;
using osu.Framework.Graphics.Shaders.Types;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;

using osuTK;

using ZeroV.Game.Utils;

namespace ZeroV.Game.Tests.Visual.Elements;

[TestFixture]
public partial class TestSceneShaders : ZeroVTestScene {

    private RainbowBox box = new() {
        Size = new Vector2(256),
        X = 128,
        Origin = Anchor.Centre,
        Anchor = Anchor.Centre,
        HsvaColour = new Vector4(-1, 0.5f, 1, 0.7f),
        SizeRatio = 1,
        BorderRatio = 0.04f,
    };

    public TestSceneShaders() {
        //this.Add(new Box {
        //    RelativeSizeAxes = Axes.Both,
        //    Colour = Colour4.Red,
        //});
        this.Add(this.box);
        this.Add(new TapLight {
            Size = new Vector2(128, 512),
            X = -128,
            Origin = Anchor.Centre,
            Anchor = Anchor.Centre,
        });
    }

    protected override void Update() {
        base.Update();
        var time = (this.Time.Current / 1000 / 3) % 1;
        this.box.SizeRatio = (Single)time;
    }

    public partial class TapLight : Box, ITexturedShaderDrawable {
        [BackgroundDependencyLoader]
        private void load(ShaderManager shaders) {
            this.TextureShader = shaders.Load(VertexShaderDescriptor.TEXTURE_2, "OrbitSelfLuminous");
        }
    }

    public partial class RainbowBox : Sprite {
        private Single sizeRatio;

        public Single SizeRatio {
            get => this.sizeRatio;
            set {
                if (this.sizeRatio == value) {
                    return;
                }

                this.sizeRatio = value;
                this.Invalidate(Invalidation.DrawNode);
            }
        }

        private Single borderRatio;

        public Single BorderRatio {
            get => this.borderRatio;
            set {
                if (this.borderRatio == value) {
                    return;
                }

                this.borderRatio = value;
                this.Invalidate(Invalidation.DrawNode);
            }
        }

        private Vector4 hsvaColour;

        public Vector4 HsvaColour {
            get => this.hsvaColour;
            set {
                for (Int32 i = 0; i <= 3; i++) {
                    if (!Single.IsFinite(value[i])) {
                        throw new ArgumentException($"{nameof(this.HsvaColour)} must be finite, but is {value[i]}.");
                    }
                }

                this.hsvaColour = value;

                this.Invalidate(Invalidation.DrawNode);
            }
        }

        [BackgroundDependencyLoader]
        private void load(ShaderManager shaders, IRenderer renderer) {
            this.Texture ??= renderer.WhitePixel;
            this.TextureShader = shaders.Load(VertexShaderDescriptor.TEXTURE_2, "SpinRainbow");
        }

        protected override DrawNode CreateDrawNode() => new CircularProgressDrawNode(this);

        protected class CircularProgressDrawNode : SpriteDrawNode {
            public new RainbowBox Source => (RainbowBox)base.Source;

            public CircularProgressDrawNode(RainbowBox source)
                : base(source) {
            }

            protected Single SizeRatio { get; private set; }
            protected Single BorderRatio { get; private set; }
            protected Vector4 HsvaColour { get; private set; }

            public override void ApplyState() {
                base.ApplyState();

                this.SizeRatio = this.Source.SizeRatio;
                this.BorderRatio = this.Source.BorderRatio;
                this.HsvaColour = this.Source.HsvaColour;
            }

            private IUniformBuffer<CircularProgressParameters>? parametersBuffer;

            protected override void Blit(IRenderer renderer) {
                //if (InnerRadius == 0 || (!RoundedCaps && Progress == 0))
                //    return;

                base.Blit(renderer);
            }

            protected override void BindUniformResources(IShader shader, IRenderer renderer) {
                base.BindUniformResources(shader, renderer);

                this.parametersBuffer ??= renderer.CreateUniformBuffer<CircularProgressParameters>();
                this.parametersBuffer.Data = new CircularProgressParameters {
                    HsvaColour = this.HsvaColour,
                    SizeRatio = (this.SizeRatio - this.BorderRatio * ZeroVMath.SQRT_2) / 2,
                    BorderRatio = this.BorderRatio / 2,
                };

                shader.BindUniformBlock("m_SpinRanbowFrameParameters", this.parametersBuffer);
            }

            protected override Boolean CanDrawOpaqueInterior => false;

            protected override void Dispose(Boolean isDisposing) {
                base.Dispose(isDisposing);
                this.parametersBuffer?.Dispose();
            }

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            private record struct CircularProgressParameters {
                public UniformVector4 HsvaColour;
                public UniformFloat SizeRatio;
                public UniformFloat BorderRatio;
                public UniformPadding8 Padding;
            }
        }
    }
}
