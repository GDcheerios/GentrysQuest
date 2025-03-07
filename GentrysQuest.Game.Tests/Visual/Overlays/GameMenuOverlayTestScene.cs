using GentrysQuest.Game.Overlays;
using GentrysQuest.Game.Overlays.Profile;
using GentrysQuest.Game.Screens;
using GentrysQuest.Game.Users;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Screens;

namespace GentrysQuest.Game.Tests.Visual.Overlays
{
    public partial class GameMenuOverlayTestScene : GentrysQuestTestScene
    {
        private GameMenuOverlay gameMenuOverlay;

        [Cached]
        private Bindable<IUser> guestUser = new Bindable<IUser>();

        [Cached]
        private ProfileButton profileButton = new ProfileButton(new Bindable<IUser>());

        [Cached]
        private ScreenManager screenManager = new ScreenManager(new ScreenStack());

        public GameMenuOverlayTestScene()
        {
            Add(new Box
            {
                RelativeSizeAxes = Axes.Both,
                Colour = Colour4.White
            });
            Add(gameMenuOverlay = new GameMenuOverlay());
            gameMenuOverlay.Disappear();
        }

        [Test]
        public void Test()
        {
            AddStep("Appear", () => gameMenuOverlay.Appear());
            AddStep("Disappear", () => gameMenuOverlay.Disappear());
        }

    }
}
