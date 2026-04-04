using System.Collections.Generic;
using System.IO;
using GentrysQuest.Game.Database;

namespace GentrysQuest.Game.IO
{
    public static class GuestFileManager
    {
        public static List<string> GetGuestNames()
        {
            string[] files = Directory.GetFiles(DatabaseManager.PATH, "*.json");
            List<string> names = new List<string>();
            foreach (string file in files) names.Add(Path.GetFileNameWithoutExtension(file));
            return names;
        }
    }
}
