using Newtonsoft.Json.Linq;

namespace GentrysQuest.Game.Online.API.Requests.Responses
{
    public record RankingItemResponse
    {
        public JObject Ranking { get; init; }
        public JObject Item { get; init; }
    }
}
