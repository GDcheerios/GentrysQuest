using GentrysQuest.Game.Screens.MainMenu;
using osu.Framework.Graphics;
using osu.Framework.Screens;
using NUnit.Framework;

namespace GentrysQuest.Game.Tests.Visual.Screens
{
    [TestFixture]
    public partial class TestSceneMainMenu : GentrysQuestTestScene
    {
        // Add visual tests to ensure correct behaviour of your game: https://github.com/ppy/osu-framework/wiki/Development-and-Testing
        // You can make changes to classes associated with the tests and they will recompile and update immediately.
        private MainMenu mainMenu;

        public TestSceneMainMenu()
        {
            Add(new ScreenStack(mainMenu = new MainMenu()) { RelativeSizeAxes = Axes.Both });
            AddStep("return", () => mainMenu.OnEntering(null));
            AddStep("press play", () => mainMenu.PressPlay());
        }
    }
}
