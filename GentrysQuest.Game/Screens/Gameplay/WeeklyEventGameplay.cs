using GentrysQuest.Game.Database;

namespace GentrysQuest.Game.Screens.Gameplay
{
    public partial class WeeklyEventGameplay(int? leaderboardId = null) : Gameplay(leaderboardId)
    {
        public override void End()
        {
            GameData.UnStore();
            base.End();
        }
    }
}
