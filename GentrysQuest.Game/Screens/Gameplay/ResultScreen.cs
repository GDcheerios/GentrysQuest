using System.Linq;
using GentrysQuest.Game.Database;
using GentrysQuest.Game.Entity.Drawables;
using GentrysQuest.Game.Overlays.Inventory;
using GentrysQuest.Game.Overlays.Results;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Screens;
using osuTK;

namespace GentrysQuest.Game.Screens.Gameplay
{
    public partial class ResultScreen : Screen
    {
        private readonly int id;
        private readonly ResultsLeaderboard leaderboard;
        private readonly StatDrawableContainer statisticsContainer;
        private readonly InventoryButton retryButton;

        public ResultScreen(int id)
        {
            this.id = id;
            AddInternal(new Box
            {
                RelativeSizeAxes = Axes.Both,
                Colour = Colour4.White
            });
            AddInternal(leaderboard = new ResultsLeaderboard
            {
                RelativeSizeAxes = Axes.Both,
                Size = new Vector2(0.45f, 1),
            });
            AddInternal(statisticsContainer = new StatDrawableContainer
            {
                Size = new Vector2(0.45f, 1),
                Anchor = Anchor.TopRight,
                Origin = Anchor.TopRight
            });

            AddInternal(retryButton = new InventoryButton("Retry")
            {
                Anchor = Anchor.BottomLeft,
                Origin = Anchor.BottomLeft,
                Margin = new MarginPadding { Left = 10, Bottom = 10 }
            });

            retryButton.SetAction(delegate
            {
                GameData.WrapUpStats();
                GameData.Reset();
            });

            foreach (var statistic in GameData.CurrentStats.GetStats().Where(statistic => statistic.Name != "Score"))
            {
                statisticsContainer.AddStat(new StatDrawable(statistic.Name, statistic.Value, false));
            }
        }

        public override void OnEntering(ScreenTransitionEvent e)
        {
            this.FadeInFromZero(500, Easing.OutQuint);
        }

        public override void OnSuspending(ScreenTransitionEvent e)
        {
            this.FadeOutFromOne(500, Easing.OutQuint);
        }
    }
}
