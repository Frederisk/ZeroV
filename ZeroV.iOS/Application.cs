using osu.Framework.iOS;
using ZeroV.Game;

namespace ZeroV.iOS
{
    public static class Application
    {
        public static void Main(string[] args) => GameApplication.Main(new ZeroVGame());
    }
}
