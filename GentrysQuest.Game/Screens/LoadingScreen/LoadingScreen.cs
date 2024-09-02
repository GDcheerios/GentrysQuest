using System.Threading.Tasks;
using GentrysQuest.Game.Database;
using GentrysQuest.Game.Graphics;
using GentrysQuest.Game.Online.API;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Screens;

namespace GentrysQuest.Game.Screens.LoadingScreen
{
    public partial class LoadingScreen : Screen
    {
        private LoadingIndicator indicator;
        private SpriteText status;
        private byte progress = 0;

        public LoadingScreen()
        {
            InternalChildren = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Colour4.Black
                }
            };
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

        private async Task loadGameData()
        {
            status.Text = "Loading game data";
            GameData.Reset();
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
            Scheduler.AddDelayed(() => { this.Push(new MainMenu.MainMenu()); }, 3000);
#else
            Scheduler.AddDelayed(() => { this.Push(new Intro.Intro()); }, 3000);
#endif
        }
    }
}
