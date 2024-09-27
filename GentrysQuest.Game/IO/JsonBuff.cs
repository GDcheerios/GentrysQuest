using GentrysQuest.Game.Entity;
using Newtonsoft.Json;

namespace GentrysQuest.Game.IO
{
    public class JsonBuff
    {
        [JsonProperty]
        public StatType BuffID { get; set; }

        [JsonProperty]
        public bool IsPercent { get; set; }

        [JsonProperty]
        public int Level { get; set; }
    }
}
