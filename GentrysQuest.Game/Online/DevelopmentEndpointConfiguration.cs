namespace GentrysQuest.Game.Online
{
    public class DevelopmentEndpointConfiguration : EndpointConfiguration
    {
        public DevelopmentEndpointConfiguration()
        {
            ServerUrl = APIEndpointUrl = "http://127.0.0.1";
            WebsocketUrl = $@"ws://127.0.0.1:8765";
            GQEndpointUrl = $@"{APIEndpointUrl}/api/gq";
        }
    }
}
