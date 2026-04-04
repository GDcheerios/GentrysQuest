using GentrysQuest.Game.Content.Gachas;
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
        private ProfileButton profileButton = new();

        [Cached]
        private ScreenManager screenManager = new ScreenManager(new ScreenStack());

        [BackgroundDependencyLoader]
        private void load()
        {
            profileButton = new ProfileButton();
            Add(gameMenuOverlay = new GameMenuOverlay { Y = -100 });
            gameMenuOverlay.GachaContainer.LoadGacha(new GameGacha());
            gameMenuOverlay.Disappear();
        }

        [Test]
        public void Test()
        {
            AddStep("Create User", () =>
            {
                user.Value = new GuestUser("testy");
                user.Value.MoneyHandler.InfiniteMoney = true;
            });
            AddStep("Appear", () => gameMenuOverlay.Appear());
            AddStep("Disappear", () => gameMenuOverlay.Disappear());
        }
    }
}
