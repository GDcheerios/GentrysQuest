using System;
using System.Threading.Tasks;
using GentrysQuest.Game.Online.API.Requests.Responses;

namespace GentrysQuest.Game.Online.API.Requests.User;

public class RefreshRankingRequest(int id) : APIRequest<RankingResponse>
{
    public override string Target { get; } = $"api/gq/get-ranking/{id}";

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
