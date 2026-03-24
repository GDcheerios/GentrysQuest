namespace GentrysQuest.Game.Online
{
    public class EndpointConfiguration
    {
        public string ServerUrl { get; protected set; } = string.Empty;
        public string WebsocketUrl { get; protected set; } = string.Empty;
        public string APIEndpointUrl { get; protected set; } = string.Empty;
        public string GQEndpointUrl { get; protected set; } = string.Empty;
        public int LeaderboardID { get; protected set; } = 1;
    }
}
