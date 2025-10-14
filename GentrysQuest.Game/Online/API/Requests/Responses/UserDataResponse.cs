using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GentrysQuest.Game.Online.API.Requests.Responses
{
    // Matches: "gq data": { "items": ..., "metadata": { ... }, "ranking": { ... }, "scores": { } }
    public class UserDataResponse
    {
        [JsonProperty("items")]
        public List<JToken> Items { get; set; }

        // "metadata": { id, money, start_amount, stats, xp }
        [JsonProperty("metadata")]
        public MetadataResponse Metadata { get; set; }

        // "ranking": { id, placement, rank, tier, unweighted, weighted }
        [JsonProperty("ranking")]
        public RankingResponse Ranking { get; set; }
    }

    public class MetadataResponse
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("money")]
        public int Money { get; set; }

        [JsonProperty("start_amount")]
        public int StartAmount { get; set; }

        // xp is a single number in your sample; if it becomes an object later, adjust accordingly
        [JsonProperty("xp")]
        public int Xp { get; set; }

        // stats is null in your sample; model it flexibly
        [JsonProperty("stats")]
        public JObject Stats { get; set; }
    }
}
