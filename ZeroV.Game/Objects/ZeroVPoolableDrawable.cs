using osu.Framework.Graphics.Pooling;

namespace ZeroV.Game.Objects;

public abstract partial class ZeroVPoolableDrawable<TObject> : PoolableDrawable
    where TObject : ZeroVObjectSource {
    public virtual TObject? Source { get; set; }
}
