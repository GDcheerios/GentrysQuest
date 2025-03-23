using GentrysQuest.Game.Audio;
using GentrysQuest.Game.Graphics;
using GentrysQuest.Game.Graphics.TextStyles;
using GentrysQuest.Game.Overlays;
using GentrysQuest.Game.Overlays.Notifications;
using GentrysQuest.Game.Overlays.Profile;
using GentrysQuest.Game.Screens;
using GentrysQuest.Game.Users;
using GentrysQuest.Game.Utils;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Input.Events;
using osu.Framework.Screens;
using osuTK.Input;
using Character = SharpFNT.Character;

namespace GentrysQuest.Game
{
    public partial class GentrysQuestGame : GentrysQuestGameBase
    {
        // Important Variables
        /// <summary>
        /// The Game's current user
        /// </summary>
        [Cached]
        private readonly Bindable<IUser> user = new();
        // Cached so that it can be accessed by other classes
        // Bindable types let us listen for changes to the variable

        /// <summary>
        /// The current equipped character
        /// </summary>
        [Cached]
        private readonly Bindable<Character> equippedCharacter = new();

        // Screen stack
        private ScreenStack screenStack;
        private LoadingScreen loadingScreen;
        private IntroScreen introScreenScreen;
        private MainMenuScreen mainMenuScreen;
        private GameplayScreen gameplayScreenScreen;

        // Main buttons
        /// <summary>
        /// The profile button that will display
        /// all the main profile details and bring
        /// user to the profile
        /// </summary>
        private ProfileButton profileButton;

        // overlay stuff
        private TopRightContainer topRightContainer;
        private NotificationManager notificationManager = new NotificationManager();
        private AudioOverlay audioOverlay;
        private PlayerSelectContainer playerSelectContainer;

        private ScreenManager screenManager;

        [Cached]
        private TitleText titleText = new("Gentry's Quest") { Anchor = Anchor.TopCentre, Y = 300 };

        private GameMenuOverlay gameMenuOverlay;

        // Dependency management
        private DependencyContainer dependencies;

        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent) =>
            dependencies = new DependencyContainer(base.CreateChildDependencies(parent));

        protected override void LoadComplete()
        {
            base.LoadComplete();
            Child = screenStack = new ScreenStack(); // I need to see
            screenManager = new ScreenManager(screenStack);
            dependencies.CacheAs(screenManager);

            // overlays
            Add(titleText);
            titleText.Hide();

            // Cursor
            Add(new GqCursorContainer());

            audioOverlay = new AudioOverlay { Depth = -4 };
            AudioManager.Instance.OnPlayMusic += delegate { audioOverlay.DisplaySong(AudioManager.Instance.CurrentSong); };

            profileButton = new ProfileButton(user);
            Add(profileButton);
            profileButton.Hide();

            // Notifications and other corner stuff
            Notification.ProvideManager(notificationManager);
            Add(topRightContainer = new TopRightContainer());
            topRightContainer.AddOverlay(audioOverlay);
            topRightContainer.AddOverlay(notificationManager);

            dependencies.CacheAs(profileButton);

            // Game menu
            Add(gameMenuOverlay = new GameMenuOverlay());
            gameMenuOverlay.Disappear();
            dependencies.CacheAs(gameMenuOverlay);

            //  Set up the screens
            bool isBandits = MathBase.RandomBool(); // we got a bandits

            dependencies.CacheAs(mainMenuScreen = new MainMenuScreen(isBandits));
            introScreenScreen = new IntroScreen(mainMenuScreen, isBandits);
            loadingScreen = new LoadingScreen(introScreenScreen);
            gameplayScreenScreen = new GameplayScreen();

            screenManager.ProvideScreens(
                loadingScreen,
                introScreenScreen,
                mainMenuScreen,
                gameplayScreenScreen
            );

            screenStack.Push(new Tutorial());
            // screenManager.SetScreen(ScreenState.Loading);
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    gameMenuOverlay.Toggle();
                    break;
            }

            return base.OnKeyDown(e);
        }
    }
}
