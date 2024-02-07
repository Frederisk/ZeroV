using osu.Framework.Graphics.Performance;

namespace ZeroV.Game.Objects;

public abstract class ZeroVObjectLifetimeEntry<TObject, TDrawable>(TObject obj) : LifetimeEntry
    where TObject: ZeroVObject
    where TDrawable : ZeroVDrawableObject<TObject> {
    public TObject Object { get; } = obj;
    public TDrawable? Drawable { get; set; }
}
