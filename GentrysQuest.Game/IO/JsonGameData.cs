using System.Collections.Generic;
using GentrysQuest.Game.Database;
using GentrysQuest.Game.Users;
using Newtonsoft.Json;

namespace GentrysQuest.Game.IO
{
    public class JsonGameData
    {
        /// <summary>
        /// The current user
        /// </summary>
        [JsonProperty]
        public User User { get; set; }

        /// <summary>
        /// The money
        /// </summary>
        [JsonProperty]
        public int Money { get; set; }

        /// <summary>
        /// The character data
        /// </summary>
        [JsonProperty]
        public List<JsonCharacter> Characters { get; set; }

        /// <summary>
        /// The weapon data
        /// </summary>
        [JsonProperty]
        public List<JsonWeapon> Weapons { get; set; }

        /// <summary>
        /// The artifact data
        /// </summary>
        [JsonProperty]
        public List<JsonArtifact> Artifacts { get; set; }

        /// <summary>
        /// Statistic data
        /// </summary>
        [JsonProperty]
        public Statistics Statistics { get; set; }
    }
}
