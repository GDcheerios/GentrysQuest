namespace GentrysQuest.Game.Online
{
    public class ProductionEndpointConfiguration : EndpointConfiguration
    {
        public ProductionEndpointConfiguration()
        {
            ServerUrl = APIEndpointUrl = "https://gdcheerios.com";
            WebsocketUrl = $@"wss://gdcheerios.com/ws/";
            GQEndpointUrl = $@"{APIEndpointUrl}/api/gq";
            LeaderboardID = 3;
        }
    }
}
