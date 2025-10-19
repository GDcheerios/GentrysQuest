namespace GentrysQuest.Game.Online
{
    public class LocalhostEndpointConfiguration : EndpointConfiguration
    {
        public LocalhostEndpointConfiguration()
        {
            ServerUrl = APIEndpointUrl = "http://127.0.0.1";
            GQEndpointUrl = $@"{APIEndpointUrl}/api/gq";
        }
    }
}
