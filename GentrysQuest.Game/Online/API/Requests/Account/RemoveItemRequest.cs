using System;
using System.Net.Http;
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
        Client.DefaultRequestHeaders.Authorization = await APIAccess.GetApiAuthorizationHeaderAsync();

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
