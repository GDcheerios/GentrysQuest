using System;
using System.Collections.Generic;
using GentrysQuest.Game.Entity;
using GentrysQuest.Game.Entity.Weapon;
using GentrysQuest.Game.Gachas;
using GentrysQuest.Game.Graphics;
using GentrysQuest.Game.Graphics.UserInterface;
using GentrysQuest.Game.Users;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Logging;
using osuTK;

namespace GentrysQuest.Game.Overlays.GameMenu.GachaTab
{
    public partial class GachaContainer : Container
    {
        private const int full_size_pull_threshold = 7;
        private const float min_pull_scale = 0.35f;

        [Resolved]
        private Bindable<IUser> user { get; set; }

        private Container leftContainer;
        private Container rightContainer;
        private FillFlowContainer<GachaRollContainer> pullContainer;
        private Container resultsOverlay;
        private FillFlowContainer resultsList;
        private GqText resultsTitle;
        private MainGqButton resultsContinueButton;

        private readonly Container characterContainer = new Container
        {
            Anchor = Anchor.CentreRight,
            Origin = Anchor.CentreRight,
            RelativeSizeAxes = Axes.Both,
            Size = new Vector2(0.48f, 0.6f),
            X = -10,
            Y = 85
        };

        private readonly Container weaponContainer = new Container
        {
            Anchor = Anchor.CentreLeft,
            Origin = Anchor.CentreLeft,
            RelativeSizeAxes = Axes.Both,
            Size = new Vector2(0.48f, 0.6f),
            X = 10,
            Y = 85
        };

        private readonly AmountSelectionBox characterAmountSelectionBox = new()
        {
            Anchor = Anchor.TopRight,
            Origin = Anchor.TopRight,
            RelativeSizeAxes = Axes.X,
            Y = 100,
            Size = new Vector2(0.35f),
            Margin = new MarginPadding { Right = 35 },
            MaxValue = 10
        };

        private readonly AmountSelectionBox weaponAmountSelectionBox = new()
        {
            Anchor = Anchor.TopLeft,
            Origin = Anchor.TopLeft,
            RelativeSizeAxes = Axes.X,
            Y = 100,
            Size = new Vector2(0.35f),
            Margin = new MarginPadding { Left = 35 },
            MaxValue = 10
        };

        private readonly GqText gachaName = new("")
        {
            Anchor = Anchor.TopCentre,
            Origin = Anchor.TopCentre,
            Font = FontUsage.Default.With(size: 64)
        };

        /// <summary>
        /// The button used to start the roll process
        /// </summary>
        private GachaRollButton rollButton;

        private readonly BasicScrollContainer<GachaButton> gachaButtonList = new()
        {
            RelativeSizeAxes = Axes.Both
        };

        private readonly List<Gacha> gachas = [];

        public GachaContainer()
        {
            foreach (Gacha gacha in gachas)
            {
                gachaButtonList.Add(new GachaButton(gacha, () => LoadGacha(gacha))
                {
                    RelativeSizeAxes = Axes.X,
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    Width = 0.95f,
                    Height = 50
                });
            }
        }

        public void LoadGacha(Gacha gacha)
        {
            Logger.Log($"Loading gacha {gacha.Name} {gacha.Price} {gacha.Weapons.Count} {gacha.Characters.Count}");
            weaponContainer.Child = new ItemShowcaseContainer([..gacha.Weapons]);
            characterContainer.Child = new ItemShowcaseContainer([..gacha.Characters]);
            gachaName.Text = gacha.Name;
            rollButton.SetAction(() =>
            {
                int characterAmount = characterAmountSelectionBox.GetAmount();
                int weaponAmount = weaponAmountSelectionBox.GetAmount();
                int totalPulls = characterAmount + weaponAmount;
                int totalPrice = (int)gacha.Price * totalPulls;

                if (totalPulls == 0 || !user.Value.MoneyHandler.CanAfford(totalPrice)) return;

                List<Character> characterResults = gacha.RollCharacter(characterAmount, user.Value);
                List<Weapon> weaponResults = gacha.RollWeapon(weaponAmount, user.Value);
                setUpRollAnimation(gacha, characterResults, weaponResults);
            });
        }

        private void setUpRollAnimation(Gacha gacha, List<Character> characterResults, List<Weapon> weaponResults)
        {
            hideResultsOverlay(immediate: true);
            leftContainer.FadeOut(200);
            rightContainer.FadeOut(200);
            pullContainer.FadeIn(200);
            updatePullScale(characterResults.Count + weaponResults.Count);

            foreach (Character characterResult in characterResults) pullContainer.Add(new GachaRollContainer([..gacha.Characters], characterResult));
            foreach (Weapon weaponResult in weaponResults) pullContainer.Add(new GachaRollContainer([..gacha.Weapons], weaponResult));

            Scheduler.AddDelayed(() =>
            {
                leftContainer.FadeIn(200);
                rightContainer.FadeIn(200);
                pullContainer.FadeOut(200);
                pullContainer.Clear();
                pullContainer.ScaleTo(new Vector2(1f), 0);
                showResultsOverlay(characterResults, weaponResults);
            }, 8000);
        }

