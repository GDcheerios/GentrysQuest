using System;
using GentrysQuest.Game.Graphics.TextStyles;
using GentrysQuest.Game.Input;
using GentrysQuest.Game.Overlays.GameMenu;
using GentrysQuest.Game.Overlays.GameMenu.GachaTab;
using GentrysQuest.Game.Overlays.Inventory;
using GentrysQuest.Game.Overlays.Notifications;
using GentrysQuest.Game.Overlays.Profile;
using GentrysQuest.Game.Screens;
using GentrysQuest.Game.Users;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK;
using osuTK.Input;
using InputHandler = GentrysQuest.Game.Input.InputHandler;

namespace GentrysQuest.Game.Overlays
{
    public partial class GameMenuOverlay : CompositeDrawable
    {
        private readonly FillFlowContainer navBar;
        private readonly Bindable<SelectionState> state = new Bindable<SelectionState>(SelectionState.Inventory);

        // buttons or something
        public readonly GqMenuButton BackButton;
        public readonly GqMenuButton WeeklyEvent;
        public readonly GqMenuButton TravelButton;
        public readonly GqMenuButton InventoryButton;
        public readonly GqMenuButton GachaButton;
        public readonly GqMenuButton ProfileButton;

        /// <summary>
        /// The main container for this class to
        /// display information.
        /// </summary>
        private Container focusContainer;

        /// <summary>
        /// The inventory overlay
        /// </summary>
        public readonly InventoryOverlay InventoryOverlay = new();

        /// <summary>
        /// The weekly event overlay
        /// </summary>
        public readonly WeeklyEventOverlay WeeklyEventOverlay = new();

        public readonly GachaContainer GachaContainer = new();

        private bool isVisible;

        [Resolved]
        private Bindable<IUser> user { get; set; }

        [Resolved]
        private ProfileButton userProfileButton { get; set; }

        [Resolved]
        private ScreenManager screenManager { get; set; }

        [Resolved]
        private InputHandler inputHandler { get; set; }

        [Resolved]
        private TitleText title { get; set; }

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
                            Spacing = new Vector2(24, 0),
                            Y = 220,
                            Children =
                            [
                                BackButton = new GqMenuButton("Quit"),
                                WeeklyEvent = new GqMenuButton("Weekly Event"),
                                TravelButton = new GqMenuButton("Travel"),
                                InventoryButton = new GqMenuButton("Inventory"),
                                GachaButton = new GqMenuButton("Gacha"),
                                ProfileButton = new GqMenuButton("Profile")
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
                                InventoryOverlay,
                                WeeklyEventOverlay,
                                GachaContainer
                            ]
                        }
                    ]
                }
            ];

            BackButton.SetAction(delegate
            {
                user.Value.Save();
                user.Value = null;
                screenManager.SetScreen(new MainMenuScreen());
            });
            WeeklyEvent.SetAction(delegate { state.Value = SelectionState.WeeklyEvent; });
            InventoryButton.SetAction(delegate { state.Value = SelectionState.Inventory; });
            GachaButton.SetAction(delegate { state.Value = SelectionState.Gacha; });
            TravelButton.SetAction(delegate { state.Value = SelectionState.Travel; });
            ProfileButton.SetAction(delegate { state.Value = SelectionState.Profile; });

            state.ValueChanged += handleState;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            user.ValueChanged += delegate { InventoryOverlay.ProvideUser(user.Value); };
            InventoryOverlay.ProvideUser(user.Value);
            state.Value = SelectionState.Inventory;

            InputEvent gameOverlayToggle = new InputEvent
            {
                Name = "Game Overlay Toggle",
                Category = "GameOverlay",
                Key = Key.Escape,
                Action = () =>
                {
                    if (isVisible)
                    {
                        Disappear();
                        isVisible = false;
                    }
                    else
                    {
                        Appear();
                        isVisible = true;
                    }
                }
            };
            InputEvent gameInventory = new InputEvent
            {
                Name = "Game Inventory",
                Category = "GameOverlay",
                Key = Key.C,
                Action = () =>
                {
                    switch (isVisible)
                    {
                        case true when state.Value != SelectionState.Inventory:
                            state.Value = SelectionState.Inventory;
                            break;

                        case true when state.Value == SelectionState.Inventory:
                            Disappear();
                            isVisible = false;
                            break;

                        default:
                            Appear();
                            isVisible = true;
                            state.Value = SelectionState.Inventory;
                            break;
                    }
                }
            };

            inputHandler.AddKeyDownEvent(gameOverlayToggle );
            inputHandler.AddKeyDownEvent(gameInventory);
        }

        public void Appear()
        {
            if (user.Value == null) return;

            title.FadeIn();
            title.Scale = Vector2.One;
            title.Origin = Anchor.BottomCentre;
            title.Anchor = Anchor.TopCentre;
            title.Y = 0;
            title.MoveToY(150, 150, Easing.Out);
            state.TriggerChange();
            this.FadeIn();
            navBar.X = -2;
            navBar.MoveToX(0, 150, Easing.Out);
            if (user.Value is GuestUser) WeeklyEvent.FadeTo(0.5f, 250, Easing.OutQuint);
            else WeeklyEvent.FadeTo(1, 250, Easing.OutQuint);
        }

        public void Disappear()
        {
            InventoryOverlay.Hide();
            WeeklyEventOverlay.Hide();
            WeeklyEventOverlay.EndLeaderboard();
            GachaContainer.AnimateHide();
            navBar.MoveToX(1, 150, Easing.In);
            title?.MoveToY(0, 150, Easing.In).FadeOut(150, Easing.In);
        }

        public void Toggle()
        {
            if (isVisible) Disappear();
            else Appear();
            isVisible = !isVisible;
        }

        private void handleState(ValueChangedEvent<SelectionState> stateChange)
        {
            InventoryOverlay.Hide();
            WeeklyEventOverlay.Hide();
            WeeklyEventOverlay.EndLeaderboard();
            GachaContainer.AnimateHide();

            switch (state.Value)
            {
                case SelectionState.Inventory:
                    InventoryOverlay.Show();
                    break;

                case SelectionState.WeeklyEvent:
                    if (user.Value is GuestUser)
                    {
                        Notification.Create("You must be logged in to view the weekly event leaderboard.", NotificationType.Error);
                        state.Value = SelectionState.Inventory;
                        return;
                    }

                    WeeklyEventOverlay.Show();
                    WeeklyEventOverlay.ReloadLeaderboard();
                    break;

                case SelectionState.Travel:
                    break;

                case SelectionState.Profile:
                    break;

                case SelectionState.Gacha:
                    GachaContainer.AnimateShow();
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
