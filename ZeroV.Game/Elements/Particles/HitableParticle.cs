using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using osu.Framework.Bindables;
using osu.Framework.Graphics.Containers;

namespace ZeroV.Game.Elements.Particles;

internal abstract partial class HitableParticle: CompositeDrawable {

    public virtual Double StartTime {
        get => this.StartTimeBindable.Value;
        init => this.StartTimeBindable.Value = value;
    }

    public readonly Bindable<Double> StartTimeBindable;

    public Orbit FatherOrbit { get; init; }

    public HitableParticle(Orbit fatherOrbit) {
        this.StartTimeBindable = new Bindable<Double>();
        this.FatherOrbit = fatherOrbit;
    }
}
