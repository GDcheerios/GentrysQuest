using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GentrysQuest.Game.Online.API.Requests.Leaderboard
{
    public class SubmitScoreRequest : APIRequest<string>
    {
        private readonly int leaderboardId;
        private readonly int userId;
        private readonly int score;
        private readonly string visitation;

        public SubmitScoreRequest(int leaderboardId, int userId, int score, string visitation)
        {
            this.leaderboardId = leaderboardId;
            this.userId = userId;
            this.score = score;
            this.visitation = visitation;
        }

        protected override HttpMethod Method => HttpMethod.Post;
        public override string Target => "api/gq/submit-leaderboard";

        protected override HttpContent CreateContent()
        {
            var payload = new
            {
                leaderboard_id = leaderboardId,
                user = userId,
                score,
                visitation
            };

            return new StringContent(
                JsonSerializer.Serialize(payload),
                Encoding.UTF8,
                "application/json");
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
