using GentrysQuest.Game.Audio;
using GentrysQuest.Game.Graphics.TextStyles;
using GentrysQuest.Game.Online.API;
using osu.Framework.Allocation;
using osu.Framework.Audio.Track;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Audio;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Screens;
using osuTK;
using osuTK.Graphics;

namespace GentrysQuest.Game.Screens.MainMenu
{
    public partial class MainMenu : Screen
    {
        private Box background;
        private TitleText title;
        private DrawableTrack menuTheme;
        private MainMenuButton playButton;
        private MainMenuButton quitButton;
        private PlayerSelectBox playerSelect;
        private Selection selection;

        [BackgroundDependencyLoader]
        private void load(ITrackStore trackStore)
        {
            playerSelect = new PlayerSelectBox(this)
            {
                Anchor = Anchor.BottomCentre,
                Origin = Anchor.BottomCentre,
                RelativeSizeAxes = Axes.Both,
                RelativePositionAxes = Axes.Both,
                Scale = new Vector2(0.6f, 0.9f),
                Y = -1.05f
            };
            menuTheme = new DrawableTrack(trackStore.Get("Gentrys_Quest_Ambient_1.mp3"));
            InternalChildren = new Drawable[]
            {
                background = new Box
                {
                    Colour = Color4.Black,
                    RelativeSizeAxes = Axes.Both,
                },
                title = new TitleText("Gentry's Quest")
                {
                    Alpha = 0
                },
                selection = new Selection(this),
                new FillFlowContainer
                {
                    AutoSizeAxes = Axes.Y,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Y = 100,
                    Spacing = new Vector2(0, 50),
                    Direction = FillDirection.Vertical,
                    Children = new Drawable[]
                    {
                        playButton = new MainMenuButton("Play")
                        {
                            Size = new Vector2(300, 150),
                            Origin = Anchor.Centre
                        },
                        quitButton = new MainMenuButton("Quit")
                        {
                            Size = new Vector2(300, 150),
                            Origin = Anchor.Centre
                        }
                    }
                },
                playerSelect
            };
            playButton.SetAction(PressPlay);
            quitButton.SetAction(delegate
            {
                _ = APIAccess.DeleteToken();
                Game.Exit();
            });
            selection.FadeOut();
        }

        public void PressPlay()
        {
            title.FadeOut(200);
            selection.Disappear();
            playButton.FadeOut(200);
            quitButton.FadeOut(200);
            playerSelect.MoveToY(-0.05f, 200, Easing.Out);
            resetTitle();
        }

        public void PressBack()
        {
            selection.Disappear();
            title.FadeIn(200);
            resetTitle();
            playButton.FadeIn(200);
            quitButton.FadeIn(200);
            playerSelect.MoveToY(-1.05f, 200, Easing.Out);
        }

        public void EnterSelection()
        {
            title.FadeIn(200);
            Scheduler.AddDelayed(
                delegate
                {
                    title.MoveToY(-400, 200, Easing.In);
                    title.ScaleTo(0.5f, 200, Easing.Out);
                }, 200
            );
            playButton.FadeOut(200);
            quitButton.FadeOut(200);
            playerSelect.MoveToY(-1.05f, 200, Easing.Out);
            Scheduler.AddDelayed(selection.Appear, 450);
        }

        public override void OnEntering(ScreenTransitionEvent e)
        {
            base.OnEntering(e);
            background.FadeColour(ColourInfo.GradientVertical(
                new Colour4(72, 72, 72, 255),
                new Colour4(58, 58, 58, 255)
            ), 500);
            title.Delay(120).Then().FadeIn(120);
        }

        public override void OnResuming(ScreenTransitionEvent e)
        {
            PressBack();
            EnterSelection();
        }

        private void resetTitle()
        {
            title.MoveToY(-200, 200, Easing.Out);
            title.ScaleTo(1, 200, Easing.Out);
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            AudioManager.ChangeMusic(menuTheme);
        }
    }
}
