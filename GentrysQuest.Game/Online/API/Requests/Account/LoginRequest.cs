using System.Net.Http;
using System.Text;
using GentrysQuest.Game.Online.API.Requests.Responses;
using Newtonsoft.Json;

namespace GentrysQuest.Game.Online.API.Requests.Account
{
    public class LoginRequest : APIRequest<LoginResponse>
    {
        private readonly string username;
        private readonly string password;

        public override string Target { get; } = "account/login-json";
        protected override HttpMethod Method { get; } = HttpMethod.Post;

        public LoginRequest(string username, string password)
        {
            this.username = username;
            this.password = password;
        }

        protected override HttpContent CreateContent()
        {
            var loginData = new { username = username, password = password };
            var json = JsonConvert.SerializeObject(loginData);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }
    }
}
