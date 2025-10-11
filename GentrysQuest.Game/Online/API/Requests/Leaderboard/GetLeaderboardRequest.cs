using System.Collections.Generic;
using GentrysQuest.Game.Scoring;

namespace GentrysQuest.Game.Online.API.Requests.Leaderboard
{
    public class GetLeaderboardRequest(int id) : APIRequest<List<LeaderboardPlacement>>
    {
        public override string Target { get; } = $"api/gq/get-leaderboard/{id}";
    }
}
