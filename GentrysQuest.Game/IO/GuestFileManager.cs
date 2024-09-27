using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using GentrysQuest.Game.Database;
using GentrysQuest.Game.Users;

namespace GentrysQuest.Game.IO
{
    public static class GuestFileManager
    {
        public static readonly JsonSerializerOptions OPTIONS = new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        public static void SaveUser()
        {
            User user = GameData.CurrentUser.Value;

            // Define the file path using the user's ID
            string filePath = Path.Combine(DatabaseManager.DB_PATH, $"{GameData.CurrentUser.Value.Name}.json");

            // Serialize the user object to JSON
            string json = JsonSerializer.Serialize(GameData.ToJsonGameData(), OPTIONS);

            // Write JSON to the file
            File.WriteAllText(filePath, json);

            Console.WriteLine($"User data saved to {filePath}");
        }

        public static void CreateUser(string name)
        {
            string filePath = Path.Combine(DatabaseManager.DB_PATH, $"{name}.json");
            User user = new User
            {
                Name = name,
                ID = null,
                Level = 1
            };
            GameData.CurrentUser.Value = user;
            string json = JsonSerializer.Serialize(GameData.ToJsonGameData(), OPTIONS);
            File.WriteAllText(filePath, json);
        }

        public static JsonGameData GetGuestData(string name)
        {
            // Define the file path using the user's ID
            string filePath = Path.Combine(DatabaseManager.DB_PATH, $"{name}.json");

            if (File.Exists(filePath))
            {
                // Read the JSON from the file
                string json = File.ReadAllText(filePath);

                // Load game data
                JsonGameData jsonGameData = JsonSerializer.Deserialize<JsonGameData>(json);
                return jsonGameData;
            }

            return null;
        }

        public static List<string> GetGuestNames()
        {
            string[] files = Directory.GetFiles(DatabaseManager.DB_PATH, "*.json");
            List<string> names = new List<string>();
            foreach (string file in files) names.Add(Path.GetFileNameWithoutExtension(file));
            return names;
        }

        public static void LoadData(string name) => GameData.LoadJsonData(GetGuestData(name));
    }
}
