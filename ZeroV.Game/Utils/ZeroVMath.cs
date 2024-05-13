using System;
using System.Collections.Generic;
using System.Numerics;

namespace ZeroV.Game.Utils;

/// <summary>
/// Stores magic numbers and mathematical methods commonly used in ZeroV.
/// </summary>
public static class ZeroVMath {
    public const Single SQRT_2 = 1.4142135623730950488016887242097f;
    public const Single DIAMOND_SIZE = 74;
    public const Single SCREEN_DRAWABLE_X = 1366;
    public const Single SCREEN_DRAWABLE_Y = 768;
    public const Single SCREEN_GAME_BASELINE_Y = 50;


    //public static T Sum<T>(this IEnumerable<T>  numberList) where T : struct, IAdditionOperators<T, T, T> {
    //    T t = default;
    //    foreach (T number in numberList) {
    //        t += number;
    //    }
    //    return t;
    //}
}
