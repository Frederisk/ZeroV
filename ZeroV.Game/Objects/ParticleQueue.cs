using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using osu.Framework.Graphics.Containers;

using ZeroV.Game.Elements.Particles;

namespace ZeroV.Game.Objects;
public partial class ParticleQueue : Container<ParticleBase> {

    private readonly Queue<ParticleBase> innerQueue = [];

    public ParticleBase? PeekOrDefault() => this.innerQueue.TryPeek(out ParticleBase? result) ? result : null;
    public Boolean TryPeek([MaybeNullWhen(false)] out ParticleBase result) => this.innerQueue.TryPeek(out result);

    public void Dequeue() {
        ParticleBase particle = this.innerQueue.Dequeue();
        this.RemoveInternal(particle, false);
    }

    public void Enqueue(ParticleBase particle) {
        this.innerQueue.Enqueue(particle);
        this.AddInternal(particle);
    }
}
