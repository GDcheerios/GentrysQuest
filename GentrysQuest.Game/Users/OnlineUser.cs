using System.Collections.Generic;
using GentrysQuest.Game.Database;
using GentrysQuest.Game.Entity;
using GentrysQuest.Game.Entity.Weapon;
using Newtonsoft.Json;

namespace GentrysQuest.Game.Users
{
    public class OnlineUser : IUser
    {
        /// <summary>
        /// The user id
        /// </summary>
        [JsonProperty("id")]
        public int? ID { get; set; }

        public string Name { get; set; }
        public Experience Experience { get; set; }
        public StatTracker Stats { get; set; }
        public int StartupAmount { get; set; }
        public int Money { get; set; }
        public Money MoneyHandler { get; set; }
        public List<Character> Characters { get; set; }
        public List<Artifact> Artifacts { get; set; }
        public List<Weapon> Weapons { get; set; }
        public Character EquippedCharacter { get; set; }

        public void Load()
        {
            throw new System.NotImplementedException();
        }

        public void Save()
        {
            throw new System.NotImplementedException();
        }

        public void Delete()
        {
            throw new System.NotImplementedException();
        }

        public void AddItem(EntityBase entity)
        {
            throw new System.NotImplementedException();
        }
    }
}