        private void showResultsOverlay(List<Character> characterResults, List<Weapon> weaponResults)
        {
            resultsList.Clear();

            int totalResults = characterResults.Count + weaponResults.Count;
            resultsTitle.Text = totalResults > 0
                ? $"You obtained {totalResults} item{(totalResults == 1 ? "" : "s")}"
                : "No items obtained";

            foreach (Character character in characterResults)
                resultsList.Add(createResultText("Character", character));

            foreach (Weapon weapon in weaponResults)
                resultsList.Add(createResultText("Weapon", weapon));

            if (totalResults == 0)
            {
                resultsList.Add(new GqText("You pulled nothing lol.")
                {
                    Font = FontUsage.Default.With(size: 28),
                });
            }

            resultsOverlay.Show();
            resultsOverlay.ClearTransforms();
            resultsOverlay.FadeOut(0);
            resultsOverlay.FadeIn(250, Easing.OutQuint);
        }

        private GqText createResultText(string itemType, EntityBase item)
        {
            return new GqText($"[{itemType}] {item.Name} ({item.StarRating.Value}-Star)")
            {
                Font = FontUsage.Default.With(size: 30),
                Colour = item.StarRating.GetColor(),
            };
        }

        private void hideResultsOverlay(bool immediate = false)
        {
            resultsOverlay.ClearTransforms();

            if (immediate)
            {
                resultsOverlay.FadeOut(0);
                resultsOverlay.Hide();
                return;
            }

            resultsOverlay.FadeOut(200).OnComplete(_ => resultsOverlay.Hide());
        }

        private void updatePullScale(int totalPulls)
        {
            float targetScale = 1f;

            if (totalPulls > full_size_pull_threshold)
                targetScale = MathF.Max(min_pull_scale, (float)full_size_pull_threshold / totalPulls);

            pullContainer.ScaleTo(new Vector2(targetScale), 200, Easing.OutQuint);
        }

        public void AnimateShow()
        {
            if (leftContainer == null || rightContainer == null) return;
            leftContainer.ResizeHeightTo(0.95f, 200, Easing.OutQuint);
            rightContainer.ResizeHeightTo(0.95f, 200, Easing.OutQuint);
        }

        public void AnimateHide()
        {
            if (leftContainer == null || rightContainer == null) return;
            leftContainer.ResizeHeightTo(0, 200, Easing.OutQuint);
            rightContainer.ResizeHeightTo(0, 200, Easing.OutQuint);
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Masking = true;
            RelativeSizeAxes = Axes.Both;
            Children =
            [
                leftContainer = new Container
                {
                    Masking = true,
                    CornerRadius = 10,
                    CornerExponent = 2,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(0.225f, 0.8f),
                    Children =
                    [
                        new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Colour = Colour4.Gray,
                        },
                        gachaButtonList
                    ]
                },
                rightContainer = new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Masking = true,
                    CornerRadius = 10,
                    CornerExponent = 2,
                    Size = new Vector2(0.75f, 0.8f),
                    Anchor = Anchor.TopRight,
                    Origin = Anchor.TopRight,
                    Children =
                    [
                        new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Colour = Colour4.Gray,
                        },
                        gachaName,
                        weaponContainer,
                        weaponAmountSelectionBox,
                        new GqText("Weapons")
                        {
                            Anchor = Anchor.TopLeft,
                            Origin = Anchor.TopLeft,
                            Margin = new MarginPadding { Left = 128 },
                            Font = FontUsage.Default.With(size: 48)
                        },
                        new GqText("Characters")
                        {
                            Anchor = Anchor.TopRight,
                            Origin = Anchor.TopRight,
                            Margin = new MarginPadding { Right = 128 },
                            Font = FontUsage.Default.With(size: 48)
                        },
                        characterContainer,
                        characterAmountSelectionBox,
                        rollButton = new GachaRollButton
                        {
                            Anchor = Anchor.BottomCentre,
                            Origin = Anchor.BottomCentre,
                            Size = new Vector2(200, 50),
                            Margin = new MarginPadding { Bottom = 25 }
                        }
                    ]
                },
                pullContainer = new FillFlowContainer<GachaRollContainer>
                {
                    RelativeSizeAxes = Axes.Both,
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopCentre
                },
                resultsOverlay = new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Alpha = 0,
                    Children =
                    [
                        new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Colour = new Colour4(0, 0, 0, 180)
                        },
                        new Container
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            RelativeSizeAxes = Axes.Both,
                            Size = new Vector2(0.6f, 0.7f),
                            Masking = true,
                            CornerRadius = 12,
                            CornerExponent = 2,
                            Children =
                            [
                                new Box
                                {
                                    RelativeSizeAxes = Axes.Both,
                                    Colour = new Colour4(28, 28, 28, 255)
                                },
                                new FillFlowContainer
                                {
                                    RelativeSizeAxes = Axes.Both,
                                    Direction = FillDirection.Vertical,
                                    Padding = new MarginPadding(24),
                                    Spacing = new Vector2(0, 12),
                                    Children =
                                    [
                                        resultsTitle = new GqText("You obtained")
                                        {
                                            Font = FontUsage.Default.With(size: 42),
                                        },
                                        new BasicScrollContainer
                                        {
                                            RelativeSizeAxes = Axes.Both,
                                            Height = 0.75f,
                                            Child = resultsList = new FillFlowContainer
                                            {
                                                RelativeSizeAxes = Axes.X,
                                                AutoSizeAxes = Axes.Y,
                                                Direction = FillDirection.Vertical,
                                                Spacing = new Vector2(0, 8)
                                            }
                                        },
                                        resultsContinueButton = new MainGqButton("Continue")
                                        {
                                            Width = 220,
                                            Height = 56
                                        }
                                    ]
                                }
                            ]
                        }
                    ]
                }
            ];

            resultsOverlay.Hide();
            resultsContinueButton.SetAction(() => hideResultsOverlay());
        }
    }
}
