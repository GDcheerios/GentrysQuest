using Android.Content.PM;
using GentrysQuest.Game;
using osu.Framework.Android;

namespace GentrysQuest.Android
{
    [Activity(MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : AndroidGameActivity
    {
        protected override osu.Framework.Game CreateGame() => new GentrysQuestGame(false);
    }
}
