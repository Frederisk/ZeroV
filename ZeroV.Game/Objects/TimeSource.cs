using System;

namespace ZeroV.Game.Objects;

/// <summary>
/// An <see langword="object"/> with a life cycle time,
/// which can indicate the start and end of its life time and duration.
/// </summary>
public abstract class TimeSource {

    /// <summary>
    /// The start time of this <see cref="TimeSource"/>.
    /// </summary>
    public abstract Double StartTime { get; }

    /// <summary>
    /// The end time of this <see cref="TimeSource"/>.
    /// </summary>
    public abstract Double EndTime { get; }

    /// <summary>
    /// The duration of this <see cref="TimeSource"/>.
    /// </summary>
    public Double Duration => this.EndTime - this.StartTime;
}
