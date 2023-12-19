using System;

using osu.Framework.Bindables;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Objects.Types;
using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.ZeroV.Scoring;

namespace osu.Game.Rulesets.ZeroV.Objects;

public abstract class ZeroVHitObject : HitObject, IHasColumn {

    /// <summary>
    /// Range = [-1,1]
    /// </summary>
    public Int32 Lane;

    private HitObjectProperty<Int32> column;
    public Bindable<Int32> ColumnBindable => this.column.Bindable;
    public virtual Int32 Column {
        get => this.column.Value;
        set => this.column.Value = value;
    }

    protected override HitWindows CreateHitWindows() => new ZeroVHitWindows();
}
