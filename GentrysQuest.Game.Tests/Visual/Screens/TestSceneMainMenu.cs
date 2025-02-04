using GentrysQuest.Game.Screens;
using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Framework.Screens;

namespace GentrysQuest.Game.Tests.Visual.Screens
{
    [TestFixture]
    public partial class TestSceneMainMenu : GentrysQuestTestScene
    {
        // Add visual tests to ensure correct behaviour of your game: https://github.com/ppy/osu-framework/wiki/Development-and-Testing
        // You can make changes to classes associated with the tests and they will recompile and update immediately.
        private MainMenuScreen mainMenu;

        public TestSceneMainMenu()
        {
            Add(new ScreenStack(mainMenu = new MainMenuScreen(true)) { RelativeSizeAxes = Axes.Both });
            AddStep("return", () => mainMenu.PressBack());
            AddStep("press play", () => mainMenu.PressPlay());
            AddStep("enter selection", () => mainMenu.EnterSelection());
        }
    }
}
