using System;
using System.Threading.Tasks;
using GentrysQuest.Game.Online.API.Requests;

namespace GentrysQuest.Game.Online.API
{
    public class APIAccess
    {
        private static string userToken;
        private static string apiKey;
        private static DateTimeOffset? apiKeyExpiresAt;
        public static EndpointConfiguration Endpoint { get; } =
#if DEBUG
            new LocalhostEndpointConfiguration();
#else
            new ProductionEndpointConfiguration();
#endif

        public APIAccess()
        {
            // Keep empty or remove the constructor entirely if not needed.
        }

        public static Task SetUserToken(string token) => Task.FromResult(userToken = token);

        public static async Task EnsureApiKeyAsync()
        {
            if (string.IsNullOrEmpty(userToken)) throw new InvalidOperationException("User token not set.");

            var needsNewKey =
                string.IsNullOrEmpty(apiKey) ||
                apiKeyExpiresAt == null ||
                DateTimeOffset.UtcNow >= apiKeyExpiresAt.Value.AddSeconds(-30);

            if (needsNewKey)
            {
                var req = new GetApiKeyRequest(userToken);
                await req.PerformAsync();
                apiKey = req.ApiKey;
                apiKeyExpiresAt = req.ExpiresAt;
            }
        }

        public static string GetApiKey() => apiKey;

        public static string GetUserToken() => userToken;

        public static async Task RevokeApiKeyAsync()
        {
            if (!string.IsNullOrEmpty(apiKey))
            {
                await new RevokeApiKeyRequest(apiKey).PerformAsync();
                apiKey = null;
                apiKeyExpiresAt = null;
            }
        }

        public static void ClearUserSession()
        {
            userToken = null;
            apiKey = null;
            apiKeyExpiresAt = null;
        }
    }
}
