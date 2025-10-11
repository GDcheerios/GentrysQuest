using GentrysQuest.Game.Online.API.Requests.Responses;

namespace GentrysQuest.Game.Online.API.Requests.Account
{
    public class GetItemRequest(int id) : APIRequest<ItemResponse>
    {
        public override string Target { get; } = $"api/gq/get-items/{id}";
    }
}
