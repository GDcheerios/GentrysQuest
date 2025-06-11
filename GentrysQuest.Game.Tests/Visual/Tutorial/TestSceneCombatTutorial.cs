using GentrysQuest.Game.Screens;
using NUnit.Framework;
using osu.Framework.Screens;

namespace GentrysQuest.Game.Tests.Visual.Tutorial
{
    [TestFixture]
    public partial class TestSceneCombatTutorial : GentrysQuestTestScene
    {
        private CombatTutorial combatTutorial;
        private ScreenStack screenStack;

        public TestSceneCombatTutorial()
        {
            AddStep("[SETUP]", () =>
            {
                Add(screenStack = new ScreenStack());
                screenStack.Push(combatTutorial = new CombatTutorial());
            });
            AddStep("Teleport enemy", () => combatTutorial.TeleportEnemy());
        }
    }
}
