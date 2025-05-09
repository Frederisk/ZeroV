using osu.Framework.Graphics.Pooling;

using ZeroV.Game.Objects;

namespace ZeroV.Game.Graphics;

public abstract partial class ZeroVPoolableDrawable<TObject> : PoolableDrawable
    where TObject : TimeSource {
    public virtual TObject? Source { get; set; }
}
