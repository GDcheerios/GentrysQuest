using System.Threading.Tasks;
using GentrysQuest.Game.Online.API;
using GentrysQuest.Game.Online.API.Requests.Account;
using GentrysQuest.Game.Online.API.Requests.Responses;
using GentrysQuest.Game.Overlays.Profile;
using GentrysQuest.Game.Users;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Bindables;

namespace GentrysQuest.Game.Tests.Visual.Online
{
    [TestFixture]
    public partial class TestAddItemRequest : GentrysQuestTestScene
    {
        [Resolved]
        private Bindable<IUser> user { get; }

        private readonly ProfileButton profileButton = new();

        private LoginResponse loginResponse;
        private APIKey apiKey;

        [BackgroundDependencyLoader]
        private void load() => Add(profileButton);

        [Test]
        public async Task Login_and_fetch_api_key_with_test_credentials()
        {
            var login = new LoginRequest("test", "1234");
            await login.PerformAsync();
            loginResponse = login.Response;

            Assert.IsNotNull(login.Response, "Login response was null.");
            Assert.IsTrue(login.Response.Success, $"Login failed: {login.Response.Error}");
            Assert.IsFalse(string.IsNullOrWhiteSpace(login.Response.Token), "Token was not returned.");

            await APIAccess.SetUserToken(login.Response.Token);
            await APIAccess.EnsureApiKeyAsync();

            apiKey = APIAccess.GetApiKey();
            Assert.IsFalse(apiKey == null, "API key was not obtained.");
        }

        [Test]
        public async Task InitUser()
        {
            user.Value = new OnlineUser(loginResponse.Data);
        }

        [Test]
        public async Task AddItem()
        {
            user.Value.AddItem();
        }
    }
}
