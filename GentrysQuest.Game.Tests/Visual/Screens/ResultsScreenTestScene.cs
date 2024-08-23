using GentrysQuest.Game.Screens.Gameplay;
using NUnit.Framework;
using osu.Framework.Screens;

namespace GentrysQuest.Game.Tests.Visual.Screens
{
    [TestFixture]
    public partial class ResultsScreenTestScene : GentrysQuestTestScene
    {
        private ScreenStack screens;
        private ResultScreen resultScreen;

        public ResultsScreenTestScene()
        {
            Add(screens = new ScreenStack());
            screens.Push(resultScreen = new ResultScreen(1));
        }
    }
}
