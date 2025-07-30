using GentrysQuest.Game.Audio;
using GentrysQuest.Game.Content.Music;
using GentrysQuest.Game.Graphics;
using GentrysQuest.Game.Graphics.TextStyles;
using GentrysQuest.Game.Online;
using GentrysQuest.Game.Online.API;
using GentrysQuest.Game.Overlays;
using GentrysQuest.Game.Overlays.Profile;
using GentrysQuest.Game.Users;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Logging;
using osu.Framework.Screens;
using osuTK;
using osuTK.Graphics;

namespace GentrysQuest.Game.Screens
{
    public partial class MainMenuScreen : GqScreen
    {
        private readonly Box background;

        [Resolved]
        private TitleText title { get; set; }

        private readonly MainGqButton playButton;
        private readonly MainGqButton quitButton;
        private readonly bool keepBgm;

        [Resolved]
        private GameMenuOverlay gameMenuOverlay { get; set; }

        [Resolved]
        private ProfileButton profileButton { get; set; }

        [Resolved]
        private Bindable<IUser> user { get; set; }

        [Resolved]
        private ScreenManager screenManager { get; set; }

        [Resolved]
        private DiscordRpc discordRpc { get; set; }

        public MainMenuScreen(bool keepIntroSong = false)
        {
            keepBgm = keepIntroSong;
            InternalChildren = new Drawable[]
            {
                background = new Box
                {
                    Colour = Color4.Black,
                    RelativeSizeAxes = Axes.Both,
                },
                new FillFlowContainer
                {
                    AutoSizeAxes = Axes.Y,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Y = 100,
                    Spacing = new Vector2(0, 50),
                    Direction = FillDirection.Vertical,
                    Children =
                    [
                        playButton = new MainGqButton("Play")
                        {
                            Size = new Vector2(300, 150),
                            Origin = Anchor.Centre
                        },
                        quitButton = new MainGqButton("Quit")
                        {
                            Size = new Vector2(300, 150),
                            Origin = Anchor.Centre
                        }
                    ]
                }
            };
            playButton.SetAction(PressPlay);
            quitButton.SetAction(delegate
            {
                _ = APIAccess.DeleteToken();
                Game.Exit();
            });
        }

        public void PressPlay()
        {
            if (user.Value == null) profileButton.GoSelection();
            else
            {
                EnterGame();
            }
        }

        public void PressBack()
        {
            gameMenuOverlay.Disappear();
            title.FadeIn(200);
            resetTitle();
            playButton.FadeIn(200);
            quitButton.FadeIn(200);
            user.Value.Save();
            user.Value = null;
        }

        public void EnterSelection()
        {
            title.FadeOut(200);
            gameMenuOverlay.Disappear();
            playButton.FadeOut(200);
            quitButton.FadeOut(200);
            resetTitle();
        }

        public void ExitSelection()
        {
            title.FadeIn(200);
            gameMenuOverlay.Disappear();
            playButton.FadeIn(200);
            quitButton.FadeIn(200);
            resetTitle();
        }

        public void EnterGame()
        {
            title.FadeIn(200);
            playButton.FadeOut(200);
            quitButton.FadeOut(200);

            if (user.Value.StartupAmount == 0)
            {
                background.FadeColour(Colour4.Black, 3000);
                profileButton.Hide();
                Scheduler.AddDelayed(() =>
                {
                    AudioManager.Instance.FadeOutMusic(3000);
                    this.Push(new Tutorial());
                    title.FadeOut();
                }, 5000);
                title.MoveToY(400, 2000, Easing.Out);
                title.ScaleTo(5, 3000, Easing.In);
            }
            else
            {
                Scheduler.AddDelayed(gameMenuOverlay.Appear, 450);
                Scheduler.AddDelayed(
                    delegate
                    {
                        title.MoveToY(100, 200, Easing.In);
                        title.ScaleTo(0.5f, 200, Easing.Out);
                    }, 200
                );
            }

            user.Value.StartupAmount++;
        }

        public override void OnEntering(ScreenTransitionEvent e)
        {
            Logger.Log("Entering Main Menu");
            base.OnEntering(e);
            profileButton.Show();
            gameMenuOverlay.Disappear();
            background.FadeColour(ColourInfo.GradientVertical(
                new Colour4(72, 72, 72, 255),
                new Colour4(58, 58, 58, 255)
            ), 500);
            title.Delay(120).Then().FadeIn(120).MoveToY(300);
            discordRpc.UpdatePresence("Main Menu", "");
        }

        public override void OnResuming(ScreenTransitionEvent e)
        {
            PressBack();
            EnterSelection();
        }

        private void resetTitle()
        {
            title.MoveToY(300, 200, Easing.Out);
            title.ScaleTo(1, 200, Easing.Out);
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            title.FadeIn();
            if (!keepBgm) AudioManager.Instance.ChangeMusic(new GentrysQuestAmbient());
        }
    }
}
