using GentrysQuest.Game.Graphics;
using GentrysQuest.Game.Screens;
using GentrysQuest.Game.Users;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;

namespace GentrysQuest.Game.Overlays.Profile
{
    public partial class ProfileButton : MainGqButton
    {
        private bool isVisible = true;
        private bool isShowingSelection = false;

        private ProfilePicture profilePicture;
        private SpriteText nameText;
        private SpriteText levelText;
        private Container experienceContainer;
        private ProgressBar experienceBar;
        private PlayerSelectContainer selectContainer;
        private SpriteText placementText;
        private SpriteText weightedGpText;

        [Resolved]
        private ScreenManager screenManager { get; set; }

        [Resolved]
        private Bindable<IUser> user { get; }

        public ProfileButton()
        {
            Depth = -1;
            Y = -1.05f;
            RelativeSizeAxes = Axes.None;
            Anchor = Anchor.TopRight;
            Origin = Anchor.TopRight;
            Height = 90;
            Width = 270;
            Margin = new MarginPadding { Top = 10, Right = 10 };
            Add(new Container
            {
                Depth = -1,
                RelativeSizeAxes = Axes.Both,
                Children =
                [
                    new FillFlowContainer
                    {
                        RelativeSizeAxes = Axes.X,
                        Height = 60,
                        Direction = FillDirection.Horizontal,
                        Children =
                        [
                            profilePicture = new ProfilePicture(),
                            new Container
                            {
                                RelativeSizeAxes = Axes.Y,
                                Width = 100,
                                Child = nameText = new SpriteText
                                {
                                    Text = "Select User",
                                    Colour = Colour4.Black,
                                    Anchor = Anchor.CentreLeft,
                                    Origin = Anchor.CentreLeft,
                                    Font = FontUsage.Default.With(size: 24)
                                }
                            },
                            new Container
                            {
                                RelativeSizeAxes = Axes.Y,
                                Width = 100,
                                Child = levelText = new SpriteText
                                {
                                    Text = "",
                                    Colour = Colour4.Black,
                                    Anchor = Anchor.CentreRight,
                                    Origin = Anchor.CentreRight,
                                    Font = FontUsage.Default.With(size: 32)
                                }
                            }
                        ]
                    },
                    placementText = new SpriteText
                    {
                        Text = "",
                        Colour = Colour4.Black,
                        Font = FontUsage.Default.With(size: 32),
                        Anchor = Anchor.BottomLeft,
                        Origin = Anchor.BottomLeft,
                        Margin = new MarginPadding { Left = 10 }
                    },
                    weightedGpText = new SpriteText
                    {
                        Text = "",
                        Colour = Colour4.Black,
                        Font = FontUsage.Default.With(size: 32),
                        Anchor = Anchor.BottomRight,
                        Origin = Anchor.BottomRight,
                        Margin = new MarginPadding { Right = 10 }
                    }
                ]
            });
            Add(experienceContainer = new Container
            {
                RelativeSizeAxes = Axes.X,
                Children =
                [
                    experienceBar = new ProgressBar()
                ]
            });
            Add(selectContainer = new PlayerSelectContainer
            {
                Anchor = Anchor.CentreLeft,
                Origin = Anchor.TopRight,
                Margin = new MarginPadding { Right = 20 },
            });
            selectContainer.Hide();

            user.ValueChanged += changedUser => handleNewUser(changedUser.NewValue);
        }

        private void bindUserStats(IUser u)
        {
            placementText.Text = $"#{u.Placement.Value}";
            weightedGpText.Text = $"{u.WeightedGp.Value} GP";

            u.Placement.ValueChanged += e => Schedule(() =>
            {
                Inform();
                placementText.Text = $"#{u.Placement.Value}";
                placementText.FinishTransforms();
                placementText.ScaleTo(1f);
                var placementSequence = placementText.ScaleTo(1.15f, 150, Easing.OutQuint).Then()
                                                     .Delay(250).Then()
                                                     .ScaleTo(1f, 250, Easing.InQuint);
                placementSequence.OnComplete(_ => Hide());
            });

            u.WeightedGp.ValueChanged += e => Schedule(() =>
            {
                Inform();
                weightedGpText.Text = $"{u.WeightedGp.Value} GP";
                weightedGpText.FinishTransforms();
                weightedGpText.ScaleTo(1f);
                var gpSequence = weightedGpText.ScaleTo(1.1f, 120, Easing.OutQuint).Then()
                                               .Delay(200).Then()
                                               .ScaleTo(1f, 220, Easing.InQuint);
                gpSequence.OnComplete(_ => Hide());
            });
        }

        private void unbindUserStats(IUser u)
        {
            if (u == null) return;

            u.Placement.ValueChanged -= null;
            u.Rank.ValueChanged -= null;
            u.Tier.ValueChanged -= null;
            u.WeightedGp.ValueChanged -= null;
            u.UnweightedGp.ValueChanged -= null;
        }

        private void handleNewUser(IUser user)
        {
            MainMenuScreen mainMenuScreen = (MainMenuScreen)screenManager.GetScreen(ScreenState.MainMenu);
            if (screenManager.CurrentScreen != mainMenuScreen) screenManager.SetScreen(ScreenState.MainMenu);

            if (user == null)
            {
                if (mainMenuScreen != null) mainMenuScreen.ExitSelection();
                nameText.Text = "Select User";
                levelText.Text = "";
                experienceBar.Current.Value = 0;
                experienceBar.Max.Value = 1000000;
                placementText.Text = "";
                weightedGpText.Text = "";
            }
            else
            {
                if (mainMenuScreen != null) mainMenuScreen.EnterGame();
                selectContainer?.Hide();
                isShowingSelection = false;
                user.Load();
                nameText.Text = user.Name;
                levelText.Text = user.Experience.CurrentLevel().ToString();
                experienceBar.Current.Value = user.Experience.CurrentXp();
                experienceBar.Max.Value = 1000000;
                bindUserStats(user);
            }
        }

        protected override bool OnClick(ClickEvent e)
        {
            if (selectContainer == null || user.Value != null) return base.OnClick(e);

            if (!isShowingSelection) selectContainer.Show();
            else selectContainer.Hide();
            isShowingSelection = !isShowingSelection;

            return base.OnClick(e);
        }

        public override void Hide()
        {
            isVisible = false;
            this.MoveToY(-200f, 350, Easing.InCirc);
        }

        public override void Show()
        {
            isVisible = true;
            this.MoveToY(0, 350, Easing.OutCirc);
        }

        public void Inform()
        {
            if (!isVisible) Show();
            this.ScaleTo(1.2f, 100, Easing.OutQuint).Then()
                .Delay(500).Then()
                .ScaleTo(1, 200, Easing.InQuint);
        }

        private void signOutUser()
        {
            user.Value?.Save();
            user.Value = null;
        }

        public void GoSelection()
        {
            signOutUser();
            isShowingSelection = true;
            selectContainer.Show();
        }

        public void GoUserSelectionLogIn()
        {
            GoSelection();
            selectContainer.OpenLogInMenu();
        }

        public void GoUserSelectionGuest()
        {
            GoSelection();
            selectContainer.OpenGuestMenu();
        }
    }
}
