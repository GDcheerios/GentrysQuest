using GentrysQuest.Game.Database;
using GentrysQuest.Game.Overlays.LoginOverlay;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK;

namespace GentrysQuest.Game.Screens.MainMenu
{
    public partial class PlayerSelectBox : CompositeDrawable
    {
        private LoginOverlay loginOverlay;
        private TextFlowContainer loginText;
        private TabButton guestButton;
        private TabButton logInButton;
        private Bindable<bool> isGuestScreen = new(false);
        private GuestSelectionContainer guestSelectionContainer;
        private MainMenuButton playButton;
        private MainMenuButton backButton;

        public PlayerSelectBox(MainMenu mainMenuScreen)
        {
            guestButton = new TabButton("Guest")
            {
                Anchor = Anchor.TopLeft,
                Origin = Anchor.BottomLeft
            };
            guestButton.SetAction(delegate { isGuestScreen.Value = true; });
            logInButton = new TabButton("Log In")
            {
                Anchor = Anchor.TopRight,
                Origin = Anchor.BottomRight,
            };
            logInButton.SetAction(delegate { isGuestScreen.Value = false; });
            playButton = new MainMenuButton("Play")
            {
                Margin = new MarginPadding { Bottom = 10 },
                Scale = new Vector2(1, 0),
                Origin = Anchor.BottomCentre,
                Anchor = Anchor.BottomCentre,
                Size = new Vector2(400, 75)
            };
            backButton = new MainMenuButton("Back")
            {
                Margin = new MarginPadding { Bottom = 10 },
                Scale = new Vector2(1, 1),
                Origin = Anchor.BottomCentre,
                Anchor = Anchor.BottomCentre,
                Size = new Vector2(400, 75)
            };

            guestSelectionContainer = new GuestSelectionContainer
            {
                Alpha = 0,
            };

            GameData.CurrentUser.ValueChanged += delegate
            {
                if (GameData.CurrentUser.Value != null)
                {
                    playButton.ScaleTo(Vector2.One, 200, Easing.OutQuint);
                    playButton.SetText($"Play as {GameData.CurrentUser.Value.Name}");
                    backButton.ScaleTo(new Vector2(1, 0), 200, Easing.OutQuint);
                }
                else
                {
                    playButton.ScaleTo(new Vector2(1, 0), 200, Easing.OutQuint);
                    backButton.ScaleTo(new Vector2(1, 1), 200, Easing.OutQuint);
                }
            };

            isGuestScreen.ValueChanged += delegate
            {
                if (isGuestScreen.Value) openGuestMenu();
                else openLogInMenu();
            };

            playButton.SetAction(mainMenuScreen.EnterSelection);
            backButton.SetAction(mainMenuScreen.PressBack);
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            AddInternal(guestButton);
            AddInternal(logInButton);
            AddInternal(new Container
            {
                RelativeSizeAxes = Axes.Both,
                CornerRadius = 16,
                CornerExponent = 2,
                Masking = true,
                Children =
                [
                    new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = Colour4.Gray
                    },
                    new Container()
                    {
                        Anchor = Anchor.TopCentre,
                        Origin = Anchor.TopCentre,
                        RelativeSizeAxes = Axes.Both,
                        FillMode = FillMode.Fit,
                        Child = loginText = new TextFlowContainer
                        {
                            Text = "Please login with GDcheerios.com account",
                            Anchor = Anchor.TopCentre,
                            Origin = Anchor.TopCentre,
                            Colour = Colour4.Black,
                            Scale = new Vector2(1.5f, 1.5f),
                            TextAnchor = Anchor.TopCentre,
                            RelativeSizeAxes = Axes.Both
                        }
                    },
                    loginOverlay = new LoginOverlay
                    {
                        Scale = new Vector2(0.8f, 0.8f),
                        Origin = Anchor.Centre,
                        Anchor = Anchor.Centre,
                    },
                    guestSelectionContainer,
                    playButton,
                    backButton
                ]
            });
        }

        private void openGuestMenu()
        {
            loginOverlay.FadeOut(300, Easing.OutQuint);
            loginText.FadeOut(300, Easing.OutQuint);
            guestSelectionContainer.FadeIn(300, Easing.OutQuint);
        }

        private void openLogInMenu()
        {
            loginOverlay.FadeIn(300, Easing.OutQuint);
            loginText.FadeIn(300, Easing.OutQuint);
            guestSelectionContainer.FadeOut(300, Easing.OutQuint);
        }
    }
}
