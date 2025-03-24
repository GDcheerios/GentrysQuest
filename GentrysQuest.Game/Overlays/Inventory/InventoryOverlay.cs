#nullable enable
using System;
using System.Linq;
using GentrysQuest.Game.Entity;
using GentrysQuest.Game.Entity.Drawables;
using GentrysQuest.Game.Entity.Weapon;
using GentrysQuest.Game.Overlays.Notifications;
using GentrysQuest.Game.Users;
using GentrysQuest.Game.Utils;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osuTK;

namespace GentrysQuest.Game.Overlays.Inventory
{
    public partial class InventoryOverlay : CompositeDrawable
    {
        /// <summary>
        /// The time it takes to fade
        /// </summary>
        private const int FADE_TIME = 300;

        /// <summary>
        /// if the inventory is being displayed
        /// </summary>
        public bool IsShowing = false;

        /// <summary>
        /// The section being displayed in the inventory
        /// </summary>
        private readonly Bindable<InventoryDisplay> displayingSection = new Bindable<InventoryDisplay>(InventoryDisplay.Hidden);

        private readonly Container topButtons;

        private readonly InventoryButton charactersButton;
        private readonly InventoryButton artifactsButton;
        private readonly InventoryButton weaponsButton;
        private readonly InventoryButton exitButton;

        private readonly Container itemContainerBox;

        private readonly FillFlowContainer inventoryTop;

        private readonly EntityInfoListContainer itemContainer;

        private readonly InventoryReverseButton reverseButton;

        private readonly SpriteText moneyText;
        private readonly SpriteText categoryText;
        private readonly InnerInventoryButton sortButton;
        private readonly InventoryButton selectionBackButton;

        private readonly string[] sortTypes = new[] { "Star Rating", "Name", "Level" };
        private int sortIndexCounter = 0;

        private ItemDisplay itemInfo;
        private SelectionModes selectionMode = SelectionModes.Single;
        private Character equippingToCharacter;
        public Artifact FocusedArtifact;
        public Weapon FocusedWeapon;
        private int? artifactSelectionIndex;

        private IUser? user { get; set; }

