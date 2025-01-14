using GentrysQuest.Game.Audio;
using GentrysQuest.Game.Overlays;
using GentrysQuest.Game.Overlays.Notifications;
using GentrysQuest.Game.Screens.LoadingScreen;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Screens;

namespace GentrysQuest.Game
{
    public partial class GentrysQuestGame(bool arcadeMode) : GentrysQuestGameBase
    {
        public static ScreenStack ScreenStack = new() { RelativeSizeAxes = Axes.Both };

        [BackgroundDependencyLoader]
        private void load()
        {
            // Add your top-level game components here.
            // A screen stack and sample screen has been provided for convenience, but you can replace it if you don't want to use screens.
            Child = ScreenStack;
            Add(new CursorContainer());
            Add(NotificationContainer.Instance);
            Add(AudioManager.Instance);
            AudioOverlay audioOverlay;
            Add(audioOverlay = new AudioOverlay { Depth = -4 });
            AudioManager.Instance.OnPlayMusic += delegate { audioOverlay.DisplaySong(AudioManager.Instance.CurrentSong); };
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            ScreenStack.Push(new LoadingScreen());
        }
    }
}
