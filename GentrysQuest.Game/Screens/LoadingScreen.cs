using System.Threading.Tasks;
using GentrysQuest.Game.Content;
using GentrysQuest.Game.Graphics;
using GentrysQuest.Game.Online;
using GentrysQuest.Game.Online.API;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Screens;

namespace GentrysQuest.Game.Screens
{
    public partial class LoadingScreen : GqScreen
    {
        private LoadingIndicator indicator;
        private SpriteText status;
        private IntroScreen introScreenScreen;
        private byte progress = 0;

        [Resolved]
        private ScreenManager screenManager { get; set; }

        [Resolved]
        private DiscordRpc discordRpc { get; set; }

        /// <summary>
        /// This is the main loading screen for the game.
        /// We also pass down variables that the game will need
        /// </summary>
        /// <param name="state"></param>
        /// <param name="profileButton"></param>
        /// <param name="introScreenScreen"></param>
        public LoadingScreen(IntroScreen introScreenScreen)
        {
            this.introScreenScreen = introScreenScreen;
            InternalChildren =
            [
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Colour4.Black
                }
            ];
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            AddInternal(indicator = new LoadingIndicator
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre
            });
            AddInternal(status = new SpriteText
            {
                Text = "Checking for updates",
                Anchor = Anchor.BottomCentre,
                Origin = Anchor.BottomCentre,
                Margin = new MarginPadding { Bottom = 50 },
                Font = FontUsage.Default.With(size: 72)
            });
        }

        public override void OnEntering(ScreenTransitionEvent e)
        {
            base.OnEntering(e);
            discordRpc.UpdatePresence("Loading", "");
        }

        private async Task loadGameData()
        {
            status.Text = "Loading game data";
            ContentManager.LoadContent();
            await Task.Delay(500);
        }

        private async Task setupAPIAccess()
        {
            status.Text = "Connecting to server";
            _ = new APIAccess(); // need to set up API access
            await APIAccess.GrabToken();
        }

        protected override async void LoadComplete()
        {
            base.LoadComplete();
            await setupAPIAccess();
            await loadGameData();

            Scheduler.AddDelayed(() =>
            {
                indicator.FadeOut(300, Easing.InOutCirc);
                status.FadeOut(300);
            }, 1000);
            Scheduler.AddDelayed(() =>
            {
                status.FadeIn(300);
                status.Text = "Get Ready!";
                status.Margin = new MarginPadding();
                status.Anchor = Anchor.Centre;
                status.Origin = Anchor.Centre;
            }, 1500);
            Scheduler.AddDelayed(() => status.FadeOut(250), 2700);
#if DEBUG
            Scheduler.AddDelayed(() => { screenManager.SetScreen(ScreenState.MainMenu); }, 3000);
#else
            Scheduler.AddDelayed(() => { screenManager.SetScreen(ScreenState.Intro); }, 3000);
#endif
        }
    }
}
