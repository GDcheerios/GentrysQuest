namespace GentrysQuest.Game.Online
{
    public class ProductionEndpointConfiguration : EndpointConfiguration
    {
        public ProductionEndpointConfiguration()
        {
            ServerUrl = APIEndpointUrl = "https://gdcheerios.com";
            WebsocketUrl = $@"wss://ws.gdcheerios.com:8765";
            GQEndpointUrl = $@"{APIEndpointUrl}/api/gq";
            LeaderboardID = 3;
        }
    }
}
