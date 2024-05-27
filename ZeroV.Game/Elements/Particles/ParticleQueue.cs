using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using osu.Framework.Graphics.Containers;

namespace ZeroV.Game.Elements.Particles;

public partial class ParticleQueue : Container<ParticleBase> {
    private readonly Queue<ParticleBase> innerQueue = [];

    /// <summary>
    /// Get a copy of the first particle in the queue.
    /// </summary>
    /// <returns>The first particle in the queue. If the queue is empty, return <see langword="null"/>.</returns>
    public ParticleBase? PeekOrDefault() => this.innerQueue.TryPeek(out ParticleBase? result) ? result : null;

    public Boolean TryPeek([MaybeNullWhen(false)] out ParticleBase result) => this.innerQueue.TryPeek(out result);

    public void DequeueInJudgeOnly() {
        ParticleBase particle = this.innerQueue.Dequeue();
        particle.OnDequeueInJudge();
    }

    public void Dequeue() {
        ParticleBase particle = this.innerQueue.Dequeue();
        this.RemoveInternal(particle, false);
    }

    public void Enqueue(ParticleBase particle) {
        this.innerQueue.Enqueue(particle);
        this.AddInternal(particle);
    }
}
