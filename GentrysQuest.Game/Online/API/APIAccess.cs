using System.Threading.Tasks;
using GentrysQuest.Game.Online.API.Requests;

namespace GentrysQuest.Game.Online.API
{
    public class APIAccess
    {
        private static string token;
        public static EndpointConfiguration Endpoint;

        public APIAccess()
        {
#if DEBUG
            Endpoint = new DevelopmentEndpointConfiguration();
            // Endpoint = new LocalhostEndpointConfiguration();
#else
            Endpoint = new ProductionEndpointConfiguration();
#endif
        }

        public static async Task GrabToken()
        {
            if (token == null)
            {
                var tokenRequest = new GetTokenRequest();
                await tokenRequest.PerformAsync();
                token = tokenRequest.Response;
            }
        }

        public static async Task DeleteToken()
        {
            if (token != null) await new DeleteTokenRequest(token).PerformAsync();
        }
    }
}
