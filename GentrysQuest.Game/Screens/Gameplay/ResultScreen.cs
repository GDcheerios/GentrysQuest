using GentrysQuest.Game.Entity.Drawables;
using GentrysQuest.Game.Overlays.Inventory;
using GentrysQuest.Game.Overlays.Results;
using osu.Framework.Graphics;
using osu.Framework.Screens;

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
