using Newtonsoft.Json;

namespace GentrysQuest.Game.Online.API.Requests.Responses
{
    public class ApiKeyTokensResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken;

        [JsonProperty("access_expires_in")]
        public int AccessExpiresIn;

        [JsonProperty("refresh_token")]
        public string RefreshToken;

        [JsonProperty("refresh_expires_in")]
        public int RefreshExpiresIn;

        [JsonProperty("key_id")]
        public string KeyId;

        [JsonProperty("scopes")]
        public string[] Scopes;

        [JsonProperty("token_type")]
        public string TokenType;
    }
}
