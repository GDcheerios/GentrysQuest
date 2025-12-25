using System.Linq;
using GentrysQuest.Game.Audio;
using GentrysQuest.Game.Content.Characters;
using GentrysQuest.Game.Content.Maps;
using GentrysQuest.Game.Content.Music;
using GentrysQuest.Game.Entity.Drawables;
using GentrysQuest.Game.Graphics.Dialogue;
using GentrysQuest.Game.Input;
using GentrysQuest.Game.Location;
using GentrysQuest.Game.Online;
using GentrysQuest.Game.Overlays;
using GentrysQuest.Game.Quests;
using GentrysQuest.Game.Screens.Gameplay;
using GentrysQuest.Game.Users;
using GentrysQuest.Game.Utils;
using osu.Framework.Allocation;
using osu.Framework.Audio.Sample;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Screens;
using osuTK.Input;
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

        [Resolved]
        private InputHandler inputHandler { get; set; }

        private DrawablePlayableEntity player;
        private GameplayHud gameplayHud;

        private readonly MapScene scene = new();

        private readonly QuestOverlay questOverlay = new();

        private readonly SceneScript introScript;

        private bool[] movementButtons = new bool[4];

        private bool ninetyPercent = false;
        private bool eightyPercent = false;
        private bool fiftyPercent = false;
        private bool twentyFivePercent = false;
        private bool tenPercent = false;
        private bool fivePercent = false;
        private bool onePercent = false;

        public Tutorial()
        {
            gameplayHud = new GameplayHud();

            introScript = new SceneScript();

            #region introScript

            introScript.AddEvent("Pro1", new SceneEvent
            {
                DialogueEvent = new DialogueEvent
                {
                    Text = "Since 1956 [p:500] around 500 people a year go missing.",
                    Duration = 4000
                },
                Duration = 3000,
                Delay = 1000
            });
            introScript.AddEvent("Pro2", new SceneEvent
            {
                DialogueEvent = new DialogueEvent
                {
                    Text = "Another 1,000 to 10,000 become stuck in a coma due to unknown reasons.",
                    Duration = 6000
                },
                Duration = 4000,
                Delay = 1000
            });
            introScript.AddEvent("Pro3", new SceneEvent
            {
                DialogueEvent = new DialogueEvent
                {
                    Text = "About 5% of the comatose individuals end up waking up, [p:500] and none of the missing people ever come back."
                },
                Delay = 1000,
                Duration = 6000
            });
            introScript.AddEvent("Fade In", new SceneEvent
            {
                Event =
                    () =>
                    {
                        scene.LoadMap(new GentrysClassroom(true));
                        scene.GetMap().FadeOut().Then().FadeIn(1000);
                        AudioManager.Instance.ChangeMusic(new Content.Music.GentrysClassroom());
                    }
            });
            introScript.AddEvent("Pan Up", new SceneEvent
            {
                Event =
                    () =>
                    {
                        scene.GetMap().MoveToY(scene.GetNpc("GMoney").Y, 5000);
                    },
                Delay = 1100
            });
            introScript.AddEvent("Gentry D1", new SceneEvent
            {
                DialogueEvent = new DialogueEvent
                {
                    Text = "Alright class, [p:500] That will be all the time we have for the test, [p:500] please submit your work.",
                    Author = "Mr. Gentry"
                },
                Duration = 4000,
                Delay = 6000
            });
            introScript.AddEvent("Student D1", new SceneEvent
            {
                DialogueEvent = new DialogueEvent
                {
                    Text = "Wait! [p:200] Mr. Gentry, [p:200] I just need to finish this last recursion problem!",
                    Author = "Student"
                },
                Duration = 3000,
                Delay = 1000
            });
            introScript.AddEvent("Gentry D2", new SceneEvent
            {
                DialogueEvent = new DialogueEvent
                {
                    Text = "You've had an hour to finish. [p:500] You'll be able to retake the test next week.",
                    Author = "Mr. Gentry"
                },
                Duration = 4000,
                Delay = 1000
            });
            introScript.AddEvent("Student D2", new SceneEvent
            {
                DialogueEvent = new DialogueEvent
                {
                    Text = "Oh fine...",
                    Author = "Student"
                },
                Duration = 1000,
                Delay = 1000
            });
            introScript.AddEvent("transition", new SceneEvent
            {
                Event = () =>
                {
                    Scheduler.AddDelayed((() => scene.GetNpc("GMoney").X = -500), 1000);
                    scene.GetMap().FadeOut(1000).Then().FadeIn(1000);
                },
                Delay = 2000
            });
            introScript.AddEvent("Gentry D3", new SceneEvent
            {
                DialogueEvent = new DialogueEvent
                {
                    Text = "Aah... [p:500] There are so many quizzes I have to grade, [p:500] and I decided to stay up late practicing frisbee golf...",
                    Author = "Mr. Gentry"
                },
                Duration = 6000,
                Delay = 3000
            });
            introScript.AddEvent("Gentry D4", new SceneEvent
            {
                DialogueEvent = new DialogueEvent
                {
                    Text = "I could totally just rest my head right now.",
                    Author = "Mr. Gentry"
                },
                Duration = 3000,
                Delay = 3000
            });
            introScript.AddEvent("Gentry D5", new SceneEvent
            {
                DialogueEvent = new DialogueEvent
                {
                    Text = "Wow, [p:100] this is surprisingly comfortable. [p:500] It feels like I'm on a cloud.",
                    Author = "Mr. Gentry"
                },
                Duration = 5000,
                Delay = 3000
            });
            introScript.AddEvent("Gentry Sleep", new SceneEvent
            {
                Event = () =>
                {
                    scene.GetMap().FadeOut(5000);
                },
                Duration = 5000,
                Delay = 3000
            });
            introScript.AddEvent("WhitePlane transition", new SceneEvent
            {
                Event = () =>
                {
                    scene.UnloadMap();
                    scene.LoadMap(new WhitePlane());
                    AudioManager.Instance.StopMusic();
                    AudioManager.Instance.ChangeMusic(new AMBI());
                    scene.GetMap().FadeOut().Then().FadeIn(5000);
                }
            });
            introScript.AddEvent("Gentry D6", new SceneEvent
            {
                DialogueEvent = new DialogueEvent
                {
                    Text = "Where am I?",
                    Author = "Mr. Gentry"
                },
                Duration = 2000,
                Delay = 10000
            });
            introScript.AddEvent("Gentry D7", new SceneEvent
            {
                DialogueEvent = new DialogueEvent
                {
                    Text = "Is anyone there?",
                    Author = "Mr. Gentry"
                },
                Delay = 2000,
                Duration = 2000
            });
            introScript.AddEvent("Gentry D8", new SceneEvent
            {
                DialogueEvent = new DialogueEvent
                {
                    Text = "How did I end up here?",
                    Author = "Mr. Gentry"
                },
                Delay = 4000,
                Duration = 2000
            });
            introScript.AddEvent("Gentry D9", new SceneEvent
            {
                DialogueEvent = new DialogueEvent
                {
                    Text = "I need to keep grading the quizzes!",
                    Author = "Mr. Gentry"
                },
                Delay = 4000,
                Duration = 2000
            });
            introScript.AddEvent("Scene Flicker", new SceneEvent
            {
                Event = () =>
                {
                    scene.GetMap().Unload();
                    scene.LoadMap(new GentrysClassroom(true));
                    scene.GetMap().FadeOut().Then().FadeIn(500);
                    foreach (DrawableEntity npc in scene.GetMap().Npcs.ToList()) scene.RemoveNpc(npc);
                    GMoney gMoney = new GMoney();
                    scene.AddPlayer(player = new DrawablePlayableEntity(gMoney));

                    if (user.Value != null)
                    {
                        user.Value.AddItem(gMoney);
                        user.Value.EquippedCharacter = gMoney;
                    }

                    player.EntityBar.Hide();
                    player.SetupClickContainer();
                    Quest movementQuest = new Quest { Title = "Movement" };
                    movementQuest.AddObjective(new Objective { Name = "Move with WASD", TargetValue = 4 });
                    InputEvent w = new InputEvent { Key = Key.W, Action = () => { handleMovementKey(Key.W); } };
                    InputEvent a = new InputEvent { Key = Key.A, Action = () => { handleMovementKey(Key.A); } };
                    InputEvent s = new InputEvent { Key = Key.S, Action = () => { handleMovementKey(Key.S); } };
                    InputEvent d = new InputEvent { Key = Key.D, Action = () => { handleMovementKey(Key.D); } };
                    inputHandler.AddKeyDownEvent(w);
                    inputHandler.AddKeyDownEvent(a);
                    inputHandler.AddKeyDownEvent(s);
                    inputHandler.AddKeyDownEvent(d);
                    movementQuest.QuestCompleted += _ =>
                    {
                        inputHandler.RemoveKeyDownEvent(w);
                        inputHandler.RemoveKeyDownEvent(a);
                        inputHandler.RemoveKeyDownEvent(s);
                        inputHandler.RemoveKeyDownEvent(d);
                    };
                    QuestManager.PushQuest(movementQuest);
                },
                Duration = 1000
            });
            introScript.AddEvent("Gentry D10", new SceneEvent
            {
                DialogueEvent = new DialogueEvent
                {
                    Text = "I'm back...",
                    Author = "Mr. Gentry"
                },
                Delay = 1000,
                Duration = 1500
            });
            introScript.AddEvent("Gentry D11", new SceneEvent
            {
                DialogueEvent = new DialogueEvent
                {
                    Text = "How did I end up on this side?",
                    Author = "Mr. Gentry"
                },
                Delay = 1000,
                Duration = 3000
            });
            introScript.AddEvent("Gentry D12", new SceneEvent
            {
                DialogueEvent = new DialogueEvent
                {
                    Text = "Whatever, [p:500] I should finish grading the quizzes.",
                    Author = "Mr. Gentry"
                },
                Event = () =>
                {
                    MapObject gradingQuestPlate = scene.GetMap().Objects.First(x => x.Name == "gradingQuestPlate");
                    gradingQuestPlate.FadeTo(0.25f);
                    Quest gradePapers = new Quest
                    {
                        Title = "Grade Quizzes",
                    };
                    gradePapers.AddObjective(new Objective
                    {
                        Name = "Finish grading the Quizzes",
                    });
                    QuestManager.PushQuest(gradePapers);
                    gradePapers.QuestCompleted += _ =>
                    {
                        AutoDialogueBox dialogueBox;
                        Overlay.Add(dialogueBox = new AutoDialogueBox(
                            "Mr. Gentry",
                            "Where did the quizzes go?"
                        ));
                        dialogueBox.Delay(2000).FadeOut(50);
                    };
                },
                Delay = 1000,
                Duration = 3000
            });

            # endregion
        }

        [BackgroundDependencyLoader]
        private void load(ISampleStore samples)
        {
            AddInternal(scene);
            AddInternal(questOverlay);
            questOverlay.Load();
        }

        public override void OnEntering(ScreenTransitionEvent e)
        {
            base.OnEntering(e);
            introScript.Start(Overlay, Scheduler);
            discordRpc.UpdatePresence("Tutorial", "Playing");
        }

        private void handleMovementKey(Key key)
        {
            switch (key)
            {
                case Key.W:
                    movementButtons[0] = true;
                    break;

                case Key.A:
                    movementButtons[1] = true;
                    break;

                case Key.S:
                    movementButtons[2] = true;
                    break;

                case Key.D:
                    movementButtons[3] = true;
                    break;
            }

            QuestManager.SignalSet("Move with WASD", movementButtons.Count(x => x));
        }
    }
}
