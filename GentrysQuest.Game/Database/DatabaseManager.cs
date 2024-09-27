using System.IO;

namespace GentrysQuest.Game.Database
{
    public class DatabaseManager
    {
        public const string DB_PATH = "GuestData";

        public static void CheckDatabase()
        {
            if (!Directory.Exists("GuestData")) Directory.CreateDirectory("GuestData");
        }
    }
}
