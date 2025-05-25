using System.Net.Http;
using System.Text;
using GentrysQuest.Game.Users;
using Newtonsoft.Json;

namespace GentrysQuest.Game.Online.API.Requests.Account
{
    public class UserSaveRequest : APIRequest<string>
    {
        private OnlineUser user;

        public UserSaveRequest(OnlineUser user)
        {
            this.user = user;
            Client.DefaultRequestHeaders.Add("Authorization", APIAccess.GetToken());
        }

        protected override HttpMethod Method { get; } = HttpMethod.Post;

        protected override HttpContent CreateContent() => new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

        public override string Target { get; } = $"gq/save";
    }
}
