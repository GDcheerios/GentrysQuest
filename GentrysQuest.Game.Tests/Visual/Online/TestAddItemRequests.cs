using GentrysQuest.Game.Content.Characters;
using GentrysQuest.Game.Content.Families;
using GentrysQuest.Game.Content.Weapons;
using GentrysQuest.Game.Entity;
using GentrysQuest.Game.Entity.Weapon;
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
    public partial class TestAddItemRequests : GentrysQuestTestScene
    {
        [Resolved]
        private Bindable<IUser> user { get; set; }

        private readonly ProfileButton profileButton = new();

        private Character character = new TestCharacter(1);
        private Artifact artifact = new TestArtifact();
        private Weapon weapon = new Sword();

        private LoginResponse loginResponse;
        private APIKey apiKey;

        [BackgroundDependencyLoader]
        private void load() => Add(profileButton);

        [Test]
        public void InitUser()
        {
            var login = new LoginRequest("test", "1234");
            AddStep("Readyness", () => profileButton.Show());
            AddStep("Login", () => login.PerformAsync());
            AddUntilStep("Wait until logged in", () => login.Response != null);
            AddStep("Set response", () => loginResponse = login.Response);
            AddAssert("Check login response", () => login.Response.Success);
            AddStep("Authenticate (set token + ensure API key)", async () =>
            {
                await APIAccess.SetUserToken(login.Response.Token);
                await APIAccess.EnsureApiKeyAsync();
                apiKey = APIAccess.GetApiKey();
            });
            AddUntilStep("Wait until API key obtained", () => apiKey != null);
            AddAssert("API key is not null", () => apiKey != null);
            AddStep("Initialize user", () => user.Value = new OnlineUser(loginResponse.Data));
            AddUntilStep("Wait until user is initialized", () => user.Value != null);
        }

        [Test]
        public void Character()
        {
            AddStep("Add character", () => user.Value.AddItem(character));
            AddWaitStep("Wait for character to be added", 50);
            AddStep("Update star rating", () => character.StarRating.Value = 5);
            AddStep("Update character", () => user.Value.UpdateItem(character));
            AddWaitStep("Wait for character to be updated", 50);
            AddStep("Remove character", () => user.Value.RemoveItem(character));
            AddWaitStep("Wait for character to be removed", 50);
        }

        [Test]
        public void Artifact()
        {
            AddStep("Add artifact", () => user.Value.AddItem(artifact));
            AddWaitStep("Wait for artifact to be added", 50);
            AddStep("Update artifact", () => artifact.StarRating.Value = 4);
            AddStep("Update artifact item", () => user.Value.UpdateItem(artifact));
            AddWaitStep("Wait for artifact to be updated", 50);
            AddStep("Remove artifact", () => user.Value.RemoveItem(artifact));
            AddWaitStep("Wait for artifact to be removed", 50);
        }

        [Test]
        public void Weapon()
        {
            AddStep("Add weapon", () => user.Value.AddItem(weapon));
            AddWaitStep("Wait for weapon to be added", 10);
            AddStep("Update weapon", () => weapon.StarRating.Value = 5);
            AddStep("Update weapon item", () => user.Value.UpdateItem(weapon));
            AddWaitStep("Wait for weapon to be updated", 10);
            AddStep("Remove weapon", () => user.Value.RemoveItem(weapon));
            AddWaitStep("Wait for weapon to be removed", 10);
        }
    }
}
