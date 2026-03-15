using GentrysQuest.Game.Graphics;
using GentrysQuest.Game.Graphics.UserInterface.Login;
using GentrysQuest.Game.Overlays.PlayerSelect;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Effects;
using osu.Framework.Graphics.Shapes;
using osuTK;

namespace GentrysQuest.Game.Overlays
{
    public partial class PlayerSelectContainer : CompositeDrawable
    {
        private LoginContainer loginOverlay;
        private TextFlowContainer loginText;
        private readonly PlayerCategoryButton guestButton;
        private readonly PlayerCategoryButton logInButton;
        private readonly Bindable<bool> isGuestScreen = new();
        private readonly GuestSelectionContainer guestSelectionContainer;
        private readonly MainGqButton playButton;

        private const float TOP_MARGIN = 70;

        public PlayerSelectContainer()
        {
            RelativePositionAxes = Axes.Y;

            guestButton = new PlayerCategoryButton("Guest")
            {
                Anchor = Anchor.TopLeft,
                Origin = Anchor.TopLeft
            };
            guestButton.SetAction(OpenGuestMenu);
            logInButton = new PlayerCategoryButton("Log In")
            {
                // Centre it while guest is disabled
                // Anchor = Anchor.TopRight,
                // Origin = Anchor.TopRight,
                Anchor = Anchor.TopCentre,
                Origin = Anchor.TopCentre,
            };
            logInButton.SetAction(OpenLogInMenu);
            playButton = new MainGqButton("play")
            {
                Margin = new MarginPadding { Bottom = 10 },
                Scale = new Vector2(1, 0),
                Origin = Anchor.BottomCentre,
                Anchor = Anchor.BottomCentre,
                Size = new Vector2(400, 75)
            };

            guestSelectionContainer = new GuestSelectionContainer
            {
                Alpha = 0,
                Margin = new MarginPadding { Top = TOP_MARGIN },
            };
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            AddInternal(new Container
            {
                RelativeSizeAxes = Axes.Both,
                CornerRadius = 16,
                CornerExponent = 2,
                Masking = true,
                EdgeEffect = new EdgeEffectParameters
                {
                    Type = EdgeEffectType.Glow,
                    Radius = 16,
                    Colour = new Colour4(0, 0, 0, 0.2f)
                },
                Children =
                [
                    new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = new Colour4(0, 0, 0, 0.2f)
                    },
                    new Container
                    {
                        Margin = new MarginPadding { Top = TOP_MARGIN },
                        Anchor = Anchor.TopCentre,
                        Origin = Anchor.TopCentre,
                        RelativeSizeAxes = Axes.Both,
                        FillMode = FillMode.Fit,
                        Child = loginText = new TextFlowContainer
                        {
                            Text = "Please login with GDcheerios.com account",
                            Anchor = Anchor.TopCentre,
                            Origin = Anchor.TopCentre,
                            Colour = Colour4.White,
                            Scale = new Vector2(1),
                            RelativeSizeAxes = Axes.Both,
                            TextAnchor = Anchor.TopCentre,
                        }
                    },
                    loginOverlay = new LoginContainer
                    {
                        Scale = new Vector2(0.8f, 0.8f),
                        Origin = Anchor.Centre,
                        Anchor = Anchor.Centre,
                    },
                    guestSelectionContainer,
                    playButton
                ]
            });
            // Disable guest selection for now
            // AddInternal(guestButton);
            AddInternal(logInButton);
        }

        public override void Show()
        {
            this.ResizeTo(new Vector2(300, 500), 200, Easing.Out);
            this.FadeIn(200, Easing.Out);
        }

        public override void Hide()
        {
            this.ResizeTo(new Vector2(50, 200), 200, Easing.Out);
            this.FadeOut(200, Easing.Out);
        }

        public void OpenGuestMenu()
        {
            loginOverlay.FadeOut(300, Easing.OutQuint);
            loginText.FadeOut(300, Easing.OutQuint);
            guestSelectionContainer.FadeIn(300, Easing.OutQuint);
        }

        public void OpenLogInMenu()
        {
            loginOverlay.FadeIn(300, Easing.OutQuint);
            loginText.FadeIn(300, Easing.OutQuint);
            guestSelectionContainer.FadeOut(300, Easing.OutQuint);
        }
    }
}
