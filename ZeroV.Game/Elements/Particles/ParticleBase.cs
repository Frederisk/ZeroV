using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using osu.Framework.Bindables;
using osu.Framework.Graphics.Containers;

namespace ZeroV.Game.Elements.Particles;


internal abstract partial class ParticleBase: CompositeDrawable {

    public virtual Single StartTime {
        get => this.StartTimeBindable.Value;
        init => this.StartTimeBindable.Value = value;
    }

    public Bindable<Single> StartTimeBindable;

    public Orbit FatherOrbit { get; init; }

    public ParticleBase(Orbit fatherOrbit) {
        this.StartTimeBindable = new Bindable<Single>();
        this.FatherOrbit = fatherOrbit;
    }

    public Boolean IsRecycable { get; private set; }

    public virtual void Recycle(Orbit fatherOrbit, Single startTime) {
        // TODO: Create appropriate methods to make objects reusable.
        this.StartTimeBindable = new Bindable<Single> {
            Value = startTime,
        };
    }
}
