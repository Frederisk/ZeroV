using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;

using osuTK;
using osuTK.Graphics;

namespace ZeroV.Game.Elements.Particles;
internal partial class PressParticle: ParticleBase {
    private Container? startContainer;
    private Container? holdContainer;
    private Container? endContainer;

    private Container? container;

    public Single EndTime {
        get => this.EndTimeBindable.Value;
        set => this.EndTimeBindable.Value = value;
    }

    public Bindable<Single> EndTimeBindable;

    public PressParticle(Orbit fatherOrbit): base(fatherOrbit) {
        this.EndTimeBindable = new Bindable<Single>();
    }

    [BackgroundDependencyLoader]
    private void load() {
        this.startContainer = new Container {
            Children = new Drawable[] {
                new Diamond {
                    Size = new Vector2(52),
                    Colour= Colour4.Black,
                },
                new Diamond {
                    Size = new Vector2(28),
                    Colour= Colour4.Orange,
                },
            }
        };
        this.holdContainer = new Container {
            Children = new Drawable[] {
                new Box {
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    Width = MathF.Sqrt(2*52*52),
                    Colour = Color4.Black
                }
            }
        };
        this.endContainer = new Container {
            Children = new Drawable[] {
                new Diamond {
                    Size = new Vector2(52),
                    Colour= Colour4.Black,
                },
                new Diamond {
                    Size = new Vector2(28),
                    Colour= Colour4.Orange,
                },
            }
        };

        this.container = new Container {
            Children = new Drawable[] {
                this.holdContainer,
                this.startContainer,
                this.endContainer,
            }
        };

        this.InternalChild = this.container;
    }

}
