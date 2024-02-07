using osu.Framework.Graphics.Performance;
using osu.Framework.Graphics.Pooling;

namespace ZeroV.Game.Objects;

public abstract class ZeroVObjectLifetimeEntry<TObject, TDrawable>(TObject @object) : LifetimeEntry
    where TObject: ZeroVObject
    where TDrawable : ZeroVDrawableObject<TObject> {
    public TObject Object { get; } = @object;
    public TDrawable? Drawable { get; set; }
}
