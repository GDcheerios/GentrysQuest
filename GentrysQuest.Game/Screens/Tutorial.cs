using GentrysQuest.Game.Audio;
using GentrysQuest.Game.Content.Characters;
using GentrysQuest.Game.Content.Effects;
using GentrysQuest.Game.Content.Music;
using GentrysQuest.Game.Content.Weapons;
using GentrysQuest.Game.Entity.Drawables;
using GentrysQuest.Game.Graphics.Dialogue;
using GentrysQuest.Game.Location;
using GentrysQuest.Game.Screens.Gameplay;
using GentrysQuest.Game.Utils;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Screens;
using osuTK;
using GentrysClassroom = GentrysQuest.Game.Content.Maps.GentrysClassroom;

namespace GentrysQuest.Game.Screens
{
    public partial class Tutorial : GqScreen
    {
        private DrawablePlayableEntity player;
        private DrawableEnemyEntity enemy;
        private GameplayHud gameplayHud;

        private SpriteText keyMovementText;

        private readonly MapScene scene = new();

        private readonly SceneScript introScript = new();
        private readonly SceneScript afterCombatScript = new();

        private bool ninetyPercent = false;
        private bool eightyPercent = false;
        private bool fiftyPercent = false;
        private bool twentyFivePercent = false;
        private bool tenPercent = false;
        private bool fivePercent = false;
        private bool onePercent = false;

