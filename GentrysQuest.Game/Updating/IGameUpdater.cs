using System.Threading.Tasks;

namespace GentrysQuest.Game.Updating
{
    public interface IGameUpdater
    {
        Task<UpdateCheckResult> CheckForUpdatesAsync();

        void ApplyDownloadedUpdateAndRestart();
    }

    public class UpdateCheckResult
    {
        public static readonly UpdateCheckResult NoUpdate = new UpdateCheckResult(false, null);

        public bool UpdateDownloaded { get; }

        public string Version { get; }

        public UpdateCheckResult(bool updateDownloaded, string version)
        {
            UpdateDownloaded = updateDownloaded;
            Version = version;
        }
    }

    public class NoOpGameUpdater : IGameUpdater
    {
        public Task<UpdateCheckResult> CheckForUpdatesAsync() => Task.FromResult(UpdateCheckResult.NoUpdate);

        public void ApplyDownloadedUpdateAndRestart()
        {
        }
    }
}
