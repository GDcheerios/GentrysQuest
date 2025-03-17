using System;
using GentrysQuest.Game.Overlays.GameMenu;
using GentrysQuest.Game.Overlays.Inventory;
using GentrysQuest.Game.Overlays.Notifications;
using GentrysQuest.Game.Overlays.Profile;
using GentrysQuest.Game.Screens;
using GentrysQuest.Game.Screens.MainMenu;
using GentrysQuest.Game.Users;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK;

namespace GentrysQuest.Game.Overlays
{
    public partial class GameMenuOverlay : CompositeDrawable
    {
        private readonly FillFlowContainer navBar;
        private readonly Bindable<SelectionState> state = new Bindable<SelectionState>(SelectionState.Inventory);

        // buttons or something
        private readonly GqMenuButton backButton;
        private readonly GqMenuButton weeklyEvent;
        private readonly GqMenuButton travelButton;
        private readonly GqMenuButton inventoryButton;
        private readonly GqMenuButton profileButton;

        /// <summary>
        /// The main container for this class to
        /// display information.
        /// </summary>
        private Container focusContainer;

        /// <summary>
        /// The inventory overlay
        /// </summary>
        private InventoryOverlay inventoryOverlay;

        /// <summary>
        /// The weekly event overlay
        /// </summary>
        private readonly WeeklyEventOverlay weeklyEventOverlay = new WeeklyEventOverlay();

        private bool isVisible;

        [Resolved]
        private Bindable<IUser> user { get; set; }

        [Resolved]
        private ProfileButton userProfileButton { get; set; }

        [Resolved]
        private ScreenManager screenManager { get; set; }

        /// <summary>
        /// A variable to check if the user has pressed the weekly event already.
        /// </summary>
        private bool hasPressedWeeklyAsGuest;

        public GameMenuOverlay()
        {
            Depth = -10000;
            RelativeSizeAxes = Axes.Both;
            InternalChildren =
            [
                new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Children =
                    [
                        navBar = new FillFlowContainer
                        {
                            Anchor = Anchor.TopCentre,
                            Origin = Anchor.TopCentre,
                            RelativePositionAxes = Axes.X,
                            Height = 1,
                            AutoSizeAxes = Axes.Both,
                            Spacing = new Vector2(45, 0),
                            Y = 200,
                            Children =
                            [
                                backButton = new GqMenuButton("Quit"),
                                weeklyEvent = new GqMenuButton("Weekly Event"),
                                travelButton = new GqMenuButton("Travel"),
                                inventoryButton = new GqMenuButton("Inventory"),
                                profileButton = new GqMenuButton("Profile")
                            ]
                        },
                        focusContainer = new Container
                        {
                            RelativeSizeAxes = Axes.Both,
                            Size = new Vector2(0.9f, 0.75f),
                            Y = 300,
                            Anchor = Anchor.TopCentre,
                            Origin = Anchor.TopCentre,
                            Children =
                            [
                                weeklyEventOverlay
                            ]
                        }
                    ]
                }
            ];

            weeklyEvent.SetAction(delegate { state.Value = SelectionState.WeeklyEvent; });
            inventoryButton.SetAction(delegate { state.Value = SelectionState.Inventory; });
            travelButton.SetAction(delegate { state.Value = SelectionState.Travel; });
            profileButton.SetAction(delegate { state.Value = SelectionState.Profile; });

            state.ValueChanged += handleState;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            user.ValueChanged += delegate { createNewInventory(); };
            inventoryOverlay = new InventoryOverlay(user.Value);
            focusContainer.Add(inventoryOverlay);
        }

        private void createNewInventory()
        {
            inventoryOverlay = new InventoryOverlay(user.Value);
        }

        public void Appear()
        {
            if (user.Value == null) return;

            state.TriggerChange();
            this.FadeIn();
            navBar.X = -2;
            navBar.MoveToX(0, 150, Easing.Out);
            if (user.Value is GuestUser) weeklyEvent.FadeTo(0.5f, 250, Easing.OutQuint);
        }

        public void Disappear()
        {
            inventoryOverlay.Hide();
            weeklyEventOverlay.Hide();
            weeklyEventOverlay.EndLeaderboard();
            navBar.MoveToX(1, 150, Easing.In);
        }

        public void Toggle()
        {
            if (isVisible) Disappear();
            else Appear();
            isVisible = !isVisible;
        }

        private void handleState(ValueChangedEvent<SelectionState> stateChange)
        {
            inventoryOverlay.Hide();
            weeklyEventOverlay.Hide();
            weeklyEventOverlay.EndLeaderboard();

            switch (state.Value)
            {
                case SelectionState.Inventory:
                    inventoryOverlay.Show();
                    break;

                case SelectionState.WeeklyEvent:
                    if (user.Value is GuestUser)
                    {
                        if (hasPressedWeeklyAsGuest)
                        {
                            userProfileButton.GoUserSelectionLogIn();
                            hasPressedWeeklyAsGuest = false;
                            Disappear();
                            return;
                        }

                        Notification.Create("You must be logged in to view the weekly event leaderboard.", NotificationType.Error);
                        state.Value = SelectionState.Inventory;
                        hasPressedWeeklyAsGuest = true;
                        return;
                    }

                    weeklyEventOverlay.Show();
                    weeklyEventOverlay.ReloadLeaderboard();
                    break;

                case SelectionState.Travel:
                    break;

                case SelectionState.Profile:
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
