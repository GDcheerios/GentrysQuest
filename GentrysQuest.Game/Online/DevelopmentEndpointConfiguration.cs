namespace GentrysQuest.Game.Online
{
    public class DevelopmentEndpointConfiguration : EndpointConfiguration
    {
        public DevelopmentEndpointConfiguration()
        {
            ServerUrl = APIEndpointUrl = "http://127.0.0.1";
            GQEndpointUrl = $@"{APIEndpointUrl}/api/gq";
        }
    }
}
