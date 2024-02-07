using ZeroV.Game.Objects;

namespace ZeroV.Game.Elements;

public class OrbitLifetimeEntry : ZeroVObjectLifetimeEntry<OrbitSource, Orbit>
{
    public OrbitLifetimeEntry(OrbitSource orbit) : base(orbit)
    {
        this.LifetimeStart = orbit.StartTime;
        this.LifetimeEnd = orbit.EndTime;
    }
}
