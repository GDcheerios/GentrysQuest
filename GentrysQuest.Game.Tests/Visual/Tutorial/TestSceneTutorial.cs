using GentrysQuest.Game.Overlays;
using GentrysQuest.Game.Overlays.Profile;
using GentrysQuest.Game.Screens;
using GentrysQuest.Game.Users;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Screens;

namespace GentrysQuest.Game.Tests.Visual.Tutorial
{
    [TestFixture]
    public partial class TestSceneTutorial : GentrysQuestTestScene
    {
        private Game.Screens.Tutorial tutorial;
        private ScreenStack screenStack;
        protected override string TestName { get; init; } = "Tutorial";

        [Cached]
        private ScreenManager screenManager { get; set; } = new ScreenManager(null);

        [Cached]
        private ProfileButton profileButton { get; set; }

        [Cached]
        private GameMenuOverlay gameMenuOverlay { get; set; }

        [Cached]
        private Bindable<IUser> user { get; set; }

        public TestSceneTutorial()
        {
            user = new Bindable<IUser>();
            profileButton = new ProfileButton();
            Add(gameMenuOverlay = new GameMenuOverlay());
            gameMenuOverlay.Disappear();
            profileButton.Hide();

            AddStep("[SETUP]", () =>
            {
                Add(screenStack = new ScreenStack());
                screenStack.Push(tutorial = new Game.Screens.Tutorial());
            });
        }
    }
}
