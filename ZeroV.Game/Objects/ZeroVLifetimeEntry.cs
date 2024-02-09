using osu.Framework.Graphics.Performance;

namespace ZeroV.Game.Objects;

public abstract class ZeroVLifetimeEntry<TSource, TDrawable>(TSource source) : LifetimeEntry
    where TSource : ZeroVObjectSource
    where TDrawable : ZeroVPoolableDrawable<TSource> {
    public TSource Source { get; } = source;
    public TDrawable? Drawable { get; set; }
}
