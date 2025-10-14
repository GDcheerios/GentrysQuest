using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using GentrysQuest.Game.Database;
using GentrysQuest.Game.Entity;
using GentrysQuest.Game.Entity.Weapon;
using GentrysQuest.Game.IO;
using GentrysQuest.Game.Online.API.Requests.Account;
using GentrysQuest.Game.Online.API.Requests.Responses;
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
            if (data == null) return;

            ID = data["id"].Value<int>();
            Name = data["username"].Value<string>();
            UserData = data["gq data"]?.ToObject<UserDataResponse>();
            if (UserData != null && UserData.Metadata == null) create();
            else Load();
        }

        public async Task Load()
        {
            Experience = new Experience();
            Experience.Level.Current.Value = 1;
            Experience.Xp.Current.Value = UserData.Metadata?.Xp ?? 0;
            Experience.Xp.Requirement.Value = 0;

            MoneyHandler = new Money(this)
            {
                Amount =
                {
                    Value = UserData.Metadata?.Money ?? 0
                }
            };

            StartupAmount = UserData.Metadata?.StartAmount ?? 0;

            if (UserData.Ranking != null)
            {
                Placement.Value = UserData.Ranking.Placement;
                WeightedGp.Value = UserData.Ranking.Weighted;
                UnweightedGp.Value = UserData.Ranking.Unweighted;
                Rank.Value = UserData.Ranking.Rank;
                Tier.Value = UserData.Ranking.Tier;
            }

            Characters = new List<Character>();
            Artifacts = new List<Artifact>();
            Weapons = new List<Weapon>();

            if (UserData.Items != null)
            {
                foreach (var item in UserData.Items)
                {
                    if (item == null || item.Type == JTokenType.Null) continue;

                    var type = item["type"]?.Value<string>()?.ToLowerInvariant();

                    switch (type)
                    {
                        case "character":
                            try
                            {
                                Character character = ItemSerializer.Deserialize<Character>(item.ToString());
                                if (character != null) Characters.Add(character);
                            }
                            catch (JsonException ex)
                            {
                                Logger.Log($"Failed to parse character: {ex.Message}", LoggingTarget.Network, LogLevel.Error);
                            }

                            break;

                        case "artifact":
                            try
                            {
                                Artifact artifact = ItemSerializer.Deserialize<Artifact>(item.ToString());
                                if (artifact != null) Artifacts.Add(artifact);
                            }
                            catch (JsonException ex)
                            {
                                Logger.Log($"Failed to parse artifact: {ex.Message}", LoggingTarget.Network, LogLevel.Error);
                            }

                            break;

                        case "weapon":
                            try
                            {
                                Weapon weapon = ItemSerializer.Deserialize<Weapon>(item.ToString());
                                if (weapon != null) Weapons.Add(weapon);
                            }
                            catch (JsonException ex)
                            {
                                Logger.Log($"Failed to parse weapon: {ex.Message}", LoggingTarget.Network, LogLevel.Error);
                            }

                            break;

                        default:
                            Logger.Log($"Unknown item type: '{type ?? "null"}'", LoggingTarget.Network);
                            break;
                    }
                }
            }

            Logger.Log($"Loaded user {Name} for {StartupAmount}", LoggingTarget.Network);
        }

        public async Task Save()
        {
            UserSaveRequest saveRequest = new UserSaveRequest(this);
            await saveRequest.PerformAsync();
        }

        private async Task create()
        {
            await Save();
            await Load();
        }

        public void Delete() => throw new NotImplementedException();

        public async void AddItem(EntityBase entity)
        {
            Notification.Create($"Obtained {entity.StarRating.Value} star {entity.Name}", NotificationType.Obtained);

            AddItemRequests requests = new AddItemRequests(ID, entity);
            await requests.PerformAsync();
            RankingItemResponse response = requests.Response;
            updateRanking(response);

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
            updateRanking(response);
        }

        public async void RemoveItem(EntityBase entity)
        {
            RemoveItemRequest request = new RemoveItemRequest(entity.ID);
            await request.PerformAsync();
            RankingResponse response = request.Response;
            updateRanking(response);

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

        private void updateRanking(RankingResponse ranking)
        {
            Rank.Value = ranking.Rank;
            WeightedGp.Value = ranking.Weighted;
            UnweightedGp.Value = ranking.Unweighted;
            Tier.Value = ranking.Tier;
            Placement.Value = ranking.Placement;
        }

        private void updateRanking(RankingItemResponse ranking) => updateRanking(ranking.Ranking.ToObject<RankingResponse>());
    }
}
