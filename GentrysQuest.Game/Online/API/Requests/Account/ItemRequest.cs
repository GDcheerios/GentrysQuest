using GentrysQuest.Game.Online.API.Requests.Responses;

namespace GentrysQuest.Game.Online.API.Requests.Account
{
    public class ItemRequest(int id) : APIRequest<ItemResponse>
    {
        public override string Target { get; } = $"gq/get-items/{id}";
    }
}
