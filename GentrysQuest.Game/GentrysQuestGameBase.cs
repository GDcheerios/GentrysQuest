using GentrysQuest.Game.Audio;
using GentrysQuest.Game.Database;
using GentrysQuest.Game.Online;
using GentrysQuest.Game.Utils;
using GentrysQuest.Resources;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.IO.Stores;
using osu.Framework.Logging;
using osuTK;

namespace GentrysQuest.Game
{
    public partial class GentrysQuestGameBase : osu.Framework.Game
    {
        // Anything in this class is shared between the test browser and the game implementation.
        // It allows for caching global dependencies that should be accessible to tests, or changing
        // the screen scaling for all components including the test browser and framework overlays.
        protected override Container<Drawable> Content { get; }
        private GameClock gameClock;

        [Cached]
        private DiscordRpc discordRpc = new("1115885237910634587");

        protected GentrysQuestGameBase()
        {
            base.Content.Add(Content = new DrawSizePreservingFillContainer
            {
                TargetDrawSize = new Vector2(1920, 1080)
            });
            DatabaseManager.CheckDatabase();
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Logger.Log("Loading GentrysQuest base resources");
            Resources.AddStore(new DllResourceStore(typeof(GentrysQuestResources).Assembly));
            _ = new GameClock(Clock);
            Add(AudioManager.Instance);
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            Logger.Log("Loading GentrysQuest base resources complete");
        }
    }
}
