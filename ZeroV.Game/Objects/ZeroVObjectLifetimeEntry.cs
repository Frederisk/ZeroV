using osu.Framework.Graphics.Performance;
using osu.Framework.Graphics.Pooling;

namespace ZeroV.Game.Objects;

public abstract class ZeroVObjectLifetimeEntry<TObject>(TObject @object) : LifetimeEntry
    where TObject: ZeroVObject {
    public TObject Object { get; } = @object;
    public PoolableDrawable? Drawable { get; set; }

}
