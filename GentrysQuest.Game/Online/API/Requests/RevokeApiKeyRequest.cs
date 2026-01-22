using System.Net.Http;
using System.Text;
using GentrysQuest.Game.Online.API.Requests.Responses;
using Newtonsoft.Json;

namespace GentrysQuest.Game.Online.API.Requests
{
    public class RevokeApiKeyRequest(APIKey apiKey) : APIRequest<string>
    {
        private readonly APIKey apiKey = apiKey;

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