        public InventoryOverlay()
        {
            this.user = user;
            RelativeSizeAxes = Axes.Both;
            RelativePositionAxes = Axes.Both;
            Anchor = Anchor.Centre;
            Depth = -3;
            InternalChildren = new Drawable[]
            {
                topButtons = new Container
                {
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopCentre,
                    RelativeSizeAxes = Axes.X,
                    RelativePositionAxes = Axes.Both,
                    Size = new Vector2(0.98f, 200),
                    Child = new FillFlowContainer<InventoryButton>
                    {
                        Direction = FillDirection.Horizontal,
                        RelativeSizeAxes = Axes.Both,
                        RelativePositionAxes = Axes.Both,
                        FillMode = FillMode.Stretch,
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Size = new Vector2(1),
                        Spacing = new Vector2(40),
                        Children = new[]
                        {
                            charactersButton = new InventoryButton("Characters"),
                            artifactsButton = new InventoryButton("Artifacts"),
                            weaponsButton = new InventoryButton("Weapons")
                        }
                    }
                },
                itemContainerBox = new Container
                {
                    Masking = true,
                    CornerExponent = 2,
                    CornerRadius = 20,
                    RelativeSizeAxes = Axes.Both,
                    RelativePositionAxes = Axes.Both,
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopCentre,
                    Size = new Vector2(0.7f, 0.78f),
                    Margin = new MarginPadding { Vertical = 180 },
                    Children = new Drawable[]
                    {
                        new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Colour = new Colour4(0, 0, 0, 185)
                        },
                        selectionBackButton = new InventoryButton("Back")
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            RelativeSizeAxes = Axes.X,
                            RelativePositionAxes = Axes.Both,
                            Size = new Vector2(0.25f, 64),
                            Position = new Vector2(-0.25f, 0.42f)
                        },
                        new DrawSizePreservingFillContainer
                        {
                            Children = new Drawable[]
                            {
                                moneyText = new SpriteText
                                {
                                    Anchor = Anchor.TopLeft,
                                    Origin = Anchor.TopLeft,
                                    Text = "$0",
                                    Font = FontUsage.Default.With(size: 56),
                                    Margin = new MarginPadding { Top = 20, Left = 50 }
                                },
                                categoryText = new SpriteText
                                {
                                    Text = "",
                                    Anchor = Anchor.TopCentre,
                                    Origin = Anchor.TopCentre,
                                    Font = FontUsage.Default.With(size: 36),
                                    Margin = new MarginPadding { Top = 10 }
                                },
                                new FillFlowContainer
                                {
                                    Direction = FillDirection.Horizontal,
                                    Anchor = Anchor.TopRight,
                                    Origin = Anchor.TopRight,
                                    Margin = new MarginPadding { Top = 20, Right = 50 },
                                    Children = new Drawable[]
                                    {
                                        reverseButton = new InventoryReverseButton
                                        {
                                            Anchor = Anchor.TopRight,
                                            Origin = Anchor.TopRight,
                                            Margin = new MarginPadding { Left = 12 },
                                            Size = new Vector2(46)
                                        },
                                        sortButton = new InnerInventoryButton(new SpriteText
                                        {
                                            Text = sortTypes[sortIndexCounter],
                                            Anchor = Anchor.Centre,
                                            Origin = Anchor.Centre
                                        })
                                        {
                                            Anchor = Anchor.TopRight,
                                            Origin = Anchor.TopRight,
                                            Size = new Vector2(200, 46),
                                        }
                                    }
                                }
                            }
                        },
                        new DrawSizePreservingFillContainer
                        {
                            RelativeSizeAxes = Axes.Both,
                            Child = itemContainer = new EntityInfoListContainer
                            {
                                RelativePositionAxes = Axes.Y,
                                Y = 0.1f,
                                Anchor = Anchor.TopLeft,
                                Origin = Anchor.TopLeft,
                                Size = new Vector2(1, 0.2f)
                            }
                        },
                        new DrawSizePreservingFillContainer
                        {
                            Child = itemInfo = new ItemDisplay(this)
                            {
                                Size = new Vector2(0, 0.9f),
                                Y = 0.1f,
                                Anchor = Anchor.TopRight,
                                Origin = Anchor.TopRight,
                            }
                        }
                    }
                },
            };
            Origin = Anchor.Centre;
            displayingSection.BindValueChanged(_ =>
            {
                changeState();
            });
            charactersButton.SetAction(() =>
            {
                swapCategory(InventoryDisplay.Characters);
                unDisplayInfo();
            });
            artifactsButton.SetAction(() =>
            {
                swapCategory(InventoryDisplay.Artifacts);
                unDisplayInfo();
            });
            weaponsButton.SetAction(() =>
            {
                swapCategory(InventoryDisplay.Weapons);
                unDisplayInfo();
            });
            sortButton.OnClickEvent += delegate
            {
                sortButton.Text.Text = HelpMe.GetNextValueFromArray(sortTypes, ref sortIndexCounter);
                itemContainer.Sort(sortTypes[sortIndexCounter], reverseButton.Reversed);
            };
            reverseButton.OnClickEvent += delegate { itemContainer.Sort(sortTypes[sortIndexCounter], reverseButton.Reversed); };
            itemContainer.FinishedLoading += delegate { itemContainer.Sort(sortTypes[sortIndexCounter], reverseButton.Reversed); };
            itemContainer.FinishedLoading += delegate
            {
                switch (selectionMode)
                {
                    case SelectionModes.Single:
                        selectionBackButton.FadeOut(100);
                        itemContainer.ResizeHeightTo(1, 100);

                        foreach (EntityInfoDrawable entityInfoDrawable in itemContainer.GetEntityInfoDrawables())
                        {
                            entityInfoDrawable.OnClickEvent += delegate
                            {
                                foreach (EntityInfoDrawable entityInfoDrawable2 in itemContainer.GetEntityInfoDrawables())
                                {
                                    if (entityInfoDrawable != entityInfoDrawable2) entityInfoDrawable2.Unselect();
                                }

                                unDisplayInfo();
                                if (entityInfoDrawable.IsSelected) displayInfo(entityInfoDrawable);

                                switch (entityInfoDrawable.entity)
                                {
                                    case Character entity:
                                        equippingToCharacter = entity;
                                        break;
                                }
                            };
                        }

                        break;

                    case SelectionModes.Multi:
                        selectionBackButton.FadeIn(100);
                        itemContainer.ResizeHeightTo(0.79f, 100);

                        foreach (EntityInfoDrawable entityInfoDrawable in itemContainer.GetEntityInfoDrawables())
                        {
                            if (entityInfoDrawable.entity == FocusedWeapon || entityInfoDrawable.entity == FocusedArtifact)
                            {
                                entityInfoDrawable.Unselect();
                                entityInfoDrawable.ResizeTo(0);
                            }
                        }

                        break;

                    case SelectionModes.Equipping:
                        selectionBackButton.FadeIn(100);
                        itemContainer.ResizeHeightTo(0.79f, 100);

                        foreach (EntityInfoDrawable entityInfoDrawable in itemContainer.GetEntityInfoDrawables())
                        {
                            entityInfoDrawable.OnClickEvent += delegate
                            {
                                entityInfoDrawable.Unselect();

                                if (artifactSelectionIndex == null)
                                {
                                    Weapon weapon = (Weapon)entityInfoDrawable.entity;
                                    Weapon? weaponFromCharacter = equippingToCharacter?.Weapon;
                                    equippingToCharacter?.SetWeapon(weapon);
                                    if (weaponFromCharacter != null) user?.AddItem(weaponFromCharacter);
                                    user?.Weapons.Remove(weapon);
                                }
                                else
                                {
                                    Artifact artifact = (Artifact)entityInfoDrawable.entity;
                                    Artifact? artifactFromCharacter = equippingToCharacter?.Artifacts.Get((int)artifactSelectionIndex);
                                    equippingToCharacter?.Artifacts.Equip(artifact, (int)artifactSelectionIndex);
                                    if (artifactFromCharacter != null) user?.AddItem(artifactFromCharacter);
                                    user?.Artifacts.Remove(artifact);
                                }

                                swapCategory(InventoryDisplay.Characters);
                                itemInfo.DisplayItem(equippingToCharacter);
                            };
                        }

                        break;
                }
            };
            selectionBackButton.SetAction(delegate
            {
                selectionMode = SelectionModes.Single;
                displayingSection.Value = InventoryDisplay.Characters;
            });
            // user.Money.Amount.ValueChanged += delegate { moneyText.Text = $"${GameData.Money.Amount}"; };
            Hide();
        }

        public void ProvideUser(IUser user) => this.user = user;

        private void setStatus()
        {
            string category;

            switch (selectionMode)
            {
                case SelectionModes.Equipping:
                    category = "Equipping ";
                    break;

                case SelectionModes.Multi:
                    category = "Select ";
                    break;

                default:
                    category = "";
                    break;
            }

            category += displayingSection.Value;
            categoryText.Text = category;
        }

        private void changeState()
        {
            itemContainer.ClearList();

            switch (displayingSection.Value)
            {
                case InventoryDisplay.Characters:
                    itemContainer.AddFromList(user?.Characters);
                    itemContainer.Sort(sortTypes[sortIndexCounter], reverseButton.Reversed);
                    break;

                case InventoryDisplay.Artifacts:
                    itemContainer.AddFromList(user?.Artifacts);
                    itemContainer.Sort(sortTypes[sortIndexCounter], reverseButton.Reversed);
                    break;

                case InventoryDisplay.Weapons:
                    itemContainer.AddFromList(user?.Weapons);
                    itemContainer.Sort(sortTypes[sortIndexCounter], reverseButton.Reversed);
                    break;
            }
        }

        private void clearSelections()
        {
            foreach (EntityInfoDrawable entityInfoDrawable in itemContainer.GetEntityInfoDrawables())
            {
                entityInfoDrawable.Unselect();
            }
        }

        private void swapCategory(InventoryDisplay inventoryDisplay)
        {
            displayingSection.Value = inventoryDisplay;
            selectionMode = SelectionModes.Single;
            setStatus();
        }

        private int getItemXp(EntityBase item)
        {
            int xp = 0;

            xp += (item.Experience.CurrentLevel() - 1) * 250;
            xp += (int)Math.Pow(item.StarRating.Value, 1.2) * 500;
            xp += item.Experience.CurrentLevel() / 4 * 1000;
            xp += item.Difficulty * 10000;

            return xp;
        }

        public void StartWeaponExchange()
        {
            displayingSection.Value = InventoryDisplay.Weapons;
            selectionMode = SelectionModes.Multi;
            changeState();
            setStatus();
        }

        public void ExchangeWeapons()
        {
            foreach (EntityInfoDrawable entityInfoDrawable in itemContainer.GetEntityInfoDrawables())
            {
                if (entityInfoDrawable.IsSelected && entityInfoDrawable.entity != FocusedWeapon)
                {
                    FocusedWeapon.AddXp(getItemXp(entityInfoDrawable.entity));
                    user.Weapons.Remove((Weapon)entityInfoDrawable.entity);
                }
            }

            changeState();
        }

        public void ClickWeapon()
        {
            clearSelections();
            Weapon? weaponRef = equippingToCharacter.Weapon;
            artifactSelectionIndex = null;

            if (weaponRef == null)
            {
                displayingSection.Value = InventoryDisplay.Weapons;
                selectionMode = SelectionModes.Equipping;
            }
            else itemInfo.DisplayItem(weaponRef);

            setStatus();
        }

        public void SwapWeapon()
        {
            artifactSelectionIndex = null;
            displayingSection.Value = InventoryDisplay.Weapons;
            selectionMode = SelectionModes.Equipping;
            itemInfo.DisplayItem(equippingToCharacter);
            setStatus();
        }

        public void RemoveWeapon()
        {
            user!.AddItem(equippingToCharacter.Weapon);
            equippingToCharacter.SetWeapon(null);
            itemInfo.DisplayItem(equippingToCharacter);
        }

        public void StartArtifactExchange()
        {
            displayingSection.Value = InventoryDisplay.Artifacts;
            selectionMode = SelectionModes.Multi;
            changeState();
            setStatus();
        }

        public void ExchangeArtifacts()
        {
            foreach (var entityInfoDrawable in itemContainer.GetEntityInfoDrawables().Where(entityInfoDrawable => entityInfoDrawable.IsSelected && entityInfoDrawable.entity != FocusedArtifact))
            {
                if (FocusedArtifact.Experience.CurrentLevel() < FocusedArtifact.StarRating * 4)
                {
                    FocusedArtifact.AddXp(getItemXp(entityInfoDrawable.entity));
                    user!.Artifacts.Remove((Artifact)entityInfoDrawable.entity);
                }
                else Notification.Create("Artifact is max level", NotificationType.Informative);
            }

            changeState();
        }

        public void ClickArtifact(int index)
        {
            clearSelections();
            Artifact? artifactRef = equippingToCharacter.Artifacts.Get(index);

            if (artifactRef == null)
            {
                displayingSection.Value = InventoryDisplay.Artifacts;
                selectionMode = SelectionModes.Equipping;
                artifactSelectionIndex = index;
            }
            else itemInfo.DisplayItem(artifactRef);

            setStatus();
        }

        public void SwapArtifact(int index)
        {
            displayingSection.Value = InventoryDisplay.Artifacts;
            selectionMode = SelectionModes.Equipping;
            artifactSelectionIndex = index;
            itemInfo.DisplayItem(equippingToCharacter);
        }

        public void RemoveArtifact(int index)
        {
            user!.AddItem(equippingToCharacter.Artifacts.Remove(index));
            itemInfo.DisplayItem(equippingToCharacter);
        }

        public void ToggleDisplay()
        {
            switch (IsShowing)
            {
                case true:
                    Hide();
                    break;

                case false:
                    Show();
                    break;
            }
        }

        private void displayInfo(EntityInfoDrawable entityInfoDrawable)
        {
            itemContainer.ResizeTo(new Vector2(0.5f, 0.9f), 100);
            itemInfo.ResizeTo(new Vector2(0.5f, 0.9f), 100);
            itemInfo.DisplayItem(entityInfoDrawable.entity);
        }

        private void unDisplayInfo()
        {
            itemContainer.ResizeTo(new Vector2(1, 0.9f), 100);
            itemInfo.ResizeTo(new Vector2(0, 0.9f), 100);
            itemInfo.FadeOut(100);
        }

        public override void Show()
        {
            IsShowing = true;
            base.Show();
            topButtons.MoveToY(0, FADE_TIME, Easing.InOutCubic);
            itemContainerBox.FadeIn(FADE_TIME, Easing.InOutCubic);
            displayingSection.Value = InventoryDisplay.Characters;
            selectionMode = SelectionModes.Single;
            setStatus();
        }

        public override void Hide()
        {
            unDisplayInfo();
            IsShowing = false;
            topButtons.MoveToY(-2, FADE_TIME, Easing.InOutCubic);
            itemContainer.ClearList();
            displayingSection.Value = InventoryDisplay.Hidden;
            itemContainerBox.FadeOut(FADE_TIME, Easing.InOutCubic);
            setStatus();
        }
    }
}
