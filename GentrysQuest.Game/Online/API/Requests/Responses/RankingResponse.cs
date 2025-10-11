using Newtonsoft.Json;

namespace GentrysQuest.Game.Online.API.Requests.Responses
{
    public class RankingResponse
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("placement")]
        public int Placement { get; set; }

        [JsonProperty("rank")]
        public string Rank { get; set; }

        [JsonProperty("tier")]
        public int Tier { get; set; }

        [JsonProperty("unweighted")]
        public int Unweighted { get; set; }

        [JsonProperty("weighted")]
        public int Weighted { get; set; }
    }
}
