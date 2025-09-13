using GentrysQuest.Game.Content.Characters;
using GentrysQuest.Game.Content.Enemies;
using GentrysQuest.Game.Content.Maps;
using GentrysQuest.Game.Content.Weapons;
using GentrysQuest.Game.Entity.Drawables;
using GentrysQuest.Game.Location;
using GentrysQuest.Game.Screens.Gameplay;
using NUnit.Framework;
using osu.Framework.Allocation;

namespace GentrysQuest.Game.Tests.Visual.Utils
{
    [TestFixture]
    public partial class TestSceneFight : GentrysQuestTestScene
    {
        private DrawablePlayableEntity player;
        private MapScene mapScene;
        private GameplayHud gameplayHud;
        protected override string TestName { get; init; } = "Test Fight";

        public TestSceneFight()
        {
            player = new DrawablePlayableEntity(new GMoney()) { Depth = -1 };
            player.GetBase().SetWeapon(new Sword());
            player.SetupClickContainer();
            mapScene = new MapScene();
            mapScene.AddPlayer(player);
            mapScene.LoadMap(new TestMap());
            gameplayHud = new GameplayHud();
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            gameplayHud.SetEntity(player.GetBase());
            Add(gameplayHud);
            Add(mapScene);
        }

        [Test]
        public void Option()
        {
            AddStep("Ready", () => { player.GetBase().UpdateStats(); });
            AddStep("Add enemy", () => mapScene.AddEnemy(new DrawableEnemyEntity(new TestEnemy()) { X = 100, Y = 100 }));
        }
    }
}