        public Tutorial()
        {
            scene.LoadMap(new GentrysClassroom());

            gameplayHud = new GameplayHud();

            #region introScript

            introScript.AddEvent("Fade In", new SceneEvent
            {
                Event =
                    () =>
                    {
                        scene.GetMap().FadeOut().Then().FadeIn(1000);
                        AudioManager.Instance.ChangeMusic(new Content.Music.GentrysClassroom());
                    }
            });
            introScript.AddEvent("Pan Up", new SceneEvent
            {
                Event =
                    () =>
                    {
                        scene.GetMap().MoveToY(450, 5000);
                    },
                Delay = 1100
            });
            introScript.AddEvent("G-Dialogue1", new SceneEvent
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
            introScript.AddEvent("C-Dialogue", new SceneEvent
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
            introScript.AddEvent("G-Dialogue2", new SceneEvent
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
            introScript.AddEvent("Power Outage", new SceneEvent
            {
                Duration = 2000, Delay = 2000,
                Event = () => { scene.FadeTo(0); }
            });
            introScript.AddEvent("G-Dialogue3", new SceneEvent
            {
                DialogueEvent = new DialogueEvent
                {
                    Author = "Mr.Gentry",
                    Text = "...",
                    Duration = 1000,
                },
                Duration = 2000
            });
            introScript.AddEvent("G-Dialogue4", new SceneEvent
            {
                DialogueEvent = new DialogueEvent
                {
                    Author = "Mr.Gentry",
                    Text = "uhh",
                    Duration = 1000,
                },
                Duration = 1000
            });
            introScript.AddEvent("Evil Gentry Entrance", new SceneEvent
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
            introScript.AddEvent("Pan to player", new SceneEvent
            {
                Event = () =>
                {
                    scene.GetMap().MoveTo(new Vector2(-player.X, GentrysClassroom.CLASSROOM_HEIGHT - 1100), 1000);
                    player.MoveTo(Vector2.Zero, 1000);
                    enemy.MoveTo(new Vector2(enemy.X + 200, 0), 1000);
                }
            });
            introScript.AddEvent("Initiate Combat Tutorial", new SceneEvent
            {
                Event = () =>
                {
                    Scheduler.AddDelayed(() =>
                        {
                            gameplayHud.Appear();
                            gameplayHud.SetEntity(player.GetBase());
                            keyMovementText.MoveToY(0.05f, 250, Easing.OutQuint).Then()
                                           .Delay(3000).Then()
                                           .FadeOut(500, Easing.OutQuint);
                        }, 250
                    );

                    AudioManager.Instance.ChangeMusic(new Anguish());

                    player.GetBase().SetWeapon(new Sword());
                    player.EntityBar.ShowAll();
                    player.GetBase().RemoveEffect("Paused");
                    player.SetupClickContainer();
                    player.Weapon.GetBase().OnHitEntity += (details) =>
                    {
                        if (details.GetHitAmount() == 1 && details.Receiver == enemy.GetBase())
                        {
                            displayDialogue(new DialogueEvent
                            {
                                Author = "Evil Gentry",
                                Text = "You really wanna try and fight me?",
                                Duration = 1000
                            });
                        }
                    };
                    // player.GetBase().OnDeath += () =>

                    enemy.GetBase().CanDie = false;
                    enemy.GetBase().SetWeapon(new Sword());
                    enemy.EntityBar.ShowAll();
                    enemy.FollowEntity(player);
                    enemy.GetBase().OnDamage += _ => checkEnemyHealth();
                    Scheduler.AddDelayed(() => { enemy.GetBase().RemoveEffect("Paused"); }, 1000);
                },
                Delay = 1000
            });

            #endregion

            #region afterCombatScript

            afterCombatScript.AddEvent("AfterFight", new SceneEvent
            {
                Event = () =>
                {
                    gameplayHud.Disappear();

                    AudioManager.Instance.StopMusic();

                    player.EntityBar.OnlyShowName();
                    player.GetBase().AddEffect(new Paused());
                    player.GetBase().SetWeapon(null);

                    enemy.EntityBar.OnlyShowName();
                    enemy.GetBase().AddEffect(new Paused());
                    enemy.GetBase().SetWeapon(null);
                }
            });
            afterCombatScript.AddEvent("EG-dialogue1", new SceneEvent
            {
                DialogueEvent = new DialogueEvent
                {
                    Author = "Evil Gentry",
                    Text = "Actually...",
                    Duration = 20
                },
                Duration = 2000,
                Delay = 1000
            });
            afterCombatScript.AddEvent("EG-dialogue2", new SceneEvent
            {
                DialogueEvent = new DialogueEvent
                {
                    Author = "Evil Gentry",
                    Text = "You're pretty strong...",
                    Duration = 2000
                },
                Duration = 5000,
                Delay = 1000
            });
            afterCombatScript.AddEvent("EG-dialogue3", new SceneEvent
            {
                DialogueEvent = new DialogueEvent
                {
                    Author = "Evil Gentry",
                    Text = "For a frisbee golf player!",
                    Duration = 2000
                },
                Duration = 2000,
                Delay = 2000
            });

            #endregion
        }

        private void checkEnemyHealth()
        {
            int currentHealth = (int)enemy.GetBase().Stats.Health.Current.Value;
            float healthPercent = (float)(currentHealth / enemy.GetBase().Stats.Health.Total());

            switch (healthPercent)
            {
                case var x when x <= 0.01f && !onePercent:
                    onePercent = true;
                    afterCombatScript.Start(Overlay, Scheduler);
                    break;

                case var x when x <= 0.05f && !fivePercent:
                    fivePercent = true;
                    displayDialogue(new DialogueEvent
                    {
                        Author = "Evil Gentry",
                        Text = "You shouldn't be this strong.",
                        Duration = 1000
                    });
                    break;

                case var x when x <= 0.1f && !tenPercent:
                    tenPercent = true;
                    displayDialogue(new DialogueEvent
                    {
                        Author = "Evil Gentry",
                        Text = "Not a chance",
                        Duration = 1000
                    });
                    break;

                case var x when x <= 0.25f && !twentyFivePercent:
                    twentyFivePercent = true;
                    displayDialogue(new DialogueEvent
                    {
                        Author = "Evil Gentry",
                        Text = "No",
                        Duration = 1000
                    });
                    break;

                case var x when x <= 0.5f && !fiftyPercent:
                    fiftyPercent = true;
                    displayDialogue(new DialogueEvent
                    {
                        Author = "Evil Gentry",
                        Text = "You're pretty strong",
                        Duration = 1000
                    });
                    break;

                case var x when x <= 0.8f && !eightyPercent:
                    eightyPercent = true;
                    displayDialogue(new DialogueEvent
                    {
                        Author = "Evil Gentry",
                        Text = "...",
                        Duration = 1000
                    });
                    break;

                case var x when x <= 0.9f && !ninetyPercent:
                    ninetyPercent = true;
                    displayDialogue(new DialogueEvent
                    {
                        Author = "Evil Gentry",
                        Text = "You've got some fight in you.",
                        Duration = 1000
                    });
                    break;
            }
        }

        private void displayDialogue(DialogueEvent dialogueEvent)
        {
            AutoDialogueBox dialogueBox = new AutoDialogueBox(dialogueEvent.Author, dialogueEvent.Text);
            AddInternal(dialogueBox);
            Scheduler.AddDelayed(() => { dialogueBox.FadeOut(500); }, dialogueEvent.Duration + 500);
            Scheduler.AddDelayed(() => { RemoveInternal(dialogueBox, true); }, dialogueEvent.Duration + 1000);
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            AddInternal(scene);
            AddInternal(gameplayHud);
            gameplayHud.Disappear();

            AddInternal(keyMovementText = new SpriteText
            {
                RelativePositionAxes = Axes.Both,
                X = 0.05f,
                Anchor = Anchor.TopLeft,
                Origin = Anchor.TopLeft,
                Text = "Use WASD to move around.",
                Font = FontUsage.Default.With(size: 50),
                Y = -0.05f
            });

            scene.GetMap().Y = -400;
        }

        public override void OnEntering(ScreenTransitionEvent e)
        {
            base.OnEntering(e);
            introScript.Start(Overlay, Scheduler);
        }
    }
}
