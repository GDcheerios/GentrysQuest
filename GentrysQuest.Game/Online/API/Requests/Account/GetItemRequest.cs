using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace GentrysQuest.Game.Online.API.Requests.Account
{
    public class GetItemRequest(int id) : APIRequest<List<JToken>>
    {
        public override string Target { get; } = $"api/gq/get-items/{id}";
    }
}
