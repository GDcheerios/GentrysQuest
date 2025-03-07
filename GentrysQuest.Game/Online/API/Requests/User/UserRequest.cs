using GentrysQuest.Game.Users;

namespace GentrysQuest.Game.Online.API.Requests.User
{
    public class UserRequest(string idUsername) : APIRequest<OnlineUser>
    {
        private string idUsername = idUsername;

        public override string Target { get; } = $@"/accounts/grab/{idUsername}";
    }
}
