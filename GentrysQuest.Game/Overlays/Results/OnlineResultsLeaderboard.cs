using System.Collections.Generic;
using System.Threading.Tasks;
using GentrysQuest.Game.Online.API.Requests.Leaderboard;
using GentrysQuest.Game.Scoring;

namespace GentrysQuest.Game.Overlays.Results
{
    public partial class OnlineResultsLeaderboard(int id) : ResultsLeaderboard
    {
        private int id = id;

        private static async Task<List<LeaderboardPlacement>> fetchLeaderboard(int id)
        {
            var leaderboardResult = new GetLeaderboardRequest(id);
            await leaderboardResult.PerformAsync();
            return leaderboardResult.Response;
        }

        public async void Load()
        {
            var placements = await fetchLeaderboard(id);
            populate(placements);
        }

        protected override async void LoadComplete()
        {
            base.LoadComplete();
            // Load();
        }

        private void populate(List<LeaderboardPlacement> placements)
        {
            foreach (LeaderboardPlacement placement in placements)
            {
                //TODO: Rewrite
                // if (GameData.CurrentUser.Value != null && placement.Username == GameData.CurrentUser.Value.Name) ScoreText.Text = $"#{placement.Placement}    {placement.Score} score";
                AddListing(placement);
            }
        }
    }
}
