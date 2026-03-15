using System;
using System.Threading.Tasks;
using GentrysQuest.Game.Online.API.Requests;
using GentrysQuest.Game.Online.API.Requests.Responses;

namespace GentrysQuest.Game.Online.API
{
    public class APIAccess
    {
        private static string userToken;
        private static APIKey apiKey;
        public static EndpointConfiguration Endpoint { get; } =
#if DEBUG
            new DevelopmentEndpointConfiguration();
#else
            new ProductionEndpointConfiguration();
#endif

        public static Task SetUserToken(string token) => Task.FromResult(userToken = token);

        public static async Task EnsureApiKeyAsync()
        {
            if (string.IsNullOrEmpty(userToken)) throw new InvalidOperationException("User token not set.");

            var referenceTime = DateTimeOffset.UtcNow;
            var needsNewKey =
                apiKey == null ||
                apiKey.ExpiresAt != null && apiKey.ExpiresAt.Value <= referenceTime;

            if (needsNewKey)
            {
                var req = new GetApiKeyRequest(userToken);
                await req.PerformAsync();
                apiKey = req.Response;
            }
        }

        public static APIKey GetApiKey() => apiKey;

        public static string GetUserToken() => userToken;

        public static void ClearUserSession()
        {
            userToken = null;
            apiKey = null;
        }
    }
}
