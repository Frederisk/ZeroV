using ZeroV.Game.Objects;

namespace ZeroV.Game.Elements;

public class OrbitLifetimeEntry : ZeroVObjectLifetimeEntry<Orbit, OrbitDrawable>
{
    public OrbitLifetimeEntry(Orbit orbit) : base(orbit)
    {
        this.LifetimeStart = orbit.StartTime;
        this.LifetimeEnd = orbit.EndTime;
    }
}
