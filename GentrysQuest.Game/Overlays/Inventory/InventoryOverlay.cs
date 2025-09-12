#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GentrysQuest.Game.Entity;
using GentrysQuest.Game.Entity.Drawables;
using GentrysQuest.Game.Entity.Weapon;
using GentrysQuest.Game.Graphics;
using GentrysQuest.Game.Overlays.Notifications;
using GentrysQuest.Game.Users;
using GentrysQuest.Game.Utils;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osuTK;
using osuTK.Input;

namespace GentrysQuest.Game.Overlays.Inventory
{
    public partial class InventoryOverlay : CompositeDrawable
    {
        /// <summary>
        /// The time it takes to fade
        /// </summary>
        private const int FADE_TIME = 300;

        private int selectedIndex = -1;

        /// <summary>
        /// if the inventory is being displayed
        /// </summary>
        public bool IsShowing = false;

        /// <summary>
        /// The section being displayed in the inventory
        /// </summary>
        private readonly Bindable<InventoryDisplay> displayingSection = new Bindable<InventoryDisplay>(InventoryDisplay.Hidden);

        private readonly Container topButtons;

        private readonly ItemDisplayContainer itemDisplayContainer;

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
        private readonly MainGqButton doneButton;

        private readonly string[] sortTypes = ["Star Rating", "Name", "Level"];
        private int sortIndexCounter = 0;

        private SelectionModes selectionMode = SelectionModes.Single;
        private Character focusedCharacter;
        private Artifact focusedArtifact;
        private Weapon focusedWeapon;
        private int artifactSelectionIndex;

        private IUser? user { get; set; }

