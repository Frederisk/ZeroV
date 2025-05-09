using ZeroV.Game.Graphics;

namespace ZeroV.Game.Elements.Orbits;

public class OrbitLifetimeEntry : ZeroVLifetimeEntry<OrbitSource, Orbit> {

    public OrbitLifetimeEntry(OrbitSource orbit) : base(orbit) {
        this.LifetimeStart = orbit.StartTime;
        this.LifetimeEnd = orbit.EndTime;
    }
}
