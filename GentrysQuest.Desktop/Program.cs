using osu.Framework;
using osu.Framework.Platform;

namespace GentrysQuest.Desktop
{
    public static class Program
    {
        public static void Main()
        {
            using (GameHost host = Host.GetSuitableDesktopHost(@"Gentry's Quest"))
            using (osu.Framework.Game game = new GentrysQuestDesktop())
                host.Run(game);
        }
    }
}
