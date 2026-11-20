using osu.Framework;
using osu.Framework.Platform;

namespace GentrysQuest.Editor
{
    public static class Program
    {
        public static void Main()
        {
            using GameHost host = Host.GetSuitableDesktopHost("Gentry's Quest Map Editor");
            using var game = new Editor();
            host.Run(game);
        }
    }
}
