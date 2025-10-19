using System.Net.Http;

namespace GentrysQuest.Game.Online.API.Requests.Leaderboard
{
    public class SubmitScoreRequest(int userID, int leaderboardID, long score) : APIRequest<string>
    {
        private int leaderboardID;
        private int userID;
        private long score;

        protected override HttpMethod Method { get; } = HttpMethod.Post;
        public override string Target { get; } = $"api/gq/submit-leaderboard/{leaderboardID}/{userID}+{score}";
    }
}
