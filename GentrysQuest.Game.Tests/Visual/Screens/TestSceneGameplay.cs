using GentrysQuest.Game.Content.Characters;
using GentrysQuest.Game.Content.Effects;
using GentrysQuest.Game.Content.Weapons;
using GentrysQuest.Game.Entity;
using GentrysQuest.Game.Entity.Weapon;
using GentrysQuest.Game.Screens;
using NUnit.Framework;
using osu.Framework.Screens;

namespace GentrysQuest.Game.Tests.Visual.Screens
{
    [TestFixture]
    public partial class TestSceneGameplay : GentrysQuestTestScene
    {
        private ScreenStack screens;
        private GameplayScreen gameplayScreen;
        private Character theGuy;
        private Weapon testWeapon;

        public TestSceneGameplay()
        {
            theGuy = new TestCharacter(1);
            testWeapon = new Bow();
            theGuy.SetWeapon(testWeapon);
            Add(screens = new ScreenStack());
            screens.Push(gameplayScreen = new GameplayScreen());
        }

        [Test]
        public void Gameplay()
        {
            AddStep("Ready", () => { });
            AddStep("AddEnemy", () => gameplayScreen.AddEnemy(theGuy.Experience.Level.Current.Value));
            // AddSliderStep("Difficulty", 0, 10, 0, i => gameplay.SetDifficulty(i));
            AddStep("Damage", (() => theGuy.Damage(10)));
            AddStep("Slow", () => theGuy.AddEffect(new Slowness()));
            AddStep("Burn", () => theGuy.AddEffect(new Burn()));
            AddStep("Spawn Enemys", () => gameplayScreen.SpawnEntities());
            AddStep("End", () => gameplayScreen.End());
        }
    }
}
