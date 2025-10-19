using GentrysQuest.Game.Graphics;
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
        private TextFlowContainer placementContainer;
        private TextFlowContainer weightedGpContainer;
        private AnimatedProfileNumber placementNumber = new() { Duration = DELAY };
        private AnimatedProfileNumber weightedGpNumber = new() { Duration = DELAY };
        private int placement = 0;
        private int weightedGp = 0;
        private const int DELAY = 500;

        [Resolved]
        private Bindable<IUser> user { get; set; }

        public ProfileButton() => Depth = -1;

        [BackgroundDependencyLoader]
        private void load()
        {
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
                    placementContainer = new TextFlowContainer
                    {
                        Colour = Colour4.Black,
                        Anchor = Anchor.BottomLeft,
                        Origin = Anchor.BottomLeft,
                        AutoSizeAxes = Axes.X,
                        Height = 32,
                        Margin = new MarginPadding { Left = 10 }
                    },
                    weightedGpContainer = new TextFlowContainer
                    {
                        Colour = Colour4.Black,
                        Anchor = Anchor.BottomRight,
                        Origin = Anchor.BottomRight,
                        AutoSizeAxes = Axes.X,
                        Height = 32,
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
            placementContainer.AddText("#");
            placementContainer.AddText(placementNumber = new AnimatedProfileNumber());
            weightedGpContainer.AddText(weightedGpNumber = new AnimatedProfileNumber());
            weightedGpContainer.AddText("GP");
            user.ValueChanged += changedUser => handleNewUser(changedUser.NewValue);
        }

        private void bindUserStats(IUser u)
        {
            u.Placement.ValueChanged += e => updatePlacement(e.NewValue);
            u.WeightedGp.ValueChanged += e => updateGp(e.NewValue);
        }

        private void updatePlacement(int placement)
        {
            Show();
            Scheduler.AddDelayed(() => placementNumber.SetNumber(placement), DELAY);
            Scheduler.AddDelayed(Hide, DELAY * 3);
        }

        private void updateGp(int gp)
        {
            Show();
            Scheduler.AddDelayed(() => weightedGpNumber.SetNumber(gp), DELAY);
            Scheduler.AddDelayed(Hide, DELAY * 3);
        }

        private void handleNewUser(IUser user)
        {
            if (user == null)
            {
                nameText.Text = "Select User";
                levelText.Text = "";
                experienceBar.Current.Value = 0;
                experienceBar.Max.Value = 1000000;
            }
            else
            {
                selectContainer?.Hide();
                isShowingSelection = false;
                nameText.Text = user.Name;
                bindUserStats(user);
                user.Placement.TriggerChange();
                user.WeightedGp.TriggerChange();
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