        public InventoryOverlay()
        {
            user = user;
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
                    Size = new Vector2(0.98f, 100),
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
                    Margin = new MarginPadding { Top = 100 },
                    Children =
                    [
                        new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Colour = new Colour4(0, 0, 0, 185)
                        },
                        itemDisplayContainer = new ItemDisplayContainer(this)
                        {
                            Anchor = Anchor.BottomCentre,
                            Origin = Anchor.BottomCentre,
                            RelativePositionAxes = Axes.Both,
                            Height = 0.9f
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
                            Children =
                            [
                                itemContainer = new EntityInfoListContainer
                                {
                                    RelativePositionAxes = Axes.Y,
                                    Y = 0.1f,
                                    Anchor = Anchor.TopLeft,
                                    Origin = Anchor.TopLeft,
                                    Size = new Vector2(1)
                                },
                                doneButton = new MainGqButton("Done")
                                {
                                    Anchor = Anchor.BottomCentre,
                                    Origin = Anchor.BottomCentre,
                                    RelativePositionAxes = Axes.Both,
                                    Size = new Vector2(100, 64),
                                    Y = -0.04f,
                                    Alpha = 0,
                                    Action = delegate
                                    {
                                        doneButton!.Hide();

                                        if (displayingSection.Value == InventoryDisplay.Artifacts)
                                        {
                                            ExchangeArtifacts();
                                            _ = displayInfo(new EntityInfoDrawable(focusedArtifact));
                                        }
                                        else
                                        {
                                            ExchangeWeapons();
                                            _ = displayInfo(new EntityInfoDrawable(focusedWeapon));
                                        }
                                    }
                                }
                            ]
                        }
                    ]
                },
            };
            Origin = Anchor.Centre;
            displayingSection.BindValueChanged(async void (_) =>
            {
                await handleDisplayChange();
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
            selectionBackButton.SetAction(delegate
            {
                unDisplayInfo();
                clearSelections();
                selectionMode = SelectionModes.Single;
            });
            if (user != null) user.MoneyHandler.Amount.ValueChanged += delegate { moneyText.Text = $"${user.MoneyHandler.Amount}"; };
            Hide();
        }

        public void ProvideUser(IUser user) => this.user = user;

        private async void handleEntityClick(EntityInfoDrawable entityInfoDrawable)
        {
            switch (selectionMode)
            {
                case SelectionModes.Single:
                    doneButton.Hide();
                    await displayInfo(entityInfoDrawable);
                    break;

                case SelectionModes.Equipping:
                    doneButton.Hide();

                    switch (entityInfoDrawable.entity)
                    {
                        case Weapon weapon:
                            if (focusedCharacter.Weapon != null) user?.AddItem(focusedCharacter.Weapon);
                            user?.Weapons.Remove(weapon);
                            focusedCharacter.Weapon = weapon;
                            await displayInfo(new EntityInfoDrawable(focusedCharacter));
                            break;

                        case Artifact artifact:
                            Artifact? artifactRef = focusedCharacter.Artifacts.Get(artifactSelectionIndex);
                            if (artifactRef != null) user?.AddItem(artifactRef);
                            user?.Artifacts.Remove(artifact);
                            focusedCharacter.Artifacts.Equip(artifact, artifactSelectionIndex);
                            await displayInfo(new EntityInfoDrawable(focusedCharacter));
                            break;
                    }

                    break;

                case SelectionModes.Multi:
                    doneButton.Show();
                    if (entityInfoDrawable.entity == focusedArtifact) entityInfoDrawable.Unselect();
                    break;
            }
        }

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

        private async Task handleDisplayChange()
        {
            itemContainer.ScrollToTop();
            await itemContainer.ClearList();

            switch (displayingSection.Value)
            {
                case InventoryDisplay.Characters:
                    await itemContainer.AddFromList(user?.Characters);
                    break;

                case InventoryDisplay.Artifacts:
                    await itemContainer.AddFromList(user?.Artifacts);
                    break;

                case InventoryDisplay.Weapons:
                    await itemContainer.AddFromList(user?.Weapons);
                    break;
            }

            List<EntityInfoDrawable> items = itemContainer.GetEntityInfoDrawables();

            if (itemContainer.GetEntityInfoDrawables().Count == 0) return;

            itemContainer.Sort(sortTypes[sortIndexCounter], reverseButton.Reversed);
            items.ForEach(item =>
            {
                item.OnClickEvent += (_, _) => handleEntityClick(item);
                item.FadeOut();
                item.starRatingContainer.starRating.Value = 0;

                if (
                    (
                        item.entity == focusedCharacter ||
                        item.entity == focusedArtifact ||
                        item.entity == focusedWeapon
                    ) &&
                    selectionMode == SelectionModes.Multi
                )
                {
                    item.Add(new Container
                    {
                        RelativeSizeAxes = Axes.Both,
                        Children =
                        [
                            new Box
                            {
                                Colour = new Colour4(0, 0, 0, 185),
                                RelativeSizeAxes = Axes.Both
                            },
                            new SpriteText
                            {
                                Text = "Current Artifact",
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                Font = FontUsage.Default.With(size: 24)
                            }
                        ]
                    });
                }
            });
            await Task.Delay(itemContainer.GetSortDuration());

            for (int i = 0; i < items.Count; i++)
            {
                items[i].FadeIn(itemContainer.GetDelay());
                var starRating = items[i].entity.StarRating;
                items[i].starRatingContainer.starRating.Value = starRating;
                await Task.Delay(i >= itemContainer.GetDelayLimit() ? 0 : itemContainer.GetDelay());
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
            if (displayingSection.Value == inventoryDisplay) _ = handleDisplayChange();
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

        public void ClickWeapon()
        {
            clearSelections();
            Weapon? weaponRef = focusedCharacter.Weapon;

            if (weaponRef == null) SwapWeapon();
            else _ = displayInfo(new EntityInfoDrawable(weaponRef));
        }

        public void StartWeaponExchange()
        {
            displayingSection.Value = InventoryDisplay.Weapons;
            selectionMode = SelectionModes.Multi;
            unDisplayInfo();
            _ = handleDisplayChange();
            setStatus();
        }

        public void ExchangeWeapons()
        {
            var entityInfoDrawables = itemContainer.GetEntityInfoDrawables().Where(entityInfoDrawable => entityInfoDrawable.IsSelected && entityInfoDrawable.entity != focusedWeapon).ToList();

            if (entityInfoDrawables.Count == 0) return;

            foreach (EntityInfoDrawable entityInfoDrawable in entityInfoDrawables)
            {
                if (entityInfoDrawable.IsSelected && entityInfoDrawable.entity != focusedWeapon)
                {
                    focusedWeapon.AddXp(getItemXp(entityInfoDrawable.entity));
                    user.Weapons.Remove((Weapon)entityInfoDrawable.entity);
                }
            }
        }

        public void SwapWeapon()
        {
            swapCategory(InventoryDisplay.Weapons);
            selectionMode = SelectionModes.Equipping;
            unDisplayInfo();
            setStatus();
        }

        public void RemoveWeapon()
        {
            user?.AddItem(focusedCharacter.Weapon);
            focusedCharacter.Weapon = null;
            _ = displayInfo(new EntityInfoDrawable(focusedCharacter));
        }

        public void StartArtifactExchange()
        {
            displayingSection.Value = InventoryDisplay.Artifacts;
            selectionMode = SelectionModes.Multi;

            unDisplayInfo();
            _ = handleDisplayChange();
            setStatus();
        }

        public void ExchangeArtifacts()
        {
            var entityInfoDrawables = itemContainer.GetEntityInfoDrawables().Where(entityInfoDrawable => entityInfoDrawable.IsSelected && entityInfoDrawable.entity != focusedArtifact).ToList();

            if (entityInfoDrawables.Count == 0) return;

            foreach (var entityInfoDrawable in entityInfoDrawables)
            {
                if (focusedArtifact.Experience.CurrentLevel() < focusedArtifact.StarRating * 4)
                {
                    focusedArtifact.AddXp(getItemXp(entityInfoDrawable.entity));
                    user!.Artifacts.Remove((Artifact)entityInfoDrawable.entity);
                }
                else Notification.Create("Artifact is max level", NotificationType.Informative);
            }
        }

        public void ClickArtifact(int index)
        {
            clearSelections();
            Artifact? artifactRef = focusedCharacter.Artifacts.Get(index);

            if (artifactRef == null) SwapArtifact(index);
            else _ = displayInfo(new EntityInfoDrawable(artifactRef));

            setStatus();
        }

        public void SwapArtifact(int index)
        {
            swapCategory(InventoryDisplay.Artifacts);
            selectionMode = SelectionModes.Equipping;
            artifactSelectionIndex = index;
            unDisplayInfo();
        }

        public void RemoveArtifact(int index)
        {
            user!.AddItem(focusedCharacter.Artifacts.Remove(index));
            _ = displayInfo(new EntityInfoDrawable(focusedCharacter));
        }

        public void LevelUpWithMoney(EntityBase item, int amount)
        {
            if ((bool)user?.MoneyHandler.CanAfford(amount))
            {
                user?.MoneyHandler.Spend(amount);
                item.AddXp(amount * 10);
            }
            else Notification.Create("Can't Afford", NotificationType.Informative);
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

        private async Task displayInfo(EntityInfoDrawable entityInfoDrawable)
        {
            await itemContainer.ClearList();
            await itemDisplayContainer.SetEntity(entityInfoDrawable.entity);
            focusedCharacter = null;
            focusedWeapon = null;
            focusedArtifact = null;

            switch (entityInfoDrawable.entity)
            {
                case Character character:
                    focusedCharacter = character;
                    break;

                case Weapon weapon:
                    focusedWeapon = weapon;
                    break;

                case Artifact artifact:
                    focusedArtifact = artifact;
                    break;
            }

            itemDisplayContainer.Show();
        }

        private void unDisplayInfo()
        {
            selectionBackButton.FadeOut(100);
            itemDisplayContainer.FadeOut(100);
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            switch (e.Key)
            {
                case Key.Left:
                    selectedIndex = -1;
                    itemDisplayContainer.FadeOut(100);

                    if (displayingSection.Value != InventoryDisplay.Hidden)
                    {
                        if (displayingSection.Value == InventoryDisplay.Characters)
                            displayingSection.Value = InventoryDisplay.Weapons;
                        else if (displayingSection.Value == InventoryDisplay.Artifacts)
                            displayingSection.Value = InventoryDisplay.Characters;
                        else if (displayingSection.Value == InventoryDisplay.Weapons)
                            displayingSection.Value = InventoryDisplay.Artifacts;
                        selectionMode = SelectionModes.Single;
                        setStatus();
                    }

                    break;

                case Key.Right:
                    selectedIndex = -1;
                    itemDisplayContainer.FadeOut(100);

                    if (displayingSection.Value != InventoryDisplay.Hidden)
                    {
                        if (displayingSection.Value == InventoryDisplay.Characters)
                            displayingSection.Value = InventoryDisplay.Artifacts;
                        else if (displayingSection.Value == InventoryDisplay.Artifacts)
                            displayingSection.Value = InventoryDisplay.Weapons;
                        else if (displayingSection.Value == InventoryDisplay.Weapons)
                            displayingSection.Value = InventoryDisplay.Characters;
                        selectionMode = SelectionModes.Single;
                        setStatus();
                    }

                    break;

                case Key.Up:
                    if (displayingSection.Value != InventoryDisplay.Hidden)
                    {
                        var items = itemContainer.GetEntityInfoDrawables();

                        if (items.Count > 0)
                        {
                            selectedIndex = Math.Max(0, selectedIndex - 1);
                            clearSelections();
                            items[selectedIndex].Select();
                            itemContainer.ScrollToItem(selectedIndex);
                        }
                    }

                    break;

                case Key.Down:
                    if (displayingSection.Value != InventoryDisplay.Hidden)
                    {
                        var items = itemContainer.GetEntityInfoDrawables();

                        if (items.Count > 0)
                        {
                            selectedIndex = Math.Min(items.Count - 1, selectedIndex + 1);
                            clearSelections();
                            items[selectedIndex].Select();
                            itemContainer.ScrollToItem(selectedIndex);
                        }
                    }

                    break;

                case Key.Enter:
                    if (displayingSection.Value != InventoryDisplay.Hidden)
                    {
                        var items = itemContainer.GetEntityInfoDrawables();

                        if (items.Count > 0 && selectedIndex >= 0 && selectedIndex < items.Count)
                        {
                            handleEntityClick(items[selectedIndex]);
                        }
                    }

                    break;

                case Key.Escape:
                    selectionMode = SelectionModes.Single;
                    unDisplayInfo();
                    _ = handleDisplayChange();
                    break;
            }

            return base.OnKeyDown(e);
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

        public void InitiateArtifactTutorial()
        {
            artifactsButton.Highlight();
        }
    }
}
