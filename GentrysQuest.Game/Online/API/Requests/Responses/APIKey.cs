using System;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace GentrysQuest.Game.Online.API.Requests.Responses
{
    public class APIKey
    {
        [JsonProperty("key_id")]
        public string ID;

        [CanBeNull]
        [JsonProperty("key")]
        public string CombinedKey;

        [CanBeNull]
        [JsonProperty("name")]
        public string Name;

        [JsonProperty("scopes")]
        public string[] Scopes;

        [CanBeNull]
        [JsonProperty("expires_at")]
        public DateTimeOffset? ExpiresAt;

        [CanBeNull]
        [JsonProperty("tokens")]
        public ApiKeyTokensResponse Tokens;

        public string GetHeader() => $"{CombinedKey}";
    }
}
