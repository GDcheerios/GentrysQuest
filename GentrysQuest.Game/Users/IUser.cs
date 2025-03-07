using System.Collections.Generic;
using GentrysQuest.Game.Database;
using GentrysQuest.Game.Entity;
using GentrysQuest.Game.Entity.Weapon;
using Newtonsoft.Json;

namespace GentrysQuest.Game.Users
{
    public interface IUser
    {
        /// <summary>
        /// The username
        /// </summary>
        [JsonProperty("username")]
        public string Name { get; set; }

        /// <summary>
        /// User level
        /// </summary>
        [JsonProperty("level")]
        public Experience Experience { get; set; }

        /// <summary>
        /// User statistics
        /// </summary>
        // [JsonProperty("statistics")]
        // public StatTracker Stats { get; set; }

        /// <summary>
        /// How many times this user has started up the game
        /// </summary>
        [JsonProperty("startupAmount")]
        public int StartupAmount { get; set; }

        /// <summary>
        /// How much money the user has
        /// </summary>
        [JsonProperty("money")]
        public int Money { get; set; }

        /// <summary>
        /// Handles money functions
        /// </summary>
        [JsonIgnore]
        public Money MoneyHandler { get; set; }

        /// <summary>
        /// The characters
        /// </summary>
        [JsonProperty("characters")]
        public List<Character> Characters { get; set; }

        /// <summary>
        /// The artifacts
        /// </summary>
        [JsonProperty("artifacts")]
        public List<Artifact> Artifacts { get; set; }

        /// <summary>
        /// The weapons
        /// </summary>
        [JsonProperty("weapons")]
        public List<Weapon> Weapons { get; set; }

        /// <summary>
        /// The user's equipped character
        /// </summary>
        [JsonIgnore]
        public Character EquippedCharacter { get; set; }

        /// <summary>
        /// Loads the user's data
        /// </summary>
        public void Load();

        /// <summary>
        /// Saves the user's data
        /// </summary>
        public void Save();

        /// <summary>
        /// Add an item to this user's data
        /// </summary>
        /// <param name="entity"></param>
        public void AddItem(EntityBase entity);
    }
}
