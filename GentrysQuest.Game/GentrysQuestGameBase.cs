using GentrysQuest.Game.Database;
using GentrysQuest.Game.Overlays;
using GentrysQuest.Game.Overlays.Notifications;
using GentrysQuest.Resources;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.IO.Stores;
using osuTK;
using AudioManager = GentrysQuest.Game.Audio.AudioManager;

namespace GentrysQuest.Game
{
    public partial class GentrysQuestGameBase : osu.Framework.Game
    {
        // Anything in this class is shared between the test browser and the game implementation.
        // It allows for caching global dependencies that should be accessible to tests, or changing
        // the screen scaling for all components including the test browser and framework overlays.
        protected override Container<Drawable> Content { get; }
        private AudioOverlay audioOverlay;

        protected GentrysQuestGameBase()
        {
            // Ensure game and tests scale with window size and screen DPI.
            base.Content.Add(Content = new DrawSizePreservingFillContainer
            {
                // You may want to change TargetDrawSize to your "default" resolution, which will decide how things scale and position when using absolute coordinates.
                TargetDrawSize = new Vector2(1000, 1000)
            });
            DatabaseManager.CheckDatabase();
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Resources.AddStore(new DllResourceStore(typeof(GentrysQuestResources).Assembly));
            base.Content.Add(NotificationContainer.Instance);
            base.Content.Add(AudioManager.Instance);
            base.Content.Add(audioOverlay = new AudioOverlay { Depth = -4 });
            AudioManager.Instance.OnPlayMusic += delegate { audioOverlay.DisplaySong(AudioManager.Instance.CurrentSong); };
        }
    }
}
