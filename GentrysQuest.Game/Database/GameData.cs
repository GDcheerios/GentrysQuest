using System.Collections.Generic;
using GentrysQuest.Game.Content;
using GentrysQuest.Game.Entity;
using GentrysQuest.Game.Entity.Weapon;
using GentrysQuest.Game.IO;
using GentrysQuest.Game.Overlays.Notifications;
using GentrysQuest.Game.Users;
using osu.Framework.Bindables;
using osu.Framework.Logging;

namespace GentrysQuest.Game.Database
{
    public static class GameData
    {
        /// <summary>
        /// The game content.
        /// </summary>
        public static ContentManager Content = new ContentManager();

        /// <summary>
        /// How many times the game has been started by this user
        /// </summary>
        public static int StartupAmount = 0;

        /// <summary>
        /// The equipped character
        /// </summary>
        public static Character EquippedCharacter { get; private set; }

        /// <summary>
        /// Doubloons
        /// </summary>
        public static Money Money { get; private set; }

        /// <summary>
        /// Game stats
        /// </summary>
        public static Statistics Statistics { get; private set; }

        public static StatTracker CurrentStats { get; private set; }

        /// <summary>
        /// The current user that's being used in the game
        /// </summary>
        public static Bindable<User> CurrentUser { get; private set; } = new();

        // The lists of entities
        public static List<Character> Characters { get; private set; }
        public static List<Artifact> Artifacts { get; private set; }
        public static List<Weapon> Weapons { get; private set; }

        // caching old data
        public static List<Character> CachedCharacters { get; private set; }
        public static List<Artifact> CachedArtifacts { get; private set; }
        public static List<Weapon> CachedWeapons { get; private set; }

        /// <summary>
        /// Store current items for later.
        /// useful for cases like weekly events when we have to use a new load out.
        /// </summary>
        public static void Store()
        {
            CachedCharacters = Characters;
            Characters = new List<Character>();
            CachedArtifacts = Artifacts;
            Artifacts = new List<Artifact>();
            CachedWeapons = Weapons;
            Weapons = new List<Weapon>();
        }

        public static void UnStore()
        {
            Characters = CachedCharacters;
            Artifacts = CachedArtifacts;
            Weapons = CachedWeapons;
        }

        public static void Reset()
        {
            EquippedCharacter = null;
            Money = new Money();
            Statistics = new Statistics();
            CurrentStats = new StatTracker();
            Characters = new List<Character>();
            Artifacts = new List<Artifact>();
            Weapons = new List<Weapon>();
        }

        public static bool UserAvailable() => CurrentUser.Value != null;

        public static bool IsGuest() => CurrentUser.Value.ID == null;

        public static void LoadJsonData(JsonGameData jsonGameData)
        {
            // Load main data
            CurrentUser.Value = jsonGameData.User;
            Money.Hand(jsonGameData.Money);

            // Load entity data
            foreach (JsonCharacter jsonCharacter in jsonGameData.Characters) Characters.Add(LoadCharacterJson(jsonCharacter));
            foreach (JsonArtifact jsonArtifact in jsonGameData.Artifacts) Artifacts.Add(LoadArtifactJson(jsonArtifact));
            foreach (JsonWeapon jsonWeapon in jsonGameData.Weapons) Weapons.Add(LoadWeaponJson(jsonWeapon));
        }

        public static Character LoadCharacterJson(JsonCharacter jsonCharacter)
        {
            Logger.Log($"Loading character {jsonCharacter.Name}");
            Character character = Content.GetCharacter(jsonCharacter.Name);
            character.LoadJson(jsonCharacter);
            return character;
        }

        public static Artifact LoadArtifactJson(JsonArtifact jsonArtifact)
        {
            Logger.Log($"Loading artifact {jsonArtifact.Name}");
            Artifact artifact = Content.GetFamily(jsonArtifact.FamilyName).GetArtifact(jsonArtifact.Name);
            artifact.LoadJson(jsonArtifact);
            return artifact;
        }

        public static Weapon LoadWeaponJson(JsonWeapon jsonWeapon)
        {
            Logger.Log($"Loading weapon {jsonWeapon.Name}");
            Weapon weapon = Content.GetWeapon(jsonWeapon.Name);
            weapon.LoadJson(jsonWeapon);
            return weapon;
        }

        public static void StartStatTracker() => CurrentStats = new StatTracker();

        public static void WrapUpStats()
        {
            Statistics.Totals.Merge(CurrentStats);
            Statistics.Most = Statistics.Most.GetBest(CurrentStats);
        }

        /// <summary>
        /// Calls the AddNotification method.
        /// If you don't want a notification don't call this method.
        /// </summary>
        /// <param name="entity">entity to add</param>
        public static void Add(EntityBase entity)
        {
            NotificationContainer.Instance.AddNotification(new Notification($"Obtained {entity.StarRating.Value} star {entity.Name}", NotificationType.Obtained));

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

        public static void EquipCharacter(Character character)
        {
            NotificationContainer.Instance.AddNotification(new Notification($"Equipped {character.Name}", NotificationType.Informative));
            EquippedCharacter = character;
        }

        public static User GetUser() => CurrentUser.Value;

        public static JsonGameData ToJsonGameData()
        {
            JsonGameData jsonGameData = new JsonGameData
            {
                User = GetUser(),
                Money = Money.Amount.Value,
                Statistics = Statistics,
            };

            List<JsonCharacter> characters = new List<JsonCharacter>();
            List<JsonArtifact> artifacts = new List<JsonArtifact>();
            List<JsonWeapon> weapons = new List<JsonWeapon>();

            foreach (Character character in Characters) characters.Add(character.ToJson());
            foreach (Artifact artifact in Artifacts) artifacts.Add(artifact.ToJson());
            foreach (Weapon weapon in Weapons) weapons.Add(weapon.ToJson());

            jsonGameData.Characters = characters;
            jsonGameData.Artifacts = artifacts;
            jsonGameData.Weapons = weapons;

            return jsonGameData;
        }
    }
}
