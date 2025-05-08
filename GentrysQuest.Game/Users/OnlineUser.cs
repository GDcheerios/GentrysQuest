using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using GentrysQuest.Game.Database;
using GentrysQuest.Game.Entity;
using GentrysQuest.Game.Entity.Weapon;
using GentrysQuest.Game.Online.API.Requests.Account;
using GentrysQuest.Game.Online.API.Requests.Responses;
using GentrysQuest.Game.Online.API.Requests.User;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using osu.Framework.Logging;

namespace GentrysQuest.Game.Users
{
    public class OnlineUser : IUser
    {
        /// <summary>
        /// The user id
        /// </summary>
        [JsonPropertyName("id")]
        public int ID { get; set; }

        [CanBeNull]
        public UserDataResponse userData { get; set; }

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

        /// <summary>
        /// This is a constructor for an online user
        /// </summary>
        /// <param name="data">json data implemented</param>
        public OnlineUser(JToken data)
        {
            ID = data["id"].Value<int>();
            Name = data["username"].Value<string>();
            userData = data["gq data"].ToObject<UserDataResponse>();
        }

        public async Task Load()
        {
            if (userData != null)
            {
                Logger.Log(JsonConvert.SerializeObject(userData));
                Experience = new Experience();
                Experience.Level.Current.Value = userData.Experience.Level;
                Experience.Xp.Current.Value = userData.Experience.CurrentXp;
                Experience.Xp.Requirement.Value = userData.Experience.RequiredXp;

                MoneyHandler = new Money(this)
                {
                    Amount =
                    {
                        Value = userData.Money
                    }
                };

                StartupAmount = userData.StartAmount;

                Characters = new List<Character>();
                foreach (var character in userData.Items.Characters) Characters.Add(character.ToObject<Character>());

                Artifacts = new List<Artifact>();
                foreach (var artifact in userData.Items.Artifacts) Artifacts.Add(artifact.ToObject<Artifact>());

                Weapons = new List<Weapon>();
                foreach (var weapon in userData.Items.Weapons) Weapons.Add(weapon.ToObject<Weapon>());
            }

            Logger.Log($"Loaded user {Name} for {StartupAmount}", LoggingTarget.Network, LogLevel.Debug);
        }

        public async Task Save()
        {
            UserSaveRequest saveRequest = new UserSaveRequest(this);
            UserDataRequest userRequest = new UserDataRequest(this.ID);
            await saveRequest.PerformAsync();
            await userRequest.PerformAsync();
            userData = userRequest.Response;
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }

        public void AddItem(EntityBase entity)
        {
            throw new NotImplementedException();
        }
    }
}
