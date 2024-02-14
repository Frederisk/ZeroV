using osu.Framework.Graphics.Performance;

using ZeroV.Game.Objects;

namespace ZeroV.Game.Graphics;

public abstract class ZeroVLifetimeEntry<TSource, TDrawable>(TSource source) : LifetimeEntry
    where TSource : TimeSource
    where TDrawable : ZeroVPoolableDrawable<TSource> {
    public TSource Source { get; } = source;
    public TDrawable? Drawable { get; set; }
}
