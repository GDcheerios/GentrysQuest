using Newtonsoft.Json;

namespace GentrysQuest.Game.Scoring
{
    public class LeaderboardPlacement
    {
        /// <summary>
        /// The placement
        /// </summary>
        [JsonProperty("placement")]
        public int Placement { get; set; }

        /// <summary>
        /// The score
        /// </summary>
        [JsonProperty("score")]
        public int Score { get; set; }

        /// <summary>
        /// The user's id
        /// </summary>
        [JsonProperty("id")]
        public int? ID { get; set; }

        /// <summary>
        /// Players username
        /// </summary>
        [JsonProperty("username")]

        public string Username { get; set; }

        [JsonProperty("weighted")]
        public int? Weighted { get; set; }

        [JsonProperty("rank")]
        public string Rank { get; set; }

        [JsonProperty("tier")]
        public int? Tier { get; set; }

        [JsonProperty("you")]
        public bool You { get; set; }
    }
}
