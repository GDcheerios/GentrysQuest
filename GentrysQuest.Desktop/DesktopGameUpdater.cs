using System;
using System.Threading.Tasks;
using GentrysQuest.Game.Updating;
using Velopack;
using Velopack.Sources;

namespace GentrysQuest.Desktop
{
    public class DesktopGameUpdater : IGameUpdater
    {
        private const string updateFeedUrlEnvVar = "https://github.com/GDcheerios/GentrysQuest";
        private const string githubTokenEnvVar = "";
        private const string githubPrereleaseEnvVar = "false";

        private VelopackAsset downloadedUpdate;
        private string updateFeedUrl;

        public async Task<UpdateCheckResult> CheckForUpdatesAsync()
        {
            updateFeedUrl = Environment.GetEnvironmentVariable(updateFeedUrlEnvVar);
            if (string.IsNullOrWhiteSpace(updateFeedUrl))
                return UpdateCheckResult.NoUpdate;

            try
            {
                var updateManager = CreateUpdateManager(updateFeedUrl);
                if (!updateManager.IsInstalled)
                    return UpdateCheckResult.NoUpdate;

                var updateInfo = await updateManager.CheckForUpdatesAsync().ConfigureAwait(false);
                if (updateInfo == null)
                    return UpdateCheckResult.NoUpdate;

                await updateManager.DownloadUpdatesAsync(updateInfo).ConfigureAwait(false);
                downloadedUpdate = updateInfo.TargetFullRelease;

                return new UpdateCheckResult(true, downloadedUpdate.Version.ToString());
            }
            catch
            {
                return UpdateCheckResult.NoUpdate;
            }
        }

        public void ApplyDownloadedUpdateAndRestart()
        {
            if (downloadedUpdate == null || string.IsNullOrWhiteSpace(updateFeedUrl))
                return;

            try
            {
                var updateManager = CreateUpdateManager(updateFeedUrl);
                updateManager.ApplyUpdatesAndRestart(downloadedUpdate, Array.Empty<string>());
            }
            catch
            {
            }
        }

        private static UpdateManager CreateUpdateManager(string source)
        {
            if (Uri.TryCreate(source, UriKind.Absolute, out Uri uri) &&
                uri.Host.Contains("github.com", StringComparison.OrdinalIgnoreCase))
            {
                string token = Environment.GetEnvironmentVariable(githubTokenEnvVar) ?? string.Empty;
                bool includePrereleases = bool.TryParse(
                    Environment.GetEnvironmentVariable(githubPrereleaseEnvVar),
                    out bool includePre) && includePre;

                return new UpdateManager(new GithubSource(source, token, includePrereleases));
            }

            return new UpdateManager(source);
        }
    }
}
