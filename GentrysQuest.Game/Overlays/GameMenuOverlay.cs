using System;
using GentrysQuest.Game.Graphics.TextStyles;
using GentrysQuest.Game.Input;
using GentrysQuest.Game.Online;
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
        public readonly GqMenuButton EventButton;
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
        public readonly EventOverlay EventOverlay = new();

        public readonly GachaContainer GachaContainer = new();

        public bool IsVisible { get; private set; }
        public bool AccessLocked { get; private set; }

        [Resolved]
        private Bindable<IUser> user { get; set; }

        [Resolved]
        private ProfileButton userProfileButton { get; set; }

        [Resolved]
        private InputHandler inputHandler { get; set; }

        [Resolved]
        private TitleText title { get; set; }

        [Resolved]
        private GqWebSocketClient websocket { get; set; }

        [Resolved]
        private ScreenManager screenManager { get; set; }

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
                            Y = 200,
                            Children =
                            [
                                BackButton = new GqMenuButton("Quit"),
                                EventButton = new GqMenuButton("Event"),
                                TravelButton = new GqMenuButton("Travel"),
                                InventoryButton = new GqMenuButton("Inventory"),
                                GachaButton = new GqMenuButton("Gacha"),
                                ProfileButton = new GqMenuButton("Profile")
                            ]
                        },
                        focusContainer = new Container
                        {
                            Name = "Focus Container",
                            RelativeSizeAxes = Axes.Both,
                            Size = new Vector2(0.8f, 0.6f),
                            FillMode = FillMode.Stretch,
                            Y = 280,
                            Anchor = Anchor.TopCentre,
                            Origin = Anchor.TopCentre,
                            Children =
                            [
                                InventoryOverlay,
                                EventOverlay,
                                GachaContainer
                            ]
                        }
                    ]
                }
            ];

            BackButton.SetAction(delegate
            {
                userProfileButton.SignOutUser();
                screenManager.SetScreen(new MainMenuScreen());
            });
            GachaButton.HideButton();
            ProfileButton.HideButton();
            TravelButton.HideButton();
            EventButton.SetAction(delegate { state.Value = SelectionState.Event; });
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
                    if (AccessLocked) return;

                    if (IsVisible)
                        Disappear();
                    else
                        Appear();
                }
            };
            InputEvent gameInventory = new InputEvent
            {
                Name = "Game Inventory",
                Category = "GameOverlay",
                Key = Key.C,
                Action = () =>
                {
                    if (AccessLocked) return;

                    switch (IsVisible)
                    {
                        case true when state.Value != SelectionState.Inventory:
                            state.Value = SelectionState.Inventory;
                            break;

                        case true when state.Value == SelectionState.Inventory:
                            Disappear();
                            break;

                        default:
                            Appear();
                            state.Value = SelectionState.Inventory;
                            break;
                    }
                }
            };

            inputHandler.AddKeyDownEvent(gameOverlayToggle);
            inputHandler.AddKeyDownEvent(gameInventory);
        }

        public void Appear()
        {
            if (AccessLocked) return;
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
            if (user.Value is GuestUser) EventButton.FadeTo(0.5f, 250, Easing.OutQuint);
            else EventButton.FadeTo(1, 250, Easing.OutQuint);
            IsVisible = true;
        }

        public void Disappear()
        {
            InventoryOverlay.Hide();
            EventOverlay.Hide();
            GachaContainer.AnimateHide();
            navBar.MoveToX(1, 150, Easing.In);
            title?.MoveToY(0, 150, Easing.In).FadeOut(150, Easing.In);
            IsVisible = false;
        }

        public void Toggle()
        {
            if (AccessLocked) return;

            if (IsVisible) Disappear();
            else Appear();
            IsVisible = !IsVisible;
        }

        public void SetAccessLocked(bool locked)
        {
            AccessLocked = locked;

            if (locked && IsVisible)
                Disappear();
        }

        private void handleState(ValueChangedEvent<SelectionState> stateChange)
        {
            InventoryOverlay.Hide();
            EventOverlay.Hide();
            GachaContainer.AnimateHide();

            switch (state.Value)
            {
                case SelectionState.Inventory:
                    InventoryOverlay.Show();
                    break;

                case SelectionState.Event:
                    if (user.Value is GuestUser)
                    {
                        Notification.Create("You must be logged in to view the event leaderboard.", NotificationType.Error);
                        state.Value = SelectionState.Inventory;
                        return;
                    }

                    EventOverlay.Show();
                    EventOverlay.UpdateEvent();
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

        public void SetState(SelectionState newState)
        {
            Appear();
            if (state.Value == newState) handleState(new ValueChangedEvent<SelectionState>(state.Value, newState));
            else state.Value = newState;
        }
    }
}
