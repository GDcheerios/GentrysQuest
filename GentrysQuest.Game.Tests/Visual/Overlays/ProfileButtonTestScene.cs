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
    [TestFixture]
    public partial class ProfileButtonTestScene : GentrysQuestTestScene
    {
        private ProfileButton profileButton;

        [Resolved]
        private Bindable<IUser> user { get; }

        [Cached]
        private ScreenManager screenManager = new ScreenManager(new ScreenStack());

        private GuestUser user1;
        private GuestUser user2;

        public ProfileButtonTestScene()
        {
            user1 = GuestUser.Create("testy1");
            user2 = GuestUser.Create("testy2");

            Add(new Box
            {
                RelativeSizeAxes = Axes.Both,
                Colour = Colour4.DarkGray
            });
            Add(profileButton = new ProfileButton());
            profileButton.Hide();
        }

        [Test]
        public void Test()
        {
            AddStep("Show", () => profileButton.Show());
            AddStep("Hide", () => profileButton.Hide());
            AddStep("Inform", () => profileButton.Inform());
            AddStep("Set user1", () => user.Value = user1);
            AddStep("Set user2", () => user.Value = user2);
        }
    }
}
