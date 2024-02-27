using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;

using ZeroV.Game.Graphics;
using ZeroV.Game.Objects;

namespace ZeroV.Game.Elements.Particles;

/// <summary>
/// The base class for all particles.
/// </summary>
public abstract partial class ParticleBase<TObject> : ZeroVPoolableDrawable<TObject>
    where TObject: TimeSource {

    public virtual Double StartTime {
        get => this.StartTimeBindable.Value;
        init => this.StartTimeBindable.Value = value;
    }

    public Bindable<Double> StartTimeBindable;

    public Orbit FatherOrbit { get; private set; } = null!;

    public ParticleBase() {
        this.Origin = Anchor.Centre;
        this.Anchor = Anchor.Centre;
        this.StartTimeBindable = new BindableDouble();
    }

    public Boolean IsRecyclable { get; private set; }

    public virtual void Recycle(Orbit fatherOrbit, Double startTime) {
        this.FatherOrbit = fatherOrbit;
        // TODO: Create appropriate methods to make objects reusable.
        this.StartTimeBindable = new Bindable<Double> {
            Value = startTime,
        };
    }
}
