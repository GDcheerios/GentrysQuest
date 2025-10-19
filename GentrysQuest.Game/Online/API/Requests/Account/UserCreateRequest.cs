using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using GentrysQuest.Game.Users;
using Newtonsoft.Json;

namespace GentrysQuest.Game.Online.API.Requests.Account
{
    public class UserCreateRequest(OnlineUser user) : APIRequest<string>
    {
        public override string Target { get; } = $"api/gq/create/";

        protected override HttpMethod Method { get; } = HttpMethod.Post;

        protected override HttpContent CreateContent() => new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

        public new async Task PerformAsync()
        {
            var apiKey = APIAccess.GetApiKey();
            if (apiKey == null)
                throw new InvalidOperationException("API key missing. Call EnsureApiKeyAsync first.");

            Client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Authorization", apiKey.GetHeader());

            try
            {
                await base.PerformAsync();
            }
            finally
            {
                Client.DefaultRequestHeaders.Authorization = null;
            }
        }
    }
}
