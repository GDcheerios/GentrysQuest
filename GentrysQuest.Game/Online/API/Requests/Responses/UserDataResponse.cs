using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GentrysQuest.Game.Online.API.Requests.Responses
{
    public class UserDataResponse
    {
        [JsonProperty("experience")]
        public ExperienceResponse Experience { get; set; }

        [JsonProperty("items")]
        public ItemResponse Items { get; set; }

        [JsonProperty("money")]
        public int Money { get; set; }

        [JsonProperty("start_amount")]
        public int StartAmount { get; set; }
    }

    public class ExperienceResponse
    {
        [JsonProperty("current xp")]
        public int CurrentXp { get; set; }

        [JsonProperty("level")]
        public int Level { get; set; }

        [JsonProperty("required xp")]
        public int RequiredXp { get; set; }
    }

    public class ItemResponse
    {
        [JsonProperty("characters")]
        public List<JToken> Characters { get; set; }

        [JsonProperty("artifacts")]
        public List<JToken> Artifacts { get; set; }

        [JsonProperty("weapons")]
        public List<JToken> Weapons { get; set; }
    }
}
