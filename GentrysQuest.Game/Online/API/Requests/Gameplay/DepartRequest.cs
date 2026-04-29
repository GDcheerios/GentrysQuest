using System.Net.Http;
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
