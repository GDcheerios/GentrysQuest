using System;
using System.Collections.Generic;
using System.IO;
using GentrysQuest.Game.Database;
using GentrysQuest.Game.Entity;
using GentrysQuest.Game.Entity.Weapon;
using GentrysQuest.Game.Overlays.Notifications;
using Newtonsoft.Json;

namespace GentrysQuest.Game.Users
{
    public class GuestUser : IUser
    {
        public GuestUser(string name) => Name = name;

        public string Name { get; set; }

        public Experience Experience { get; set; } = new Experience();

        // public StatTracker Stats { get; set; } = new StatTracker();
        public int StartupAmount { get; set; }
        public int Money { get; set; }
        public Money MoneyHandler { get; set; }
        public List<Character> Characters { get; set; } = [];
        public List<Artifact> Artifacts { get; set; } = [];
        public List<Weapon> Weapons { get; set; } = [];
        public Character EquippedCharacter { get; set; } = null;

        public void Load()
        {
            string filePath = Path.Combine(DatabaseManager.PATH, $"{Name}.json");
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Guest data file '{filePath}' not found.");

            try
            {
                string jsonData = File.ReadAllText(filePath);
                JsonConvert.PopulateObject(jsonData, this);
                MoneyHandler = new Money(this);
            }
            catch (JsonException ex)
            {
                throw new InvalidOperationException($"Error loading guest user data for '{Name}'.", ex);
            }
        }

        public void Save()
        {
            string filePath = Path.Combine(DatabaseManager.PATH, $"{Name}.json");

            try
            {
                string jsonData = JsonConvert.SerializeObject(this, Formatting.Indented);
                File.WriteAllText(filePath, jsonData);
            }
            catch (IOException ex)
            {
                throw new InvalidOperationException($"Error saving guest user data for '{Name}'.", ex);
            }
        }

        public void AddItem(EntityBase entity)
        {
            Notification.Create($"Obtained {entity.StarRating.Value} star {entity.Name}", NotificationType.Obtained);

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

        public void Delete()
        {
            string filePath = Path.Combine(DatabaseManager.PATH, $"{Name}.json");

            try
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);
            }
            catch (IOException ex)
            {
                throw new InvalidOperationException($"Error deleting guest user data for '{Name}'.", ex);
            }
        }

        public static GuestUser Create(string name)
        {
            string filePath = Path.Combine(DatabaseManager.PATH, $"{name}.json");

            if (File.Exists(filePath))
            {
                Notification.Create($"Guest user '{name}' already exists.", NotificationType.Error);
                return new(name);
            }

            GuestUser newUser = new(name);
            newUser.Save();

            return newUser;
        }
    }
}
