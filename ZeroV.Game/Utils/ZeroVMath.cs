using System;

namespace ZeroV.Game.Utils;

/// <summary>
/// Stores magic numbers and mathematical methods commonly used in ZeroV.
/// </summary>
public static class ZeroVMath {
    public const Single SQRT_2 = 1.4142135623730950488016887242097f;
    public const Single SQRT_3 = 1.7320508075688772935274463415059f;
    public const Single SQRT_5 = 2.2360679774997896964091736687313f;
    public const Single SQRT_7 = 2.6457513110645905905016157536393f;
    public const Single DIAMOND_SIZE = 74;
    public const Single SCREEN_DRAWABLE_X = 1366;
    public const Single SCREEN_DRAWABLE_Y = 768;
    public const Single SCREEN_GAME_BASELINE_Y = 50;
    public const Double JUDGE_TIME_MILLISECONDS_NONE_OR_MISS = 250;
    public const Double JUDGE_TIME_MILLISECONDS_NORMAL = 75;
    public const Double JUDGE_TIME_MILLISECONDS_PERFECT = 30;
    //public const Double JUDGE_TIME_MILLISECONDS_MAX_PERFECT = 0;
    public const Double PARTICLE_FADING_TIME = 300;
    public const Int32 DRAWABLE_POOL_INITIAL_SIZE_ORBIT = 10;
    public const Int32 DRAWABLE_POLL_INITIAL_SIZE_PARTICLE = 20;
    public static readonly Int32? DRAWABLE_POOL_MAX_SIZE_ORBIT = 15;
    public static readonly Int32? DRAWABLE_POOL_MAX_SIZE_PARTICLE = 25;
}
