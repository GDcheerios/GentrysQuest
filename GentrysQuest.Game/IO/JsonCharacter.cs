using JetBrains.Annotations;
using Newtonsoft.Json;

namespace GentrysQuest.Game.IO
{
    public class JsonCharacter : IJsonEntity
    {
        public string Name { get; set; }
        public int ID { get; set; }
        public int StarRating { get; set; }
        public int Level { get; set; }
        public int CurrentXp { get; set; }
        public int RequiredXp { get; set; }

        [JsonProperty("$type")]
        public string Type { get; set; }

        /// <summary>
        /// The weapon
        /// </summary>
        [JsonProperty]
        [CanBeNull]
        public JsonWeapon CurrentWeapon { get; set; }

        /// <summary>
        /// Artifacts
        /// </summary>
        [JsonProperty]
        public JsonArtifact[] Artifacts { get; set; }
    }
}
