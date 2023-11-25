using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using osu.Framework.Allocation;

namespace ZeroV.Game;
public partial class ZeroVGameSettings {

    /// <summary>
    /// The time before the judgment for the Particle to fall. The shorter the time, the faster the Particle falls.
    /// </summary>
    public Double StartTimeOffset { get; } = TimeSpan.FromSeconds(0.5).TotalMilliseconds;
    public Double MaxPerfectOffset { get; } = TimeSpan.FromSeconds(0.03).TotalMilliseconds;
    public Double PerfectOffset { get; } = TimeSpan.FromSeconds(0.2).TotalMilliseconds;
    public Double GoodOffset { get; } = TimeSpan.FromSeconds(0.5).TotalMilliseconds;

}
