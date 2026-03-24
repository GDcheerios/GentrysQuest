using System.Collections.Generic;
using System.Threading.Tasks;
using GentrysQuest.Game.Database;
using GentrysQuest.Game.Entity;
using GentrysQuest.Game.Entity.Weapon;
using Newtonsoft.Json;
using osu.Framework.Bindables;

namespace GentrysQuest.Game.Users
{
    public interface IUser
    {
        /// <summary>
        /// The username
        /// </summary>
        [JsonProperty("username")]
        string Name { get; set; }

        /// <summary>
        /// User level
        /// </summary>
        [JsonProperty("experience")]
        Experience Experience { get; set; }

        /// <summary>
        /// How much money the user has
        /// </summary>
        [JsonProperty("money")]
        int Money { get; set; }

        /// <summary>
        /// The global rank (placement) of the user
        /// </summary>
        Bindable<int> Placement { get; set; }

        /// <summary>
        /// The Weighted GP of the user
        /// </summary>
        Bindable<int> WeightedGp { get; set; }

        /// <summary>
        /// The Unweighted GP of the user
        /// </summary>
        Bindable<int> UnweightedGp { get; set; }

        /// <summary>
        /// The rank of the user
        /// </summary>
        Bindable<string> Rank { get; set; }

        /// <summary>
        /// The tier of the user
        /// </summary>
        Bindable<int> Tier { get; set; }

        /// <summary>
        /// Handles money functions
        /// </summary>
        [JsonIgnore]
        Money MoneyHandler { get; set; }

        /// <summary>
        /// The characters
        /// </summary>
        [JsonProperty("characters")]
        List<Character> Characters { get; set; }

        /// <summary>
        /// The artifacts
        /// </summary>
        [JsonProperty("artifacts")]
        List<Artifact> Artifacts { get; set; }

        /// <summary>
        /// The weapons
        /// </summary>
        [JsonProperty("weapons")]
        List<Weapon> Weapons { get; set; }

        /// <summary>
        /// The user's equipped character
        /// </summary>
        [JsonIgnore]
        Character EquippedCharacter { get; set; }

        /// <summary>
        /// Determines whether gameplay is in normal progression or temporary event mode.
        /// Event mode should not persist item changes to online inventory.
        /// </summary>
        [JsonIgnore]
        UserSessionMode SessionMode { get; set; }

        /// <summary>
        /// Loads the user's data
        /// </summary>
        Task Load();

        /// <summary>
        /// Saves the user's data
        /// </summary>
        Task Save();

        /// <summary>
        /// Add an item to this user's data.
        /// </summary>
        /// <param name="entity">The entity to be added</param>
        Task AddItem(EntityBase entity);

        /// <summary>
        /// Update the item. Only functional with online users.
        /// </summary>
        /// <param name="entity">the entity to update</param>
        Task UpdateItem(EntityBase entity);

        /// <summary>
        /// Remove the item from the inventory.
        /// </summary>
        /// <param name="entity">The entity to be removed</param>
        Task RemoveItem(EntityBase entity);

        /// <summary>
        /// Add a statistic to the user's data.
        /// </summary>
        /// <param name="statistic">The statistic</param>
        Task<Statistic> AddStatistic(Statistic statistic);
    }
}
