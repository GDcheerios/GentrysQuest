using System;
using System.Net;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using GentrysQuest.Game.Online.API.Requests;
using GentrysQuest.Game.Online.API.Requests.Responses;
using osu.Framework.Logging;

namespace GentrysQuest.Game.Online.API
{
    public class APIAccess
    {
        private static string userToken;
        private static APIKey apiKey;
        private static string apiAccessToken;
        private static DateTimeOffset? apiAccessTokenExpiresAt;
        private static string apiRefreshToken;
        private static bool sessionExpired;
        private static readonly SemaphoreSlim apiKeyRefreshGate = new(1, 1);
        private static readonly SemaphoreSlim apiTokenRefreshGate = new(1, 1);
        public static event Action SessionExpired;
        public static EndpointConfiguration Endpoint { get; } =
#if DEBUG
            new DevelopmentEndpointConfiguration();
#else
            new ProductionEndpointConfiguration();
#endif

        public static bool IsSessionExpired => sessionExpired;

        public static Task SetUserToken(string token)
        {
            userToken = token;
            apiKey = null;
            apiAccessToken = null;
            apiAccessTokenExpiresAt = null;
            apiRefreshToken = null;
            sessionExpired = false;
            return Task.CompletedTask;
        }

        public static async Task EnsureApiKeyAsync(bool forceRefresh = false)
        {
            if (sessionExpired)
                throw new SessionExpiredException("Session expired. Please log in again.");

            if (string.IsNullOrEmpty(userToken)) throw new InvalidOperationException("User token not set.");

            await apiKeyRefreshGate.WaitAsync();
            try
            {
                if (sessionExpired)
                    throw new SessionExpiredException("Session expired. Please log in again.");

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
                {
                    if (req.LastStatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        markSessionExpired("Bearer token unauthorized while requesting API key.");
                        throw new SessionExpiredException("Session expired. Please log in again.");
                    }

                    throw new InvalidOperationException("Failed to retrieve API key.");
                }

                apiKey = req.Response;
                if (req.Response.Tokens != null)
                    setApiTokens(req.Response.Tokens);
            }
            finally
            {
                apiKeyRefreshGate.Release();
            }
        }

        public static async Task EnsureApiAccessTokenAsync(bool forceRefresh = false)
        {
            if (sessionExpired)
                throw new SessionExpiredException("Session expired. Please log in again.");

            await apiTokenRefreshGate.WaitAsync();
            try
            {
                if (!forceRefresh && hasUsableApiAccessToken())
                    return;

                if (!string.IsNullOrWhiteSpace(apiRefreshToken))
                {
                    var refreshRequest = new RefreshApiKeyTokenRequest(apiRefreshToken);
                    await refreshRequest.PerformAsync();

                    if (refreshRequest.Response != null)
                    {
                        setApiTokens(refreshRequest.Response);
                        return;
                    }

                    if (refreshRequest.LastStatusCode == HttpStatusCode.Unauthorized)
                        apiRefreshToken = null;
                }

                if (!string.IsNullOrWhiteSpace(apiKey?.CombinedKey))
                {
                    var tokenRequest = new IssueApiKeyTokenRequest(apiKey.CombinedKey);
                    await tokenRequest.PerformAsync();

                    if (tokenRequest.Response != null)
                    {
                        setApiTokens(tokenRequest.Response);
                        return;
                    }
                }

                await EnsureApiKeyAsync(forceRefresh: true);
                if (hasUsableApiAccessToken())
                    return;

                if (!string.IsNullOrWhiteSpace(apiKey?.CombinedKey))
                {
                    var tokenRequest = new IssueApiKeyTokenRequest(apiKey.CombinedKey);
                    await tokenRequest.PerformAsync();
                    if (tokenRequest.Response != null)
                    {
                        setApiTokens(tokenRequest.Response);
                        return;
                    }
                }

                markSessionExpired("Unable to obtain API access token.");
                throw new SessionExpiredException("Session expired. Please log in again.");
            }
            finally
            {
                apiTokenRefreshGate.Release();
            }
        }

        public static async Task<AuthenticationHeaderValue> GetApiAuthorizationHeaderAsync(bool forceRefresh = false)
        {
            await EnsureApiAccessTokenAsync(forceRefresh);

            if (string.IsNullOrWhiteSpace(apiAccessToken))
                throw new SessionExpiredException("Session expired. Please log in again.");

            return new AuthenticationHeaderValue("Bearer", apiAccessToken);
        }

        public static APIKey GetApiKey() => apiKey;

        public static string GetUserToken() => userToken;

        public static void ClearUserSession()
        {
            userToken = null;
            apiKey = null;
            apiAccessToken = null;
            apiAccessTokenExpiresAt = null;
            apiRefreshToken = null;
            sessionExpired = false;
        }

        private static void markSessionExpired(string reason)
        {
            if (sessionExpired)
                return;

            sessionExpired = true;
            userToken = null;
            apiKey = null;
            apiAccessToken = null;
            apiAccessTokenExpiresAt = null;
            apiRefreshToken = null;
            Logger.Log($"Session expired: {reason}", LoggingTarget.Network, LogLevel.Important);
            SessionExpired?.Invoke();
        }

        private static bool hasUsableApiAccessToken()
        {
            if (string.IsNullOrWhiteSpace(apiAccessToken))
                return false;

            var expiresAt = apiAccessTokenExpiresAt ?? DateTimeOffset.MinValue;
            return expiresAt > DateTimeOffset.UtcNow;
        }

        private static void setApiTokens(ApiKeyTokensResponse tokens)
        {
            if (tokens == null)
                return;

            if (!string.IsNullOrWhiteSpace(tokens.AccessToken))
            {
                apiAccessToken = tokens.AccessToken;
                var skewSeconds = 30;
                var ttl = Math.Max(0, tokens.AccessExpiresIn - skewSeconds);
                apiAccessTokenExpiresAt = DateTimeOffset.UtcNow.AddSeconds(ttl);
            }

            if (!string.IsNullOrWhiteSpace(tokens.RefreshToken))
                apiRefreshToken = tokens.RefreshToken;
        }
    }
}
