using GentrysQuest.Game.Graphics.UserInterface.Login;
using GentrysQuest.Game.Online.API.Requests.Account;
using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Logging;
using osuTK;

namespace GentrysQuest.Game.Tests.Visual.Overlays
{
    [TestFixture]
    public partial class LoginOverlayTestScene : GentrysQuestTestScene
    {
        private LoginContainer loginContainer;
        private LoginRequest loginRequest;

        public LoginOverlayTestScene()
        {
            Add(new Box
            {
                RelativeSizeAxes = Axes.Both,
                Colour = Colour4.White
            });
            Add(new Container
            {
                Size = new Vector2(500, 500),
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Child = loginContainer = new LoginContainer()
            });
        }

        [Test]
        public void TestRequest()
        {
            AddStep("Create request", () => { loginRequest = new LoginRequest("test", "1234"); });
            AddStep("Perform request", () => { _ = loginRequest.PerformAsync(); });
            AddStep("Log result", () => { Logger.Log(loginRequest.Response.ToString(), LoggingTarget.Information); });
        }
    }
}
