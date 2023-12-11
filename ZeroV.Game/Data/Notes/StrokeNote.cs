namespace ZeroV.Game.Data.Particles;

public class StrokeNote : NoteBase {
    public required StrokeType Type { get; init; }
    public enum StrokeType {
        Left,
        Right
    }
}
