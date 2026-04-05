using GentrysQuest.Game.Audio;
using GentrysQuest.Game.Database;
using GentrysQuest.Game.Input;
using GentrysQuest.Game.Online;
using GentrysQuest.Game.Online.API;
using GentrysQuest.Game.Overlays.Notifications;
using GentrysQuest.Game.Users;
using GentrysQuest.Game.Utils;
using GentrysQuest.Resources;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.IO.Stores;
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

        [Cached]
        private InputHandler inputHandler = new();

        [Cached]
        private GqWebSocketClient websocketClient = new();

        /// <summary>
        /// The Game's current user
        /// </summary>
        [Cached]
        protected Bindable<IUser> user { get; } = new();
        // Cached so that it can be accessed by other classes
        // Bindable types let us listen for changes to the variable

        protected GentrysQuestGameBase()
        {
            base.Content.Add(Content = new DrawSizePreservingFillContainer
            {
                TargetDrawSize = new Vector2(1000, 1000)
            });
            base.Content.Add(inputHandler);
            DatabaseManager.CheckDatabase();
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Resources.AddStore(new DllResourceStore(typeof(GentrysQuestResources).Assembly));
            _ = new GameClock(Clock);
            Add(AudioManager.Instance);
            APIAccess.SessionExpired += handleSessionExpired;
        }

        private void handleSessionExpired()
        {
            Scheduler.Add(() =>
            {
                if (user.Value == null)
                    return;

                _ = websocketClient.DisconnectAsync();
                user.Value = null;
                Notification.Create("Session expired. Please log in again.", NotificationType.Error);
            });
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                APIAccess.SessionExpired -= handleSessionExpired;
                websocketClient.Dispose();
                discordRpc.Dispose();
            }

            base.Dispose(isDisposing);
        }
    }
}
