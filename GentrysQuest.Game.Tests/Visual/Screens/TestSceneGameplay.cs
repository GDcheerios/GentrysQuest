using GentrysQuest.Game.Content.Characters;
using GentrysQuest.Game.Content.Maps;
using GentrysQuest.Game.Content.Weapons;
using GentrysQuest.Game.Entity;
using GentrysQuest.Game.Entity.Weapon;
using GentrysQuest.Game.Overlays;
using GentrysQuest.Game.Overlays.Profile;
using GentrysQuest.Game.Screens;
using GentrysQuest.Game.Users;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Screens;

namespace GentrysQuest.Game.Tests.Visual.Screens
{
    [TestFixture]
    public partial class TestSceneGameplay : GentrysQuestTestScene
    {
        private ScreenStack screens;
        private GameplayScreen gameplayScreen;
        private Character player;
        private Weapon testWeapon;

        [Resolved]
        private Bindable<IUser> user { get; set; }

        [Cached]
        private GameMenuOverlay gameMenuOverlay;

        [Cached]
        private ProfileButton profileButton = new();

        [Cached]
        private ScreenManager screenManager;

        public TestSceneGameplay()
        {
            // player = new TestCharacter(1);
            player = new Airxy();
            testWeapon = new Bow();
            player.SetWeapon(testWeapon);
            Add(screens = new ScreenStack());
            Add(gameMenuOverlay = new GameMenuOverlay());
            screenManager = new ScreenManager(screens);
            screens.Push(gameplayScreen = new GameplayScreen());
        }

        [Test]
        public void Gameplay()
        {
            AddStep("Ready", () =>
            {
                profileButton = new ProfileButton();
                gameMenuOverlay.Disappear();
                user.Value = new GuestUser();
                user.Value.AddItem(player);
                user.Value.AddItem(new Sword());
                user.Value.AddItem(new Spear());
                user.Value.AddItem(new Hammer());
                user.Value.AddItem(new BraydensOsuPen());
                user.Value.EquippedCharacter = player;
                gameplayScreen.LoadGameplay(user.Value, new TestMap());
            });
        }
    }
}
