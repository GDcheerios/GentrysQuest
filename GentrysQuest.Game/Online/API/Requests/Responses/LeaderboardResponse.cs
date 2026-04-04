using System.Collections.Generic;
using GentrysQuest.Game.Scoring;
using Newtonsoft.Json;

namespace GentrysQuest.Game.Online.API.Requests.Responses
{
    public class LeaderboardResponse
    {
        [JsonProperty("leaderboard")]
        public List<LeaderboardPlacement> Leaderboard { get; set; }

        [JsonProperty("user_placement")]
        public LeaderboardPlacement UserPlacement { get; set; }
    }
}
