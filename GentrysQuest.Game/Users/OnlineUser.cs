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
using GentrysQuest.Game.Overlays.Notifications;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using osu.Framework.Bindables;
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
        public UserDataResponse UserData { get; set; }

        public string Name { get; set; }
        public Experience Experience { get; set; }
        public StatTracker Stats { get; set; }
        public int StartupAmount { get; set; }
        public int Money { get; set; }
        public Bindable<int> Placement { get; set; } = new();
        public Bindable<int> WeightedGp { get; set; } = new();
        public Bindable<int> UnweightedGp { get; set; } = new();
        public Bindable<string> Rank { get; set; } = new();
        public Bindable<int> Tier { get; set; } = new();
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
            UserData = data["gq data"]?.ToObject<UserDataResponse>();
        }

        public async Task Load()
        {
            if (UserData != null)
            {
                Logger.Log(JsonConvert.SerializeObject(UserData));

                // Experience: server currently provides a single xp value under metadata.xp
                Experience = new Experience();
                Experience.Level.Current.Value = 1; // default until server provides level
                Experience.Xp.Current.Value = UserData.Metadata?.Xp ?? 0;
                Experience.Xp.Requirement.Value = 0; // set appropriately if you have a formula

                // Money and startup amount from metadata
                MoneyHandler = new Money(this)
                {
                    Amount =
                    {
                        Value = UserData.Metadata?.Money ?? 0
                    }
                };

                StartupAmount = UserData.Metadata?.StartAmount ?? 0;

                // Ranking (optional bindings)
                if (UserData.Ranking != null)
                {
                    Placement.Value = UserData.Ranking.Placement;
                    WeightedGp.Value = UserData.Ranking.Weighted;
                    UnweightedGp.Value = UserData.Ranking.Unweighted;
                    Rank.Value = UserData.Ranking.Rank;
                    Tier.Value = UserData.Ranking.Tier;
                }

                // Items: may be null
                Characters = new List<Character>();
                if (UserData.Items?.Characters != null)
                {
                    foreach (var character in UserData.Items.Characters)
                        Characters.Add(character.ToObject<Character>());
                }

                Artifacts = new List<Artifact>();
                if (UserData.Items?.Artifacts != null)
                {
                    foreach (var artifact in UserData.Items.Artifacts)
                        Artifacts.Add(artifact.ToObject<Artifact>());
                }

                Weapons = new List<Weapon>();
                if (UserData.Items?.Weapons != null)
                {
                    foreach (var weapon in UserData.Items.Weapons)
                        Weapons.Add(weapon.ToObject<Weapon>());
                }
            }

            Logger.Log($"Loaded user {Name} for {StartupAmount}", LoggingTarget.Network, LogLevel.Debug);
        }

        public async Task Save()
        {
            UserSaveRequest saveRequest = new UserSaveRequest(this);
            UserDataRequest userRequest = new UserDataRequest(ID);
            await saveRequest.PerformAsync();
            await userRequest.PerformAsync();
            UserData = userRequest.Response;
        }

        public void Delete() => throw new NotImplementedException();

        public async void AddItem(EntityBase entity)
        {
            Notification.Create($"Obtained {entity.StarRating.Value} star {entity.Name}", NotificationType.Obtained);

            AddItemRequest request = new AddItemRequest(ID, entity);
            await request.PerformAsync();
            RankingItemResponse response = request.Response;

            switch (entity)
            {
                case Character character:
                    Characters.Add(character);
                    break;

                case Artifact artifact:
                    Artifacts.Add(artifact);
                    break;

                case Weapon weapon:
                    Weapons.Add(weapon);
                    break;
            }
        }

        public async void UpdateItem(EntityBase entity)
        {
            UpdateItemRequest request = new UpdateItemRequest(entity);
            await request.PerformAsync();
            RankingItemResponse response = request.Response;
        }

        public async void RemoveItem(EntityBase entity)
        {
            RemoveItemRequest request = new RemoveItemRequest(entity.ID);
            await request.PerformAsync();
            RankingResponse response = request.Response;

            switch (entity)
            {
                case Character character:
                    Characters.Remove(character);
                    break;

                case Artifact artifact:
                    Artifacts.Remove(artifact);
                    break;

                case Weapon weapon:
                    Weapons.Remove(weapon);
                    break;
            }
        }
    }
}
