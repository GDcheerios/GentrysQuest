using System;
using GentrysQuest.Game.Online.API;
using GentrysQuest.Game.Online.API.Requests.Account;
using GentrysQuest.Game.Overlays.Notifications;
using GentrysQuest.Game.Users;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Logging;

namespace GentrysQuest.Game.Graphics.UserInterface.Login
{
    public partial class LoginContainer : Container
    {
        private Container innerContent;
        private Box background;
        private LoadingIndicator loadingIndicator;
        private GqTextBox usernameInput;
        private GqPasswordBox passwordInput;
        private LoginButton loginButton;

        [Resolved]
        private Bindable<IUser> user { get; set; }

        private const int INPUT_HEIGHT = 40;

        [BackgroundDependencyLoader]
        private void load()
        {
            Height = INPUT_HEIGHT * 6;
            RelativeSizeAxes = Axes.X;
            InternalChildren =
            [
                loadingIndicator = new LoadingIndicator
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Alpha = 0
                },
                innerContent = new Container
                {
                    Masking = true,
                    CornerRadius = 10,
                    CornerExponent = 2,
                    RelativeSizeAxes = Axes.Both,
                    Children =
                    [
                        usernameInput = new GqTextBox
                        {
                            PlaceholderText = "Username",
                            RelativeSizeAxes = Axes.X,
                            LengthLimit = 24,
                            Height = INPUT_HEIGHT
                        },
                        passwordInput = new GqPasswordBox
                        {
                            RelativeSizeAxes = Axes.X,
                            Height = INPUT_HEIGHT,
                            Y = INPUT_HEIGHT * 2
                        },
                        loginButton = new LoginButton
                        {
                            RelativeSizeAxes = Axes.X,
                            Anchor = Anchor.BottomCentre,
                            Origin = Anchor.BottomCentre,
                            Margin = new MarginPadding { Bottom = 10 },
                            Height = INPUT_HEIGHT,
                            Width = 0.5f
                        },
                        background = new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Colour = Colour4.Black,
                            Alpha = 0f
                        }
                    ]
                }
            ];

            loginButton.SetAction(async void () =>
            {
                var loginRequest = new LoginRequest(usernameInput.Text, passwordInput.Text);
                startLoading();

                try
                {
                    await loginRequest.PerformAsync();

                    if (loginRequest.Response != null)
                    {
                        loadingIndicator.ChangeStatus("Loading data");

                        OnlineUser retrievedUser = new OnlineUser(loginRequest.Response.Data!);

                        if (loginRequest.Response.Data!["gq data"] == null)
                        {
                            loadingIndicator.ChangeStatus("Creating data");
                            await retrievedUser.Save();
                        }

                        if (loginRequest.Response.Token != null)
                        {
                            loadingIndicator.ChangeStatus("Authenticating");
                            await APIAccess.SetUserToken(loginRequest.Response.Token);
                            await APIAccess.EnsureApiKeyAsync();
                        }

                        await retrievedUser.Load();

                        user.Value = retrievedUser;
                    }
                    else
                    {
                        Notification.Create("Login failed", NotificationType.Error);
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log($"An error occurred during login {ex.StackTrace}", LoggingTarget.Network);
                }
                finally
                {
                    stopLoading();
                }
            });
        }

        private void startLoading()
        {
            innerContent.FadeTo(0.7f, 250);
            background.FadeTo(0.2f, 250);
            loadingIndicator.FadeIn(250);
        }

        private void stopLoading()
        {
            innerContent.FadeTo(1, 250);
            background.FadeTo(0, 250);
            loadingIndicator.FadeOut(250);
        }
    }
}
