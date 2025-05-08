using GentrysQuest.Game.Content.Characters;
using GentrysQuest.Game.Content.Enemies;
using GentrysQuest.Game.Content.Maps;
using GentrysQuest.Game.Content.Weapons;
using GentrysQuest.Game.Entity;
using GentrysQuest.Game.Entity.Drawables;
using GentrysQuest.Game.Location;
using GentrysQuest.Game.Screens.Gameplay;
using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osuTK;

namespace GentrysQuest.Game.Tests.Visual
{
    [TestFixture]
    public partial class TestSceneFight : GentrysQuestTestScene
    {
        private DrawablePlayableEntity player;
        private MapScene mapScene;
        private GameplayHud gameplayHud;
        private StatDrawableContainer statContainer;

        public TestSceneFight()
        {
            Add(new Box
            {
                RelativeSizeAxes = Axes.Both,
                Colour = Colour4.Gray
            });
            player = new DrawablePlayableEntity(new StarterCharacter());
            player.GetBase().SetWeapon(new Sword());
            player.SetupClickContainer();
            mapScene = new MapScene();
            mapScene.AddPlayer(player);
            mapScene.LoadMap(new TestMap());
            gameplayHud = new GameplayHud();
            gameplayHud.SetEntity(player.GetBase());
            statContainer = new StatDrawableContainer()
            {
                Anchor = Anchor.TopRight,
                Origin = Anchor.TopRight,
                RelativeSizeAxes = Axes.None,
                Size = new Vector2(200, 400)
            };
            Add(statContainer);
            Add(gameplayHud);
            Add(mapScene);

            foreach (Stat stat in player.GetBase().Stats.GetStats())
            {
                statContainer.AddStat(new StatDrawable(stat.Name, (float)stat.Total(), false));
            }
        }

        [Test]
        public void Option()
        {
            AddStep("Ready", () => { player.GetBase().UpdateStats(); });
            AddStep("Add enemy", () => mapScene.AddEnemy(new DrawableEnemyEntity(new TestEnemy()) { X = 100, Y = 100 }));
        }

        protected override bool OnClick(ClickEvent e)
        {
            foreach (Stat stat in player.GetBase().Stats.GetStats())
            {
                statContainer.GetStatDrawable(stat.Name).UpdateValue((float)stat.Current.Value);
            }

            return base.OnClick(e);
        }
    }
}
