using osu.Framework;
using osu.Framework.Platform;
using System.Threading.Tasks;

namespace GentrysQuest.Desktop
{
    public static class Program
    {
        public static async Task Main()
        {
            using (GameHost host = Host.GetSuitableDesktopHost(@"Gentry's Quest"))
            using (osu.Framework.Game game = new GentrysQuestDesktop())
                host.Run(game);
        }
    }
}
