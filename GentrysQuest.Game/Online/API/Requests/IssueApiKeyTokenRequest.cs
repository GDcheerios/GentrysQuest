using System;
using System.Net.Http;
using GentrysQuest.Game.Online.API.Requests.Responses;

namespace GentrysQuest.Game.Online.API.Requests
{
    public class IssueApiKeyTokenRequest(string combinedApiKey) : APIRequest<ApiKeyTokensResponse>
    {
        private readonly string combinedApiKey = combinedApiKey ?? throw new ArgumentNullException(nameof(combinedApiKey));

        public override string Target => "auth/keys/token";

        protected override HttpMethod Method => HttpMethod.Post;

        public new async System.Threading.Tasks.Task PerformAsync()
        {
            Client.DefaultRequestHeaders.Remove("X-API-Key");
            Client.DefaultRequestHeaders.Add("X-API-Key", combinedApiKey);

            try
            {
                await base.PerformAsync();
            }
            finally
            {
                Client.DefaultRequestHeaders.Remove("X-API-Key");
            }
        }
    }
}
