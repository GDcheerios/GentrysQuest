using System;
using System.Net.Http;
using System.Text;
using GentrysQuest.Game.Online.API.Requests.Responses;
using Newtonsoft.Json;

namespace GentrysQuest.Game.Online.API.Requests
{
    public class RefreshApiKeyTokenRequest(string refreshToken) : APIRequest<ApiKeyTokensResponse>
    {
        private readonly string refreshToken = refreshToken ?? throw new ArgumentNullException(nameof(refreshToken));

        public override string Target => "auth/keys/refresh";

        protected override HttpMethod Method => HttpMethod.Post;

        protected override HttpContent CreateContent()
        {
            var payload = new
            {
                refresh_token = refreshToken
            };

            var json = JsonConvert.SerializeObject(payload);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }
    }
}
