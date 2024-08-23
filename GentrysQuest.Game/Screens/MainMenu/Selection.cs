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
        private Bindable<SelectionState> state = new Bindable<SelectionState>(SelectionState.WeeklyChallenge);
        private MainMenuButton weeklyChallenge;
        private MainMenuButton travelButton;
        private MainMenuButton inventoryButton;
        private Container focusContainer;
        private InventoryOverlay inventoryOverlay = new InventoryOverlay();
        private WeeklyChallengeOverlay weeklyChallengeOverlay = new WeeklyChallengeOverlay();

        public Selection()
        {
            RelativeSizeAxes = Axes.Both;
            InternalChildren = new Drawable[]
            {
                new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    FillMode = FillMode.Fit,
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
                                weeklyChallenge = new MainMenuButton("Weekly Challenge")
                                {
                                    Size = new Vector2(200, 100),
                                    Anchor = Anchor.Centre,
                                    Origin = Anchor.Centre
                                },
                                // travelButton = new MainMenuButton("Travel")
                                // {
                                //     Size = new Vector2(200, 100),
                                //     Anchor = Anchor.Centre,
                                //     Origin = Anchor.Centre
                                // },
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
                            Size = new Vector2(1000, 800),
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Y = 100,
                            Children = new Drawable[]
                            {
                                inventoryOverlay,
                                weeklyChallengeOverlay
                            }
                        }
                    }
                }
            };

            inventoryButton.SetAction(delegate { state.Value = SelectionState.Inventory; });
            weeklyChallenge.SetAction(delegate { state.Value = SelectionState.WeeklyChallenge; });

            state.ValueChanged += delegate
            {
                inventoryOverlay.Hide();

                switch (state.Value)
                {
                    case SelectionState.Inventory:
                        inventoryOverlay.Show();
                        break;

                    case SelectionState.WeeklyChallenge:
                        weeklyChallengeOverlay.Show();
                        break;
                }
            };
        }

        public void Appear()
        {
            state.Value = SelectionState.WeeklyChallenge;
            this.FadeIn();
            navBar.X = -2;
            navBar.MoveToX(0, 250, Easing.OutQuint);
        }

        public void Disappear()
        {
            inventoryOverlay.Hide();
            navBar.MoveToX(2, 250, Easing.OutQuint);
        }
    }
}
