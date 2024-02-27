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

    public virtual Single StartTime {
        get => this.StartTimeBindable.Value;
        init => this.StartTimeBindable.Value = value;
    }

    public Bindable<Single> StartTimeBindable;

    [Resolved]
    public Orbit FatherOrbit { get; private set; }

    public ParticleBase() {
        this.Origin = Anchor.Centre;
        this.Anchor = Anchor.Centre;
        this.StartTimeBindable = new Bindable<Single>();
    }

    public Boolean IsRecyclable { get; private set; }

    public virtual void Recycle(Orbit fatherOrbit, Single startTime) {
        // TODO: Create appropriate methods to make objects reusable.
        this.StartTimeBindable = new Bindable<Single> {
            Value = startTime,
        };
    }
}
