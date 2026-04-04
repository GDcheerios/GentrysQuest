using osu.Framework;
using osu.Framework.Platform;
using Velopack;

namespace GentrysQuest.Desktop
{
    public static class Program
    {
        public static void Main()
        {
            VelopackApp.Build().Run();

            using (GameHost host = Host.GetSuitableDesktopHost(@"Gentry's Quest"))
            using (osu.Framework.Game game = new GentrysQuestDesktop())
                host.Run(game);
        }
    }
}
