using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GentrysQuest.Game.Online.API.Requests.Responses
{
    public record LoginResponse
    {
        public bool Success { get; set; }

        [CanBeNull]
        public string Error { get; set; }

        [CanBeNull]
        public JToken Data { get; set; }

        private string token;

        [CanBeNull]
        [JsonProperty("token")]
        public string Token
        {
            get => token;
            set => token = value;
        }

        [JsonProperty("access_token")]
        private string AccessToken
        {
            set => token = value;
        }
    }
}
