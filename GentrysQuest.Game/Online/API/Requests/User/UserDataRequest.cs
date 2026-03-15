using GentrysQuest.Game.Online.API.Requests.Responses;

namespace GentrysQuest.Game.Online.API.Requests.User
{
    public class UserDataRequest(int id) : APIRequest<UserDataResponse>
    {
        public override string Target { get; } = $"/gq/get/{id}";
    }
}
