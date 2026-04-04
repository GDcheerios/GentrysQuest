using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GentrysQuest.Game.Graphics;
using GentrysQuest.Game.Online.API.Requests.Leaderboard;
using GentrysQuest.Game.Scoring;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Logging;

namespace GentrysQuest.Game.Overlays.Results
{
    public partial class OnlineResultsLeaderboard : ResultsLeaderboard
    {
        private LoadingIndicator loadingIndicator;
        private int loadVersion;

        private static async Task<List<LeaderboardPlacement>> fetchLeaderboard(int id)
        {
            var leaderboardResult = new GetLeaderboardRequest(id);
            await leaderboardResult.PerformAsync();
            return leaderboardResult.Response;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            RelativeSizeAxes = Axes.Both;
            AddInternal(new Container
            {
                Child = loadingIndicator = new LoadingIndicator("Loading leaderboard...")
            });
        }

        public async void Load(int id)
        {
            int requestVersion = ++loadVersion;
            loadingIndicator.FadeIn(100);
            Logger.Log($"Loading leaderboard for {id}", LoggingTarget.Network, LogLevel.Important);
            LeaderboardPanels.Clear();
            Placements.Clear();

            List<LeaderboardPlacement> placements = null;

            try
            {
                placements = await fetchLeaderboard(id);
            }
            catch (System.Exception ex)
            {
                Logger.Log($"Leaderboard fetch failed: {ex.Message}", LoggingTarget.Network, LogLevel.Important);
            }

            if (placements == null)
            {
                Logger.Log("Leaderboard fetch returned no data.", LoggingTarget.Network, LogLevel.Important);
                return;
            }

            if (requestVersion != loadVersion)
                return;

            populate(normalizePlacements(placements));
            loadingIndicator.FadeOut(100);
        }

        private void populate(List<LeaderboardPlacement> placements)
        {
            if (placements == null || placements.Count == 0)
                return;

            foreach (LeaderboardPlacement placement in placements)
            {
                if (placement == null)
                    continue;

                AddListing(placement);
            }
        }

        private static List<LeaderboardPlacement> normalizePlacements(List<LeaderboardPlacement> placements)
        {
            var deduped = placements
                          .Where(p => p != null)
                          .GroupBy(p => p.ID?.ToString() ?? p.Username?.Trim().ToLowerInvariant() ?? string.Empty)
                          .Select(g => g
                                       .OrderByDescending(p => p.Score)
                                       .ThenBy(p => p.Placement)
                                       .First())
                          .OrderByDescending(p => p.Score)
                          .ThenBy(p => p.Placement)
                          .ToList();

            for (int i = 0; i < deduped.Count; i++)
                deduped[i].Placement = i + 1;

            return deduped;
        }
    }
}
