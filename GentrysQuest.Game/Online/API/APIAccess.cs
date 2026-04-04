using System;
using System.Threading;
using System.Threading.Tasks;
using GentrysQuest.Game.Online.API.Requests;
using GentrysQuest.Game.Online.API.Requests.Responses;

namespace GentrysQuest.Game.Online.API
{
    public class APIAccess
    {
        private static string userToken;
        private static APIKey apiKey;
        private static readonly SemaphoreSlim apiKeyRefreshGate = new(1, 1);
        public static EndpointConfiguration Endpoint { get; } =
#if DEBUG
            new DevelopmentEndpointConfiguration();
#else
            new ProductionEndpointConfiguration();
#endif

        public static Task SetUserToken(string token) => Task.FromResult(userToken = token);

        public static async Task EnsureApiKeyAsync(bool forceRefresh = false)
        {
            if (string.IsNullOrEmpty(userToken)) throw new InvalidOperationException("User token not set.");

            await apiKeyRefreshGate.WaitAsync();
            try
            {
                var referenceTime = DateTimeOffset.UtcNow;
                var needsNewKey =
                    forceRefresh ||
                    apiKey == null ||
                    apiKey.ExpiresAt != null && apiKey.ExpiresAt.Value <= referenceTime;

                if (!needsNewKey)
                    return;

                var req = new GetApiKeyRequest(userToken);
                await req.PerformAsync();

                if (req.Response == null)
                    throw new InvalidOperationException("Failed to retrieve API key.");

                apiKey = req.Response;
            }
            finally
            {
                apiKeyRefreshGate.Release();
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
