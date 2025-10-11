using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace GentrysQuest.Game.Online.API.Requests
{
    public class RevokeApiKeyRequest(string apiKey) : APIRequest<string>
    {
        private readonly string apiKey = string.IsNullOrWhiteSpace(apiKey) ? null : apiKey;

        public override string Target => "auth/keys";

        protected override HttpMethod Method => HttpMethod.Delete;

        protected override HttpContent CreateContent()
        {
            var payload = new { apiKey };
            var json = JsonConvert.SerializeObject(payload);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }
    }
}
