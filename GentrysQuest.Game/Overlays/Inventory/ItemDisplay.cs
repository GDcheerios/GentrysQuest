using System;
using GentrysQuest.Game.Entity;
using GentrysQuest.Game.Entity.Drawables;
using GentrysQuest.Game.Entity.Weapon;
using GentrysQuest.Game.Graphics;
using GentrysQuest.Game.Graphics.TextStyles;
using GentrysQuest.Game.Users;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Logging;
using osuTK;

namespace GentrysQuest.Game.Overlays.Inventory
{
    public partial class ItemDisplay : CompositeDrawable
    {
        private EntityBase entity;
        private readonly InventoryOverlay inventoryReference;
        private readonly SpriteText nameText;
        private readonly TaggedTextContainer descriptionText;
        private readonly SpriteText levelText;
        private readonly SpriteText experienceRequirementText;
        private readonly ProgressBar experienceBar;
        private readonly InventoryLevelUpBox inventoryLevelUpBox;
        private readonly InventoryButton levelUpButton;
        private readonly StatDrawableContainer statDrawableContainer;
        private readonly InnerInventoryButton detailsButton;
        private readonly InnerInventoryButton statsButton;
        private readonly InnerInventoryButton artifactsButton;
        private readonly Container detailsContainer;
        private readonly Container statsContainer;
        private readonly Container artifactsContainer;
        private EquipPanel artifactIcon1;
        private EquipPanel artifactIcon2;
        private EquipPanel artifactIcon3;
        private EquipPanel artifactIcon4;
        private EquipPanel artifactIcon5;
        private EquipPanel weaponPanel;
        private Character characterInfo;
        private Artifact artifactInfo;
        private Weapon weaponInfo;
        private Bindable<DisplayMode> displaying = new();

        [Resolved]
        private Bindable<IUser> user { get; set; }

        #region DesignProperties

        private const int LEVEL_UP_BUTTON_WIDTH = 420;
        private const int LEVEL_UP_BUTTON_HEIGHT = 50;

        #endregion

