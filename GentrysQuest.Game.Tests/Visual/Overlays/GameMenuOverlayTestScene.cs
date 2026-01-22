using GentrysQuest.Game.Overlays;
using GentrysQuest.Game.Overlays.Profile;
using GentrysQuest.Game.Screens;
using GentrysQuest.Game.Users;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Screens;

namespace GentrysQuest.Game.Tests.Visual.Overlays
{
    public partial class GameMenuOverlayTestScene : GentrysQuestTestScene
    {
        private GameMenuOverlay gameMenuOverlay;

        [Resolved]
        private Bindable<IUser> user { get; set; }

        [Cached]
        private ProfileButton profileButton;

        [Cached]
        private ScreenManager screenManager = new ScreenManager(new ScreenStack());

        public GameMenuOverlayTestScene()
        {
            user.Value = GuestUser.Create("testy");
            profileButton = new ProfileButton();
            Add(gameMenuOverlay = new GameMenuOverlay { Y = -100 });
        }

        [Test]
        public void Test()
        {
            AddStep("Create User", () => gameMenuOverlay.Show());
            AddStep("Appear", () => gameMenuOverlay.Appear());
            AddStep("Disappear", () => gameMenuOverlay.Disappear());
        }
    }
}
