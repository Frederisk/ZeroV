using System;

using osu.Framework.iOS;

using ZeroV.Game;

namespace ZeroV.iOS;

public static class Application {
    public static void Main(String[] args) => GameApplication.Main(new ZeroVGame());
}
