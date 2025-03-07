using GentrysQuest.Game.Content.Characters;
using GentrysQuest.Game.Content.Effects;
using GentrysQuest.Game.Content.Maps;
using GentrysQuest.Game.Content.Weapons;
using GentrysQuest.Game.Entity.Drawables;
using GentrysQuest.Game.Location;
using GentrysQuest.Game.Screens.Gameplay;
using GentrysQuest.Game.Utils;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Screens;
using osuTK;

namespace GentrysQuest.Game.Screens
{
    public partial class Tutorial : GqScreen
    {
        private DrawablePlayableEntity player;
        private DrawableEnemyEntity enemy;
        private GameplayHud gameplayHud;

        private readonly MapScene scene = new();

        private readonly SceneScript script = new();

        public Tutorial()
        {
            scene.LoadMap(new GentrysClassroom());

            gameplayHud = new GameplayHud();

            #region Script

            script.AddEvent("Fade In", new SceneEvent
            {
                Event =
                    () =>
                    {
                        scene.GetMap().FadeOut().Then().FadeIn(1000);
                    }
            });
            script.AddEvent("Pan Up", new SceneEvent
            {
                Event =
                    () =>
                    {
                        scene.GetMap().MoveToY(450, 5000);
                    },
                Delay = 1100
            });
            script.AddEvent("G-Dialogue1", new SceneEvent
            {
                DialogueEvent = new DialogueEvent
                {
                    Author = "Mr.Gentry",
                    Text = "Hello students, Today we’ll be going over chapter 2 of computer programming.",
                    Duration = 4000
                },
                Duration = 7000,
                Delay = 7000
            });
            script.AddEvent("C-Dialogue", new SceneEvent
            {
                DialogueEvent = new DialogueEvent
                {
                    Author = "Class",
                    Text = "...",
                    Duration = 1000,
                },
                Duration = 2000,
                Delay = 2000
            });
            script.AddEvent("G-Dialogue2", new SceneEvent
            {
                DialogueEvent = new DialogueEvent
                {
                    Author = "Mr.Gentry",
                    Text = "Okay so, today…today will be more of a fun day.",
                    Duration = 2000
                },
                Duration = 2000,
                Delay = 2000
            });
            script.AddEvent("Power Outage", new SceneEvent
            {
                Duration = 2000, Delay = 2000,
                Event = () => { scene.FadeTo(0); }
            });
            script.AddEvent("G-Dialogue3", new SceneEvent
            {
                DialogueEvent = new DialogueEvent
                {
                    Author = "Mr.Gentry",
                    Text = "...",
                    Duration = 1000,
                },
                Duration = 2000
            });
            script.AddEvent("G-Dialogue4", new SceneEvent
            {
                DialogueEvent = new DialogueEvent
                {
                    Author = "Mr.Gentry",
                    Text = "uhh",
                    Duration = 1000,
                },
                Duration = 1000
            });
            script.AddEvent("Evil Gentry Entrance", new SceneEvent
            {
                Event = () =>
                {
                    scene.RemoveNpc(scene.GetMap().Npcs[^1]);
                    scene.FadeIn();
                    scene.AddPlayer(player = new DrawablePlayableEntity(new GMoney()) { Y = -200, X = -200 });
                    scene.AddEnemy(enemy = new DrawableEnemyEntity(new EvilGentry()) { Y = -200, X = 200 });
                    player.EntityBar.OnlyShowName();
                    enemy.EntityBar.OnlyShowName();
                    player.GetBase().AddEffect(new Paused());
                    enemy.GetBase().AddEffect(new Paused());
                },
                Delay = 10
            });
            script.AddEvent("Pan to player", new SceneEvent
            {
                Event = () =>
                {
                    scene.GetMap().MoveTo(new Vector2(-player.X, GentrysClassroom.CLASSROOM_HEIGHT - 1100), 1000);
                    player.MoveTo(Vector2.Zero, 1000);
                    enemy.MoveTo(new Vector2(enemy.X + 200, 0), 1000);
                }
            });
            script.AddEvent("Initiate Combat Tutorial", new SceneEvent
            {
                Event = () =>
                {
                    gameplayHud.Appear();
                    gameplayHud.SetEntity(player.GetBase());

                    player.GetBase().SetWeapon(new Sword());
                    player.EntityBar.ShowAll();
                    player.GetBase().RemoveEffect("Paused");
                    player.SetupClickContainer();

                    enemy.GetBase().SetWeapon(new Sword());
                    enemy.EntityBar.ShowAll();
                    enemy.FollowEntity(player);
                    enemy.GetBase().RemoveEffect("Paused");
                },
                Delay = 1000
            });

            #endregion
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            AddInternal(scene);
            AddInternal(gameplayHud);
            gameplayHud.Disappear();

            scene.GetMap().Y = -400;
        }

        public override void OnEntering(ScreenTransitionEvent e)
        {
            base.OnEntering(e);
            script.Start(Overlay, Scheduler);
        }
    }
}
