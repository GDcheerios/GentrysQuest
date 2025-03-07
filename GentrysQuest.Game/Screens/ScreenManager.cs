using System;
using osu.Framework.Allocation;
using osu.Framework.Screens;

namespace GentrysQuest.Game.Screens
{
    public partial class ScreenManager : IDependencyInjectionCandidate
    {
        private ScreenStack stack;
        private LoadingScreen loadingScreen;
        private IntroScreen introScreenScreen;
        private MainMenuScreen mainMenuScreen;
        private GameplayScreen gameplayScreen;

        public ScreenManager(ScreenStack stack) => this.stack = stack;

        public void ProvideScreens(
            LoadingScreen loadingScreen,
            IntroScreen introScreenScreen,
            MainMenuScreen mainMenuScreen,
            GameplayScreen gameplayScreen
        )
        {
            this.loadingScreen = loadingScreen;
            this.introScreenScreen = introScreenScreen;
            this.mainMenuScreen = mainMenuScreen;
            this.gameplayScreen = gameplayScreen;
        }

        public void SetScreen(ScreenState state)
        {
            switch (state)
            {
                case ScreenState.Loading:
                    stack.Push(loadingScreen);
                    break;

                case ScreenState.Intro:
                    stack.Push(introScreenScreen);
                    break;

                case ScreenState.MainMenu:
                    stack.Push(mainMenuScreen);
                    break;

                case ScreenState.Gameplay:
                    stack.Push(gameplayScreen);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        public GqScreen CurrentScreen => stack.CurrentScreen as GqScreen;

        public GqScreen GetScreen(ScreenState state)
        {
            switch (state)
            {
                case ScreenState.Loading:
                    return loadingScreen;

                case ScreenState.Intro:
                    return introScreenScreen;

                case ScreenState.MainMenu:
                    return mainMenuScreen;

                case ScreenState.Gameplay:
                    return gameplayScreen;

                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }
    }
}
