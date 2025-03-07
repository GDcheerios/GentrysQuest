using System.Net.Http;
using System.Text;
using GentrysQuest.Game.Users;
using Newtonsoft.Json;

namespace GentrysQuest.Game.Online.API.Requests.Account
{
    public class LoginRequest(string username, string password) : APIRequest<OnlineUser>
    {
        public override string Target { get; } = "account/login-json";

        protected override HttpMethod Method { get; } = HttpMethod.Post;

        protected override HttpContent CreateContent()
        {
            var loginData = new { username, password };
            string json = JsonConvert.SerializeObject(loginData);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }
    }
}
