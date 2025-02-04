using GentrysQuest.Game.Audio;
using GentrysQuest.Game.Overlays;
using GentrysQuest.Game.Overlays.Notifications;
using GentrysQuest.Game.Screens;
using GentrysQuest.Game.Screens.Gameplay;
using GentrysQuest.Game.Screens.LoadingScreen;
using GentrysQuest.Game.Utils;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Screens;

namespace GentrysQuest.Game
{
    public partial class GentrysQuestGame(bool arcadeMode) : GentrysQuestGameBase
    {
        // Screen stack
        private ScreenStack screenStack;
        private LoadingScreen loadingScreen;
        private Intro introScreen;
        private MainMenuScreen mainMenuScreen;
        private Gameplay gameplayScreen;

        protected override void LoadComplete()
        {
            base.LoadComplete();

            //  Set up the screen stack
            Child = screenStack = new ScreenStack();

            bool isBandits = MathBase.RandomBool(); // we got a bandits

            mainMenuScreen = new MainMenuScreen(isBandits);
            introScreen = new Intro(mainMenuScreen, isBandits);
            loadingScreen = new LoadingScreen(introScreen);
            gameplayScreen = new Gameplay();

            screenStack.Push(loadingScreen);

            Add(new CursorContainer());

            Add(NotificationContainer.Instance);

            AudioOverlay audioOverlay;
            Add(audioOverlay = new AudioOverlay { Depth = -4 });
            AudioManager.Instance.OnPlayMusic += delegate { audioOverlay.DisplaySong(AudioManager.Instance.CurrentSong); };
        }
    }
}
