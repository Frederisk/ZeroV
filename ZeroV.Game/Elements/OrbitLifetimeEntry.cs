using ZeroV.Game.Objects;

namespace ZeroV.Game.Elements;

public class OrbitLifetimeEntry : ZeroVObjectLifetimeEntry<Orbit>
{
    public OrbitLifetimeEntry(Orbit @object) : base(@object)
    {
        this.LifetimeStart = @object.StartTime;
        this.LifetimeEnd = @object.EndTime;
    }
}
