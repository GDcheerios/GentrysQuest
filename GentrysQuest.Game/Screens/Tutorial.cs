using GentrysQuest.Game.Audio;
using GentrysQuest.Game.Content.Characters;
using GentrysQuest.Game.Content.Effects;
using GentrysQuest.Game.Content.Enemies;
using GentrysQuest.Game.Content.Families.Intro;
using GentrysQuest.Game.Content.Maps;
using GentrysQuest.Game.Content.Weapons;
using GentrysQuest.Game.Entity;
using GentrysQuest.Game.Entity.Drawables;
using GentrysQuest.Game.Graphics.Dialogue;
using GentrysQuest.Game.Location;
using GentrysQuest.Game.Online;
using GentrysQuest.Game.Overlays;
using GentrysQuest.Game.Screens.Gameplay;
using GentrysQuest.Game.Users;
using GentrysQuest.Game.Utils;
using osu.Framework.Allocation;
using osu.Framework.Audio.Sample;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Audio;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Screens;
using osuTK;
using GentrysClassroom = GentrysQuest.Game.Content.Maps.GentrysClassroom;

namespace GentrysQuest.Game.Screens
{
    public partial class Tutorial : GqScreen
    {
        [Resolved]
        private DiscordRpc discordRpc { get; set; }

        [Resolved]
        private GameMenuOverlay gameMenuOverlay { get; set; }

        [Resolved]
        private Bindable<IUser> user { get; set; }

        private DrawablePlayableEntity player;
        private DrawableEnemyEntity enemy;
        private GameplayHud gameplayHud;

        private SpriteText keyMovementText;
        private SpriteText keyAttackText;
        private SpriteText keyDodgeText;
        private SpriteText keySkillText;
        private SpriteText InventoryText;

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

        private DrawableSample flashbangSound;
        private DrawableSample lightDie;

        private Box flashCover;

