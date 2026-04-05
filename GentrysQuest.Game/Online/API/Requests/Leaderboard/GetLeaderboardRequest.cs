using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GentrysQuest.Game.Online.API.Requests.Responses;
using GentrysQuest.Game.Scoring;

namespace GentrysQuest.Game.Online.API.Requests.Leaderboard
{
    public class GetLeaderboardRequest : APIRequest<LeaderboardResponse>
    {
        private readonly int id;
        private readonly int amount;
        private readonly int? userId;

        public override string Target => userId.HasValue
            ? $"api/gq/get-leaderboard/{id}?amount={amount}&user_id={userId.Value}"
            : $"api/gq/get-leaderboard/{id}?amount={amount}";

        public new List<LeaderboardPlacement> Response => base.Response?.Leaderboard;
        public LeaderboardPlacement UserPlacement => base.Response?.UserPlacement;

        public GetLeaderboardRequest(int id, int amount = 100, int? userId = null)
        {
            this.id = id;
            this.amount = amount;
            this.userId = userId;
        }

        public new async Task PerformAsync()
        {
            Client.DefaultRequestHeaders.Authorization = await APIAccess.GetApiAuthorizationHeaderAsync();

            try
            {
                await base.PerformAsync();
                annotateCurrentUserPlacement();
            }
            finally
            {
                Client.DefaultRequestHeaders.Authorization = null;
            }
        }

        private void annotateCurrentUserPlacement()
        {
            if (base.Response?.Leaderboard == null || base.Response.Leaderboard.Count == 0)
                return;

            int? resolvedUserId = userId ?? base.Response.UserPlacement?.ID;
            if (!resolvedUserId.HasValue)
                return;

            foreach (var placement in base.Response.Leaderboard)
            {
                if (placement == null)
                    continue;

                placement.You = placement.ID.HasValue && placement.ID.Value == resolvedUserId.Value;
                if (!placement.You)
                    continue;

                if (!string.IsNullOrWhiteSpace(placement.Username) &&
                    !placement.Username.EndsWith(" (you)", StringComparison.OrdinalIgnoreCase))
                    placement.Username += " (you)";
            }
        }
    }
}
