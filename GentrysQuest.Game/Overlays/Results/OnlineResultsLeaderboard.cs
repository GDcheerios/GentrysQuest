using System.Collections.Generic;
using System.Threading.Tasks;
using GentrysQuest.Game.Online.API.Requests;
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

        protected override async void LoadComplete()
        {
            base.LoadComplete();
            var placements = await fetchLeaderboard(id);

            foreach (LeaderboardPlacement placement in placements)
            {
                AddListing(placement);
            }
        }
    }
}
