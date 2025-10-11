using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using osu.Framework.Logging; // for quick tracing

namespace GentrysQuest.Game.Online.API.Requests
{
    public class GetApiKeyRequest(string userToken) : APIRequest<string>
    {
        private readonly string _userToken = userToken ?? throw new ArgumentNullException(nameof(userToken));

        public override string Target => "auth/keys";

        protected override HttpMethod Method => HttpMethod.Post;

        protected override HttpContent CreateContent()
        {
            var payload = new
            {
                name = "GentrysQuest",
                expires_at = DateTimeOffset.UtcNow.AddHours(1).ToString("O"),
                scopes = new[]
                {
                    "leaderboard:read",
                    "leaderboard:write",
                    "account:read",
                    "account:write"
                }
            };
            var json = JsonConvert.SerializeObject(payload);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        public new async System.Threading.Tasks.Task PerformAsync()
        {
            Logger.Log("GetApiKeyRequest: starting", LoggingTarget.Network);

            if (Client == null)
                throw new InvalidOperationException("HttpClient not initialized.");

            var token = (_userToken ?? string.Empty)
                        .Replace("\r", string.Empty)
                        .Replace("\n", string.Empty)
                        .Trim();

            if (token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                token = token.Substring("Bearer ".Length).Trim();

            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            try
            {
                await base.PerformAsync();
                Logger.Log($"GetApiKeyRequest: response len={(Response?.Length ?? 0)}", LoggingTarget.Network);
            }
            finally
            {
                Client.DefaultRequestHeaders.Authorization = null;
            }
        }

        public string ApiKey => Response;
        public DateTimeOffset? ExpiresAt { get; private set; }
    }
}
