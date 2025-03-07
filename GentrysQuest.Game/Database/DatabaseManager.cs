using System.IO;

namespace GentrysQuest.Game.Database
{
    public class DatabaseManager
    {
        public const string PATH = "GuestData";

        public static void CheckDatabase()
        {
            if (!Directory.Exists("GuestData")) Directory.CreateDirectory("GuestData");
        }

        public static void DeleteDatabase() => Directory.Delete("GuestData", true);
    }
}
