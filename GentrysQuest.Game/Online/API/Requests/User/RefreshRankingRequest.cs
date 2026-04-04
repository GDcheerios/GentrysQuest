using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using GentrysQuest.Game.Online.API.Requests.Responses;

namespace GentrysQuest.Game.Online.API.Requests.User;

public class RefreshRankingRequest(int id) : APIRequest<RankingResponse>
{
    public override string Target { get; } = $"api/gq/get-ranking/{id}";

    public new async Task PerformAsync()
    {
        var apiKey = APIAccess.GetApiKey();
        if (apiKey == null)
            throw new InvalidOperationException("API key missing. Call EnsureApiKeyAsync first.");

        Client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(apiKey.GetHeader());

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
