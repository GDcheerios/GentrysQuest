using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GentrysQuest.Game.Online.API.Requests.Responses
{
    public record RankingItemResponse
    {
        [JsonProperty("ranking")]
        public JObject Ranking { get; init; }

        [JsonProperty("item")]
        public JObject Item { get; init; }
    }
}
