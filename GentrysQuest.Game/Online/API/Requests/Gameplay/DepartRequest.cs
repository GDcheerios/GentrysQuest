using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace GentrysQuest.Game.Online.API.Requests.Gameplay;

public class DepartRequest : APIRequest<string>
{
    private readonly string id;

    public DepartRequest(string id) => this.id = id;

    protected override HttpMethod Method => HttpMethod.Post;
    public override string Target => $"api/gq/depart/{id}";

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
