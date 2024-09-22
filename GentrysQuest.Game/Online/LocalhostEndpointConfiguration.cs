namespace GentrysQuest.Game.Online
{
    public class LocalhostEndpointConfiguration : EndpointConfiguration
    {
        public LocalhostEndpointConfiguration()
        {
            ServerUrl = APIEndpointUrl = "http://localhost";
            GQEndpointUrl = $@"{APIEndpointUrl}/api/gq";
        }
    }
}
