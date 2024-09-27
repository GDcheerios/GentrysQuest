using GentrysQuest.Game.Database;
using Newtonsoft.Json;

namespace GentrysQuest.Game.Users
{
    public class User
    {
        /// <summary>
        /// The username
        /// </summary>
        [JsonProperty("username")]
        public string Name { get; set; }

        /// <summary>
        /// The user id
        /// </summary>
        [JsonProperty("id")]
        public int? ID { get; set; }

        /// <summary>
        /// User level
        /// </summary>
        [JsonProperty("level")]
        public int Level { get; set; } = 1;

        [JsonProperty("statistics")]
        public Statistics Statistics { get; set; }
    }
}