        public Tutorial()
        {
            scene.LoadMap(new GentrysClassroom());

            gameplayHud = new GameplayHud();

            introScript = new SceneScript();

            #region introScript

            // introScript.AddEvent("Fade In", new SceneEvent
            // {
            //     Event =
            //         () =>
            //         {
            //             scene.GetMap().FadeOut().Then().FadeIn(1000);
            //             AudioManager.Instance.ChangeMusic(new Content.Music.GentrysClassroom());
            //         }
            // });
            // introScript.AddEvent("Pan Up", new SceneEvent
            // {
            //     Event =
            //         () =>
            //         {
            //             scene.GetMap().MoveToY(scene.GetNpc("GMoney").Y, 5000);
            //         },
            //     Delay = 1100
            // });
            // introScript.AddEvent("G-Dialogue", new SceneEvent
            // {
            //     DialogueEvent = new DialogueEvent
            //     {
            //         Author = "Mr.Gentry",
            //         Text = "Hello students,",
            //         Duration = 1000
            //     },
            //     Duration = 3000,
            //     Delay = 7000
            // });
            // introScript.AddEvent("G-Dialogue", new SceneEvent
            // {
            //     DialogueEvent = new DialogueEvent
            //     {
            //         Author = "Mr.Gentry",
            //         Text = "Today we’ll be going over chapter 2 of computer programming.",
            //         Duration = 4300
            //     },
            //     Duration = 7000
            // });
            // introScript.AddEvent("C-Dialogue", new SceneEvent
            // {
            //     DialogueEvent = new DialogueEvent
            //     {
            //         Author = "Class",
            //         Text = "...",
            //         Duration = 1000,
            //     },
            //     Duration = 2000,
            //     Delay = 2000
            // });
            // introScript.AddEvent("G-Dialogue", new SceneEvent
            // {
            //     DialogueEvent = new DialogueEvent
            //     {
            //         Author = "Mr.Gentry",
            //         Text = "Okay...",
            //         Duration = 500
            //     },
            //     Duration = 2000,
            //     Delay = 2000
            // });
            // introScript.AddEvent("G-Dialogue", new SceneEvent
            // {
            //     DialogueEvent = new DialogueEvent
            //     {
            //         Author = "Mr.Gentry",
            //         Text = "So, today…",
            //         Duration = 1000
            //     },
            //     Duration = 2000
            // });
            // introScript.AddEvent("G-Dialogue", new SceneEvent
            // {
            //     DialogueEvent = new DialogueEvent
            //     {
            //         Author = "Mr.Gentry",
            //         Text = "Today will be more of a fun day.",
            //         Duration = 2000
            //     },
            //     Duration = 3000
            // });
            introScript.AddEvent("Power Outage", new SceneEvent
            {
                Duration = 2000, Delay = 2000,
                Event = () =>
                {
                    scene.UnloadMap();
                    AudioManager.Instance.StopMusic();
                    AudioManager.Instance.PlaySound(lightDie);
                }
            });
            // introScript.AddEvent("G-Dialogue", new SceneEvent
            // {
            //     DialogueEvent = new DialogueEvent
            //     {
            //         Author = "Mr.Gentry",
            //         Text = "...",
            //         Duration = 1000,
            //     },
            //     Duration = 2000
            // });
            // introScript.AddEvent("G-Dialogue", new SceneEvent
            // {
            //     DialogueEvent = new DialogueEvent
            //     {
            //         Author = "Mr.Gentry",
            //         Text = "uhh",
            //         Duration = 1000,
            //     },
            //     Duration = 1500
            // });
            // introScript.AddEvent("Evil Gentry First Convo", new SceneEvent
            // {
            //     Delay = 2500, Duration = 5000,
            //     DialogueEvent = new DialogueEvent
            //     {
            //         Author = "???",
            //         Text = "Finally...",
            //         Duration = 2000
            //     },
            //     Event = () =>
            //     {
            //         AudioManager.Instance.ChangeMusic(new AMBI());
            //     }
            // });
            // introScript.AddEvent("EG-dialogue", new SceneEvent
            // {
            //     Duration = 8000,
            //     DialogueEvent = new DialogueEvent
            //     {
            //         Author = "???",
            //         Text = "I've been waiting years for this moment to come.",
            //         Duration = 5000
            //     }
            // });
            // introScript.AddEvent("G-dialogue", new SceneEvent
            // {
            //     Delay = 1000, Duration = 3000,
            //     DialogueEvent = new DialogueEvent
            //     {
            //         Author = "Mr.Gentry",
            //         Text = "Who are you?",
            //         Duration = 500
            //     }
            // });
            // introScript.AddEvent("EG-dialogue", new SceneEvent
            // {
            //     Delay = 1500, Duration = 3000,
            //     DialogueEvent = new DialogueEvent
            //     {
            //         Author = "???",
            //         Text = "hmhmhmhmhm",
            //         Duration = 1500
            //     }
            // });
            // introScript.AddEvent("EG-dialogue", new SceneEvent
            // {
            //     Delay = 2000, Duration = 7000,
            //     DialogueEvent = new DialogueEvent
            //     {
            //         Author = "???",
            //         Text = "You actually know me quite well.",
            //         Duration = 5000
            //     }
            // });
            // introScript.AddEvent("EG-dialogue", new SceneEvent
            // {
            //     Delay = 0, Duration = 3000,
            //     DialogueEvent = new DialogueEvent
            //     {
            //         Author = "???",
            //         Text = "In fact...",
            //         Duration = 1000
            //     }
            // });
            // introScript.AddEvent("EG-dialogue", new SceneEvent
            // {
            //     Delay = 0, Duration = 6000,
            //     DialogueEvent = new DialogueEvent
            //     {
            //         Author = "???",
            //         Text = "I'd say you and I...",
            //         Duration = 4000
            //     }
            // });
            // introScript.AddEvent("dialogue", new SceneEvent
            // {
            //     Delay = 2000, Duration = 5000,
            //     DialogueEvent = new DialogueEvent
            //     {
            //         Author = "???",
            //         Text = "Are the same.",
            //         Duration = 2000
            //     }
            // });
            introScript.AddEvent("new map", new SceneEvent
            {
                Event = () =>
                {
                    scene.RemoveNpc(scene.GetMap().Npcs[^1]);
                    scene.FadeIn();
                    scene.AddPlayer(player = new DrawablePlayableEntity(new GMoney()) { Y = -200, X = -200 });
                    player.EntityBar.OnlyShowName();
                    player.GetBase().AddEffect(new Paused());
                    scene.LoadMap(new EvilGentrysVoid());
                    DrawableEnemyEntity lostSpirit1 = new DrawableEnemyEntity(new LostSpirit()) { Y = -500 };
                    DrawableEnemyEntity lostSpirit2 = new DrawableEnemyEntity(new LostSpirit()) { Y = -2500 };
                    DrawableEnemyEntity lostSpirit3 = new DrawableEnemyEntity(new LostSpirit()) { Y = -3000, X = -200 };
                    DrawableEnemyEntity lostSpirit4 = new DrawableEnemyEntity(new LostSpirit()) { Y = -4000, X = 200 };
                    DrawableEnemyEntity lostSpirit5 = new DrawableEnemyEntity(new LostSpirit()) { Y = -5000 };

                    lostSpirit1.GetBase().Stats.Health.Current.Value = 100;
                    lostSpirit2.GetBase().Stats.Health.Current.Value = 100;
                    lostSpirit3.GetBase().Stats.Health.Current.Value = 100;
                    lostSpirit4.GetBase().Stats.Health.Current.Value = 100;
                    lostSpirit5.GetBase().Stats.Health.Current.Value = 100;

                    lostSpirit1.GetBase().Stats.Speed.Current.Value = 0.25;
                    lostSpirit2.GetBase().Stats.Speed.Current.Value = 0.25;
                    lostSpirit3.GetBase().Stats.Speed.Current.Value = 0.25;
                    lostSpirit4.GetBase().Stats.Speed.Current.Value = 0.25;
                    lostSpirit5.GetBase().Stats.Speed.Current.Value = 0.25;

                    lostSpirit5.GetBase().OnDeath += () =>
                    {
                        Keyboard keyboardArtifact = new Keyboard
                        {
                            MainAttribute = new Buff(20, StatType.Health, true)
                        };
                        user.Value.AddItem(keyboardArtifact);
                        InventoryText.FadeIn(200);
                    };

                    scene.AddEnemy(lostSpirit1);
                    scene.AddEnemy(lostSpirit2);
                    scene.AddEnemy(lostSpirit3);
                    scene.AddEnemy(lostSpirit4);
                    scene.AddEnemy(lostSpirit5);
                },
                Delay = 10
            });
            introScript.AddEvent("Pan to player", new SceneEvent
            {
                Event = () =>
                {
                    user.Value.AddItem(player.GetBase());
                    player.MoveTo(Vector2.Zero, 5000);
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
                            keyMovementText.FadeIn(250, Easing.OutQuint);
                        }, 6000
                    );

                    player.OnMove += hideMovementText;
                    player.GetBase().OnHitEntity += hideAttackText;
                    player.OnDodge += hideDodgeText;
                    player.GetBase().SetWeapon(new Sword());
                    player.GetBase().Weapon!.Buff.Value.Value = 0;
                    player.EntityBar.ShowAll();
                    player.GetBase().RemoveEffect("Paused");
                    player.GetBase().Heal();
                    player.SetupClickContainer();
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
                    player.GetBase().Spawn();

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
            afterCombatScript.AddEvent("Flashbang", new SceneEvent
            {
                Event = () =>
                {
                    enemy.FadeOut(250);
                    enemy.MoveTo(Vector2.Zero, 250);
                    scene.GetMap().MoveTo(new Vector2(0, 0), 500);
                    Scheduler.AddDelayed(() =>
                    {
                        AudioManager.Instance.PlaySound(flashbangSound);
                        flashCover.FadeIn(25, Easing.OutQuint);
                    }, 300);
                },
                Delay = 1000
            });
            afterCombatScript.AddEvent("UnloadMap", new SceneEvent
            {
                Event = () =>
                {
                    scene.UnloadMap();
                },
                Delay = 1000
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
                    enemy.GetBase().Stats.Speed.Current.Value += 0.2;
                    displayEnemyDialogue(new DialogueEvent
                    {
                        Text = "You shouldn't be this strong.",
                        Duration = 1000
                    });
                    break;

                case var x when x <= 0.1f && !tenPercent:
                    tenPercent = true;
                    enemy.GetBase().Stats.Speed.Current.Value += 0.2;
                    displayEnemyDialogue(new DialogueEvent
                    {
                        Text = "Not a chance",
                        Duration = 1000
                    });
                    break;

                case var x when x <= 0.25f && !twentyFivePercent:
                    twentyFivePercent = true;
                    enemy.GetBase().Stats.Speed.Current.Value += 0.2;
                    displayEnemyDialogue(new DialogueEvent
                    {
                        Text = "No",
                        Duration = 1000
                    });
                    break;

                case var x when x <= 0.5f && !fiftyPercent:
                    fiftyPercent = true;
                    enemy.GetBase().Stats.Speed.Current.Value += 0.2;
                    displayEnemyDialogue(new DialogueEvent
                    {
                        Text = "You're pretty strong",
                        Duration = 1000
                    });
                    break;

                case var x when x <= 0.8f && !eightyPercent:
                    eightyPercent = true;
                    displayEnemyDialogue(new DialogueEvent
                    {
                        Text = "...",
                        Duration = 1000
                    });
                    break;

                case var x when x <= 0.9f && !ninetyPercent:
                    ninetyPercent = true;
                    displayEnemyDialogue(new DialogueEvent
                    {
                        Text = "You've got some fight in you.",
                        Duration = 1000
                    });
                    break;
            }
        }

        public void TeleportEnemy()
        {
            Stun stun = new Stun(10000);
            enemy.GetBase().AddEffect(stun);
            enemy.Weapon.StopAnimation();
            enemy.FadeOutFromOne(150).Then()
                 .MoveTo(350 * MathBase.GetAngleToVector(MathBase.RandomInt(0, 360)), 150).Then()
                 .FadeInFromZero(150).Then().Finally(_ => { enemy.GetBase().RemoveEffect(stun); });
        }

        private void displayEnemyDialogue(DialogueEvent dialogueEvent)
        {
            TeleportEnemy();
            AutoDialogueBox dialogueBox = new AutoDialogueBox("Evil Gentry", dialogueEvent.Text);
            AddInternal(dialogueBox);
            Scheduler.AddDelayed(() => { dialogueBox.FadeOut(500); }, dialogueEvent.Duration + 500);
            Scheduler.AddDelayed(() => { RemoveInternal(dialogueBox, true); }, dialogueEvent.Duration + 1000);
        }

        private void hideMovementText(Vector2 direction, double speed)
        {
            if (!keyMovementText.IsPresent) return;

            keyMovementText.FadeOut(500);
            keyAttackText.Delay(1000).Then().MoveToX(-0.1f, 500, Easing.OutQuint);
        }

        private void hideAttackText(DamageDetails details)
        {
            if (!keyAttackText.IsPresent) return;

            keyAttackText.FadeOut(500);
            keyDodgeText.Delay(1000).Then().FadeIn(500, Easing.OutQuint);
        }

        private void hideDodgeText()
        {
            if (!keyDodgeText.IsPresent) return;

            keyDodgeText.FadeOut(500);
            keySkillText.Delay(1000).Then()
                        .MoveToX(-0.1f, 500, Easing.OutQuint).Then()
                        .Delay(6000).Then()
                        .FadeOut(500, Easing.OutQuint);
        }

        [BackgroundDependencyLoader]
        private void load(ISampleStore samples)
        {
            AddInternal(scene);
            AddInternal(gameplayHud);
            gameplayHud.Disappear();

            flashbangSound = new DrawableSample(samples.Get("flashbang.mp3"));
            lightDie = new DrawableSample(samples.Get("light-die.mp3"));

            AddInternal(keyMovementText = new SpriteText
            {
                RelativePositionAxes = Axes.Both,
                X = 0.05f,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Alpha = 0,
                Text = "Use WASD to move around.",
                Font = FontUsage.Default.With(size: 70),
                Y = -0.15f
            });

            AddInternal(keyAttackText = new SpriteText
            {
                RelativePositionAxes = Axes.Both,
                X = 0.6f,
                Anchor = Anchor.BottomRight,
                Origin = Anchor.BottomRight,
                Text = "Use LEFT MOUSE button for main attack.",
                Font = FontUsage.Default.With(size: 70),
                Y = -0.2f
            });

            AddInternal(keyDodgeText = new SpriteText
            {
                RelativePositionAxes = Axes.Both,
                X = 0.05f,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Alpha = 0,
                Text = "Use SHIFT to dodge attacks.",
                Font = FontUsage.Default.With(size: 70),
                Y = -0.15f
            });

            AddInternal(keySkillText = new SpriteText
            {
                RelativePositionAxes = Axes.Both,
                X = 0.6f,
                Anchor = Anchor.BottomRight,
                Origin = Anchor.BottomRight,
                Text = "You can also use RIGHT MOUSE, SPACE, or R to use skills.",
                Font = FontUsage.Default.With(size: 50),
                Y = -0.2f
            });

            AddInternal(InventoryText = new SpriteText
            {
                Text = "Press C to open inventory",
                Origin = Anchor.Centre,
                Anchor = Anchor.Centre,
                Font = FontUsage.Default.With(size: 100),
                Depth = -5,
                Alpha = 0
            });

            AddInternal(flashCover = new Box
            {
                RelativeSizeAxes = Axes.Both,
                Colour = Colour4.White,
                Alpha = 0
            });
        }

        public override void OnEntering(ScreenTransitionEvent e)
        {
            base.OnEntering(e);
            introScript.Start(Overlay, Scheduler);
            discordRpc.UpdatePresence("Tutorial", "Playing");
        }
    }
}
