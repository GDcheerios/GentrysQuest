using GentrysQuest.Game.Database;
using GentrysQuest.Game.Overlays.Inventory;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK;

namespace GentrysQuest.Game.Screens.MainMenu
{
    public partial class Selection : CompositeDrawable
    {
        private readonly FillFlowContainer navBar;
        private Bindable<SelectionState> state = new Bindable<SelectionState>(SelectionState.Inventory);
        private MainMenuButton backButton;
        private MainMenuButton weeklyEvent;
        private MainMenuButton travelButton;
        private MainMenuButton inventoryButton;
        private Container focusContainer;
        private InventoryOverlay inventoryOverlay = new InventoryOverlay() { Y = -0.05f };
        private WeeklyEventOverlay weeklyEventOverlay = new WeeklyEventOverlay();

        public Selection(MainMenuScreen parent)
        {
            RelativeSizeAxes = Axes.Both;
            InternalChildren = new Drawable[]
            {
                new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Children = new Drawable[]
                    {
                        navBar = new FillFlowContainer
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            RelativePositionAxes = Axes.X,
                            Height = 1,
                            AutoSizeAxes = Axes.Both,
                            Spacing = new Vector2(45, 0),
                            Y = -300,
                            Children = new Drawable[]
                            {
                                backButton = new MainMenuButton("Back")
                                {
                                    Size = new Vector2(200, 100),
                                    Anchor = Anchor.Centre,
                                    Origin = Anchor.Centre
                                },
                                weeklyEvent = new MainMenuButton("Weekly Event")
                                {
                                    Size = new Vector2(200, 100),
                                    Anchor = Anchor.Centre,
                                    Origin = Anchor.Centre
                                },
                                travelButton = new MainMenuButton("Travel")
                                {
                                    Size = new Vector2(200, 100),
                                    Anchor = Anchor.Centre,
                                    Origin = Anchor.Centre
                                },
                                inventoryButton = new MainMenuButton("Inventory")
                                {
                                    Size = new Vector2(200, 100),
                                    Anchor = Anchor.Centre,
                                    Origin = Anchor.Centre
                                },
                            }
                        },
                        focusContainer = new Container
                        {
                            RelativeSizeAxes = Axes.Both,
                            Size = new Vector2(0.9f, 0.75f),
                            Anchor = Anchor.BottomCentre,
                            Origin = Anchor.BottomCentre,
                            Children = new Drawable[]
                            {
                                inventoryOverlay,
                                weeklyEventOverlay
                            }
                        }
                    }
                }
            };

            backButton.SetAction(parent.PressBack);
            inventoryButton.SetAction(delegate { state.Value = SelectionState.Inventory; });
            weeklyEvent.SetAction(delegate { state.Value = SelectionState.WeeklyEvent; });

            state.ValueChanged += delegate
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
                        weeklyEventOverlay.Show();
                        weeklyEventOverlay.ReloadLeaderboard();
                        break;
                }
            };
        }

        public void Appear()
        {
            state.TriggerChange();
            checkUser();
            this.FadeIn();
            navBar.X = -2;
            navBar.MoveToX(0, 250, Easing.OutQuint);
        }

        public void Disappear()
        {
            inventoryOverlay.Hide();
            weeklyEventOverlay.Hide();
            weeklyEventOverlay.EndLeaderboard();
            navBar.MoveToX(2, 250, Easing.OutQuint);
        }

        private void checkUser()
        {
            if (GameData.UserAvailable() && GameData.IsGuest())
            {
                state.Value = SelectionState.Inventory;
                weeklyEvent.Scale = new Vector2(0);
            }
            else
            {
                weeklyEvent.Scale = new Vector2(1);
            }
        }
    }
}
