using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using GentrysQuest.Game.Online.API.Requests.Responses;

namespace GentrysQuest.Game.Online.API.Requests.Account;

public class RemoveItemRequest(int id) : APIRequest<RankingResponse>
{
    private int id = id;

    public override string Target { get; } = $"api/gq/remove-item/{id}";
    protected override HttpMethod Method => HttpMethod.Post;

    public new async Task PerformAsync()
    {
        var apiKey = APIAccess.GetApiKey();
        if (apiKey == null) throw new InvalidOperationException("API key missing. Call EnsureApiKeyAsync first.");

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
