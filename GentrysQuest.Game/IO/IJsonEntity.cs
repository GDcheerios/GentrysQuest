using Newtonsoft.Json;

namespace GentrysQuest.Game.IO
{
    public interface IJsonEntity
    {
        /// <summary>
        /// Entity Name
        /// </summary>
        [JsonProperty]
        string Name { get; set; }

        /// <summary>
        /// Entity ID
        /// </summary>
        [JsonProperty]
        int ID { get; set; }

        /// <summary>
        /// Star Rating
        /// </summary>
        [JsonProperty]
        int StarRating { get; set; }

        /// <summary>
        /// Current Level
        /// </summary>
        [JsonProperty]
        int Level { get; set; }

        /// <summary>
        /// Current Xp
        /// </summary>
        [JsonProperty]
        int CurrentXp { get; set; }
    }
}
