using osu.Framework.Graphics.Pooling;

namespace ZeroV.Game.Objects;
public abstract partial class ZeroVDrawableObject<TObject> : PoolableDrawable
    where TObject : ZeroVObject {

    public virtual TObject? Object { get; set; }
}
