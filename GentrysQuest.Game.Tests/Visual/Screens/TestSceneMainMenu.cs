using GentrysQuest.Game.Overlays;
using GentrysQuest.Game.Overlays.Profile;
using GentrysQuest.Game.Screens;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Screens;

namespace GentrysQuest.Game.Tests.Visual.Screens
{
    [TestFixture]
    public partial class TestSceneMainMenu : GentrysQuestTestScene
    {
        private MainMenuScreen mainMenu;

        [Cached]
        private GameMenuOverlay gameMenuOverlay { get; set; } = new();

        [Cached]
        private ProfileButton profileButton { get; set; } = new();

        [Cached]
        private ScreenManager screenManager { get; set; } = new ScreenManager(new ScreenStack());

        public TestSceneMainMenu()
        {
            Add(new ScreenStack(mainMenu = new MainMenuScreen(true)) { RelativeSizeAxes = Axes.Both });
            AddStep("return", () => mainMenu.PressBack());
            AddStep("press play", () => mainMenu.PressPlay());
            AddStep("enter selection", () => mainMenu.EnterSelection());
        }
    }
}