        public ItemDisplay(InventoryOverlay inventoryReference)
        {
            displaying.Value = DisplayMode.Details;
            this.inventoryReference = inventoryReference;
            RelativeSizeAxes = Axes.Both;
            RelativePositionAxes = Axes.Both;
            InternalChildren = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = ColourInfo.GradientVertical(new Colour4(12, 12, 12, 46), new Colour4(21, 21, 21, 64))
                },
                new Container // name
                {
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopCentre,
                    Size = new Vector2(1, 80),
                    RelativeSizeAxes = Axes.X,
                    Children = new Drawable[]
                    {
                        new Container
                        {
                            RelativePositionAxes = Axes.Both,
                            RelativeSizeAxes = Axes.Both,
                            Colour = ColourInfo.GradientHorizontal(Colour4.Gray, new Colour4(255, 255, 255, 0)),
                            Children = new Drawable[]
                            {
                                new Box
                                {
                                    RelativePositionAxes = Axes.Both,
                                    RelativeSizeAxes = Axes.Both
                                },
                                new Box
                                {
                                    Colour = Colour4.Black,
                                    RelativePositionAxes = Axes.Both,
                                    RelativeSizeAxes = Axes.X,
                                    Anchor = Anchor.TopCentre,
                                    Origin = Anchor.TopCentre,
                                    Size = new Vector2(1, 3)
                                },
                                new Box
                                {
                                    Colour = Colour4.Black,
                                    RelativePositionAxes = Axes.Both,
                                    RelativeSizeAxes = Axes.X,
                                    Anchor = Anchor.BottomCentre,
                                    Origin = Anchor.BottomCentre,
                                    Size = new Vector2(1, 3)
                                }
                            }
                        },
                        nameText = new SpriteText
                        {
                            Text = "",
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Font = FontUsage.Default.With(size: 64),
                            RelativeSizeAxes = Axes.X,
                            Width = 1,
                            Truncate = true,
                            AllowMultiline = false,
                            Padding = new MarginPadding { Left = 2 }
                        }
                    }
                },
                new Container // navigation bar
                {
                    RelativeSizeAxes = Axes.X,
                    Y = 80,
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopCentre,
                    Children = new Drawable[]
                    {
                        new Container
                        {
                            Anchor = Anchor.TopCentre,
                            Origin = Anchor.TopCentre,
                            RelativeSizeAxes = Axes.X,
                            Size = new Vector2(1, 50),
                            Children = new Drawable[]
                            {
                                new Box
                                {
                                    RelativeSizeAxes = Axes.Both,
                                    Colour = Colour4.Gray
                                },
                                new Box
                                {
                                    Anchor = Anchor.BottomCentre,
                                    Origin = Anchor.BottomCentre,
                                    RelativeSizeAxes = Axes.X,
                                    Size = new Vector2(1, 3),
                                    Colour = Colour4.DarkGray
                                },
                                new Box
                                {
                                    Anchor = Anchor.TopCentre,
                                    Origin = Anchor.TopCentre,
                                    RelativeSizeAxes = Axes.X,
                                    Size = new Vector2(1, 3),
                                    Colour = Colour4.DarkGray
                                },
                                new FillFlowContainer
                                {
                                    AutoSizeAxes = Axes.Both,
                                    Direction = FillDirection.Horizontal,
                                    Origin = Anchor.Centre,
                                    Anchor = Anchor.Centre,
                                    Spacing = new Vector2(20, 0),
                                    Children = new Drawable[]
                                    {
                                        detailsButton = new InnerInventoryButton(new SpriteText
                                        {
                                            Text = "Details",
                                            Anchor = Anchor.Centre,
                                            Origin = Anchor.Centre
                                        })
                                        {
                                            Size = new Vector2(128, 42)
                                        },
                                        statsButton = new InnerInventoryButton(new SpriteText
                                        {
                                            Text = "Stats",
                                            Anchor = Anchor.Centre,
                                            Origin = Anchor.Centre
                                        })
                                        {
                                            Size = new Vector2(128, 42)
                                        },
                                        artifactsButton = new InnerInventoryButton(new SpriteText
                                        {
                                            Text = "Artifacts",
                                            Anchor = Anchor.Centre,
                                            Origin = Anchor.Centre
                                        })
                                        {
                                            Size = new Vector2(128, 42)
                                        }
                                    }
                                }
                            }
                        }
                    }
                },
                new Container // details, stats, artifacts parent
                {
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopCentre,
                    RelativeSizeAxes = Axes.X,
                    Width = 0.95f,
                    Height = 400,
                    CornerExponent = 2,
                    CornerRadius = 15,
                    Masking = true,
                    Y = 150,
                    Children = new Drawable[]
                    {
                        new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Colour = new Colour4(0, 0, 0, 48)
                        },
                        new Container
                        {
                            Anchor = Anchor.TopCentre,
                            Origin = Anchor.TopCentre,
                            Child = new FillFlowContainer
                            {
                                Direction = FillDirection.Horizontal,
                                Spacing = new Vector2(10, 0),
                                Children = new Drawable[]
                                {
                                    levelText = new SpriteText
                                    {
                                        Text = "Level",
                                        Anchor = Anchor.TopCentre,
                                        Origin = Anchor.TopCentre,
                                        Font = FontUsage.Default.With(size: 24)
                                    },
                                    experienceBar = new ProgressBar(0, 1)
                                    {
                                        Size = new Vector2(100, 12),
                                        ForegroundColour = Colour4.Aqua,
                                        BackgroundColour = new Colour4(255, 255, 255, 12),
                                        Margin = new MarginPadding { Top = 5 },
                                        Anchor = Anchor.TopCentre,
                                        Origin = Anchor.TopCentre,
                                    },
                                    experienceRequirementText = new SpriteText
                                    {
                                        Text = "",
                                        Anchor = Anchor.TopCentre,
                                        Origin = Anchor.TopCentre,
                                    }
                                }
                            }
                        },
                        detailsContainer = new Container
                        {
                            RelativeSizeAxes = Axes.Both,
                            Children = new Drawable[]
                            {
                                weaponPanel = new EquipPanel(null)
                                {
                                    Margin = new MarginPadding { Left = 40, Top = 30 },
                                    Anchor = Anchor.TopLeft,
                                    Origin = Anchor.TopLeft
                                },
                                descriptionText = new TaggedTextContainer
                                {
                                    Y = 130,
                                    Anchor = Anchor.TopCentre,
                                    Origin = Anchor.TopCentre,
                                    RelativeSizeAxes = Axes.Both,
                                    Padding = new MarginPadding(20)
                                }
                            }
                        },
                        statsContainer = new Container
                        {
                            RelativeSizeAxes = Axes.Both,
                            Children = new Drawable[]
                            {
                                statDrawableContainer = new StatDrawableContainer
                                {
                                    Y = 20,
                                    Anchor = Anchor.TopCentre,
                                    Origin = Anchor.TopCentre
                                }
                            }
                        },
                        artifactsContainer = new Container
                        {
                            RelativeSizeAxes = Axes.Both,
                            Children = new Drawable[]
                            {
                                new BasicScrollContainer
                                {
                                    RelativeSizeAxes = Axes.Both,
                                    Size = new Vector2(1, 0.8f),
                                    Position = new Vector2(0, 32),
                                    ClampExtension = 1,
                                    ScrollbarVisible = true,
                                    Child = new FillFlowContainer
                                    {
                                        Y = 0,
                                        Direction = FillDirection.Vertical,
                                        AutoSizeAxes = Axes.Y,
                                        Anchor = Anchor.TopLeft,
                                        Origin = Anchor.TopLeft,
                                        Children = new Drawable[]
                                        {
                                            artifactIcon1 = new EquipPanel(null),
                                            artifactIcon2 = new EquipPanel(null),
                                            artifactIcon3 = new EquipPanel(null),
                                            artifactIcon4 = new EquipPanel(null),
                                            artifactIcon5 = new EquipPanel(null),
                                        }
                                    }
                                }
                            }
                        }
                    }
                },
                new FillFlowContainer // Level buttons
                {
                    Size = new Vector2(0, 60),
                    Anchor = Anchor.BottomCentre,
                    Origin = Anchor.BottomCentre,
                    Direction = FillDirection.Horizontal,
                    Margin = new MarginPadding { Bottom = 20 },
                    AutoSizeAxes = Axes.X,
                    Spacing = new Vector2(20),
                    Children = new Drawable[]
                    {
                        inventoryLevelUpBox = new InventoryLevelUpBox
                        {
                            Size = new Vector2(LEVEL_UP_BUTTON_WIDTH, LEVEL_UP_BUTTON_HEIGHT),
                        },
                        levelUpButton = new InventoryButton("Upgrade")
                        {
                            Size = new Vector2(LEVEL_UP_BUTTON_WIDTH, LEVEL_UP_BUTTON_HEIGHT),
                        }
                    }
                }
            };
            detailsButton.SetAction(delegate { displaying.Value = DisplayMode.Details; });
            statsButton.SetAction(delegate { displaying.Value = DisplayMode.Stats; });
            artifactsButton.SetAction(delegate { displaying.Value = DisplayMode.Artifacts; });
            statsContainer.Hide();
            artifactsContainer.Hide();
            displaying.ValueChanged += delegate
            {
                const int DURATION = 100;

                switch (displaying.Value)
                {
                    case DisplayMode.Details:
                        detailsContainer.FadeIn(DURATION);
                        artifactsContainer.FadeOut(DURATION);
                        statsContainer.FadeOut(DURATION);
                        break;

                    case DisplayMode.Stats:
                        detailsContainer.FadeOut(DURATION);
                        artifactsContainer.FadeOut(DURATION);
                        statsContainer.FadeIn(DURATION);
                        break;

                    case DisplayMode.Artifacts:
                        detailsContainer.FadeOut(DURATION);
                        artifactsContainer.FadeIn(DURATION);
                        statsContainer.FadeOut(DURATION);
                        break;
                }
            };
        }

        public void DisplayItem(EntityBase entity)
        {
            characterInfo = null;
            artifactInfo = null;
            weaponInfo = null;
            this.FadeIn(50);
            nameText.Text = entity.Name;
            descriptionText.SetTaggedText(entity.Description);
            updateExperienceBar(entity);
            levelUpButton.SetAction(delegate
            {
                int amount = inventoryLevelUpBox.GetAmount();

                if (user.Value.MoneyHandler.CanAfford(amount))
                {
                    entity.AddXp(amount * 10);
                    user.Value.MoneyHandler.Spend(amount);
                }

                updateExperienceBar(entity);
            });
            statDrawableContainer.Clear();

            switch (entity)
            {
                case Character character:
                    characterInfo = character;

                    inventoryLevelUpBox.Show();
                    resizeLevelUpComponents(2);

                    descriptionText.Y = 130;
                    weaponPanel.Show();
                    artifactsButton.Show();
                    artifactsButton.Scale = new Vector2(1);

                    foreach (Stat stat in character.Stats.GetStats())
                    {
                        statDrawableContainer.AddStat(new StatDrawable($"{stat.Name} {stat.GetDefault()} + {stat.GetAdditional()}", (float)stat.Total(), false, stat.Name));
                    }

                    weaponPanel.SetEquip(character.Weapon);
                    weaponPanel.SetAction(delegate { inventoryReference.ClickWeapon(); });
                    weaponPanel.SetSwapAction(delegate { inventoryReference.SwapWeapon(); });
                    weaponPanel.SetRemoveAction(delegate { inventoryReference.RemoveWeapon(); });

                    artifactIcon1.SetEquip(character.Artifacts.Get(0));
                    artifactIcon1.SetAction(delegate { inventoryReference.ClickArtifact(0); });
                    artifactIcon1.SetSwapAction(delegate { inventoryReference.SwapArtifact(0); });
                    artifactIcon1.SetRemoveAction(delegate { inventoryReference.RemoveArtifact(0); });

                    artifactIcon2.SetEquip(character.Artifacts.Get(1));
                    artifactIcon2.SetAction(delegate { inventoryReference.ClickArtifact(1); });
                    artifactIcon2.SetSwapAction(delegate { inventoryReference.SwapArtifact(1); });
                    artifactIcon2.SetRemoveAction(delegate { inventoryReference.RemoveArtifact(1); });

                    artifactIcon3.SetEquip(character.Artifacts.Get(2));
                    artifactIcon3.SetAction(delegate { inventoryReference.ClickArtifact(2); });
                    artifactIcon3.SetSwapAction(delegate { inventoryReference.SwapArtifact(2); });
                    artifactIcon3.SetRemoveAction(delegate { inventoryReference.RemoveArtifact(2); });

                    artifactIcon4.SetEquip(character.Artifacts.Get(3));
                    artifactIcon4.SetAction(delegate { inventoryReference.ClickArtifact(3); });
                    artifactIcon4.SetSwapAction(delegate { inventoryReference.SwapArtifact(3); });
                    artifactIcon4.SetRemoveAction(delegate { inventoryReference.RemoveArtifact(3); });

                    artifactIcon5.SetEquip(character.Artifacts.Get(4));
                    artifactIcon5.SetAction(delegate { inventoryReference.ClickArtifact(4); });
                    artifactIcon5.SetSwapAction(delegate { inventoryReference.SwapArtifact(4); });
                    artifactIcon5.SetRemoveAction(delegate { inventoryReference.RemoveArtifact(4); });

                    characterInfo.OnUpdateStats -= updateCharacterStatContainer;
                    characterInfo.OnUpdateStats += updateCharacterStatContainer;
                    break;

                case Artifact artifact:
                    displaying.Value = DisplayMode.Stats;
                    inventoryReference.FocusedArtifact = artifact;
                    artifactInfo = artifact;
                    Buff mainAttribute = artifact.MainAttribute;

                    string description = $"{artifact.Description}\n"
                                         + $"Two Set Buff:\n"
                                         + $"{artifact.family.TwoSetBuff.BuffExplanation()}\n"
                                         + $"Four Set Buff:\n"
                                         + $"{artifact.family.FourSetBuff?.Explanation}";

                    descriptionText.SetTaggedText(description);
                    descriptionText.Y = 0;
                    inventoryLevelUpBox.Hide();
                    levelUpButton.SetAction(delegate
                    {
                        inventoryReference.ExchangeArtifacts();
                        updateExperienceBar(entity);
                    });
                    resizeLevelUpComponents(2);
                    statDrawableContainer.AddStat(new StatDrawable(mainAttribute.StatType.ToString(), (float)mainAttribute.Value.Value, true, identifier: getIdentifier(mainAttribute), isPercent: mainAttribute.IsPercent));

                    foreach (Buff attribute in artifact.Attributes)
                    {
                        statDrawableContainer.AddStat(new StatDrawable(attribute.StatType.ToString(), (float)attribute.Value.Value, false, identifier: getIdentifier(attribute), isPercent: attribute.IsPercent));
                    }

                    artifactsButton.Hide();
                    weaponPanel.Hide();
                    artifactsButton.Scale = new Vector2(0);

                    artifactInfo.OnLevelUp -= updateArtifactStatContainer;
                    artifactInfo.OnLevelUp += updateArtifactStatContainer;
                    break;

                case Weapon weapon:
                    displaying.Value = DisplayMode.Stats;
                    inventoryReference.FocusedWeapon = weapon;
                    weaponInfo = weapon;

                    descriptionText.Y = 0;
                    inventoryLevelUpBox.Show();
                    weaponPanel.Hide();
                    levelUpButton.SetAction(delegate
                    {
                        if (user.Value.MoneyHandler.CanAfford(inventoryLevelUpBox.GetAmount()))
                        {
                            entity.AddXp(inventoryLevelUpBox.GetAmount() * 10);
                            user.Value.MoneyHandler.Spend(inventoryLevelUpBox.GetAmount());
                        }

                        inventoryReference.ExchangeWeapons();

                        updateExperienceBar(entity);
                    });
                    resizeLevelUpComponents(3);
                    artifactsButton.Hide();
                    artifactsButton.Scale = new Vector2(0);
                    statDrawableContainer.AddStat(new StatDrawable("Damage", (float)weapon.Damage.Total(), true));
                    statDrawableContainer.AddStat(new StatDrawable(weapon.Buff.StatType.ToString(), (float)weapon.Buff.Value.Value, false));
                    weaponInfo.OnLevelUp -= updateWeaponStatContainer;
                    weaponInfo.OnLevelUp += updateWeaponStatContainer;
                    break;
            }
        }

        private void resizeLevelUpComponents(int amount)
        {
            if (amount < 2) amount = 2;
            inventoryLevelUpBox.ResizeTo(new Vector2(LEVEL_UP_BUTTON_WIDTH / amount, LEVEL_UP_BUTTON_HEIGHT));
            levelUpButton.ResizeTo(new Vector2(LEVEL_UP_BUTTON_WIDTH / amount, LEVEL_UP_BUTTON_HEIGHT));
        }

        private void updateExperienceBar(EntityBase entityRef)
        {
            levelText.Text = entityRef.Experience.Level.ToString();
            experienceRequirementText.Text = entityRef.Experience.Xp.ToString();
            experienceBar.Min = 0;
            experienceBar.Current = entityRef.Experience.Xp.Current.Value;
            experienceBar.Max = entityRef.Experience.Xp.Requirement.Value;
        }

        private void updateCharacterStatContainer()
        {
            try
            {
                foreach (Stat stat in characterInfo.Stats.GetStats())
                {
                    statDrawableContainer.GetStatDrawable(stat.Name).UpdateValue((float)stat.Total());
                }
            }
            catch (Exception)
            {
                Logger.Log("Yep, You already know (Character)");
            }
        }

        private void updateArtifactStatContainer()
        {
            try
            {
                statDrawableContainer.GetStatDrawable(getIdentifier(artifactInfo.MainAttribute)).UpdateValue((float)artifactInfo.MainAttribute.Value.Value);

                foreach (Buff buff in artifactInfo.Attributes)
                {
                    StatDrawable statDrawable = statDrawableContainer.GetStatDrawable(getIdentifier(buff));
                    if (statDrawable != null) statDrawable.UpdateValue((float)buff.Value.Value);
                    else statDrawableContainer.AddStat(new StatDrawable(buff.StatType.ToString(), (float)buff.Value.Value, false, getIdentifier(buff), isPercent: buff.IsPercent), true);
                }
            }
            catch (Exception)
            {
                Logger.Log("Yep, You already know (Artifact)");
            }
        }

        private void updateWeaponStatContainer()
        {
            try
            {
                statDrawableContainer.GetStatDrawable("Damage").UpdateValue((float)weaponInfo.Damage.Total());
                statDrawableContainer.GetStatDrawable(weaponInfo.Buff.StatType.ToString()).UpdateValue((float)weaponInfo.Buff.Value.Value);
            }
            catch (Exception)
            {
                Logger.Log("Yep, You already know (Weapon)");
            }
        }

        private string getIdentifier(Buff buff) => $"{buff.StatType}{(buff.IsPercent ? '%' : ' ')}";
    }
}
