using System.Threading.Tasks;
using GentrysQuest.Game.Online.API.Requests.Responses;

namespace GentrysQuest.Game.Online.API.Requests.Leaderboard
{
    public class GetStatisticsRequest : APIRequest<EventStatisticsResponse>
    {
        private readonly int? leaderboardId;
        private readonly int? userId;

        public override string Target
        {
            get
            {
                string target = "api/gq/get-statistics";

                bool hasQuery = false;

                if (leaderboardId.HasValue)
                {
                    target += $"?leaderboard_id={leaderboardId.Value}";
                    hasQuery = true;
                }

                if (userId.HasValue)
                    target += $"{(hasQuery ? "&" : "?")}user_id={userId.Value}";

                return target;
            }
        }

        public GetStatisticsRequest(int? leaderboardId = null, int? userId = null)
        {
            this.leaderboardId = leaderboardId;
            this.userId = userId;
        }

        public new async Task PerformAsync()
        {
            Client.DefaultRequestHeaders.Authorization = await APIAccess.GetApiAuthorizationHeaderAsync();

            try
            {
                await base.PerformAsync();
            }
            finally
            {
                Client.DefaultRequestHeaders.Authorization = null;
            }
        }
    }
}
