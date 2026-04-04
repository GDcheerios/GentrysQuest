using System.Collections.Generic;
using Newtonsoft.Json;

namespace GentrysQuest.Game.Online.API.Requests.Responses
{
    public class EventStatisticsResponse
    {
        [JsonProperty("leaderboard_id")]
        public int? LeaderboardId { get; set; }

        [JsonProperty("leaderboard")]
        public LeaderboardStatisticsResponse Leaderboard { get; set; }

        [JsonProperty("user")]
        public UserStatisticsResponse User { get; set; }
    }

    public class LeaderboardStatisticsResponse
    {
        [JsonProperty("total_players")]
        public long TotalPlayers { get; set; }

        [JsonProperty("total_plays")]
        public long? TotalPlays { get; set; }

        [JsonProperty("total_score")]
        public long TotalScore { get; set; }

        [JsonProperty("average_score")]
        public double? AverageScore { get; set; }

        [JsonProperty("statistics")]
        public StatisticsSummaryResponse Statistics { get; set; }
    }

    public class UserStatisticsResponse
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("scores")]
        public UserScoreSummaryResponse Scores { get; set; }

        [JsonProperty("statistics")]
        public StatisticsSummaryResponse Statistics { get; set; }

        [JsonProperty("last_run")]
        public LastRunResponse LastRun { get; set; }
    }

    public class UserScoreSummaryResponse
    {
        [JsonProperty("total_plays")]
        public long? TotalPlays { get; set; }

        [JsonProperty("total_score")]
        public long? TotalScore { get; set; }

        [JsonProperty("average_score")]
        public double? AverageScore { get; set; }
    }

    public class LastRunResponse
    {
        [JsonProperty("visitation")]
        public string Visitation { get; set; }

        [JsonProperty("score")]
        public long? Score { get; set; }

        [JsonProperty("statistics")]
        public StatisticsSummaryResponse Statistics { get; set; }
    }

    public class StatisticsSummaryResponse
    {
        [JsonProperty("total_amount")]
        public long TotalAmount { get; set; }

        [JsonProperty("average_amount")]
        public double? AverageAmount { get; set; }

        [JsonProperty("by_type")]
        public List<StatisticTypeSummaryResponse> ByType { get; set; } = [];
    }

    public class StatisticTypeSummaryResponse
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("total_amount")]
        public long TotalAmount { get; set; }

        [JsonProperty("average_amount")]
        public double? AverageAmount { get; set; }
    }
}
