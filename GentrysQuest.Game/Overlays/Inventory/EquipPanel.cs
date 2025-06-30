using System;
using GentrysQuest.Game.Entity;
using GentrysQuest.Game.Entity.Drawables;
using GentrysQuest.Game.Entity.Weapon;
using GentrysQuest.Game.Graphics;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Input.Events;
using osuTK;

namespace GentrysQuest.Game.Overlays.Inventory
{
    public partial class EquipPanel : GqButton
    {
        private readonly Sprite icon;
        private readonly SpriteText name;
        private EntityBase entityReference;
        private readonly Box backgroundBox;
        private readonly StarRatingContainer starRatingContainer;
        private Action clickAction;
        private TextureStore textureStore;
        private readonly InnerInventoryButton removeItemButton;
        private readonly InnerInventoryButton swapItemButton;
        private readonly FillFlowContainer infoContainer;
        private readonly Container infoContainer1;
        private readonly Container infoContainer2;
        private readonly Container infoContainer3;

        // TODO: Add indicators for stats
        public EquipPanel(EntityBase entity)
        {
            entityReference = entity;
            Size = new Vector2(420, 100);
            Margin = new MarginPadding(10);
            Masking = true;
            BorderColour = Colour4.Black;
            BorderThickness = 6;
            CornerExponent = 2;
            CornerRadius = 15;
            InternalChildren = new Drawable[]
            {
                backgroundBox = new Box
                {
                    Colour = ColourInfo.GradientVertical(
                        new Colour4(150, 150, 150, 255),
                        new Colour4(50, 50, 50, 255)
                    ),
                    RelativeSizeAxes = Axes.Both,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre
                },
                new Container
                {
                    Masking = true,
                    CornerExponent = 2,
                    CornerRadius = 8,
                    Anchor = Anchor.TopLeft,
                    Origin = Anchor.TopLeft,
                    Size = new Vector2(64),
                    Margin = new MarginPadding { Left = 12, Top = 12 },
                    Children = new Drawable[]
                    {
                        new Box
                        {
                            Colour = new Colour4(25, 25, 25, 50),
                            RelativeSizeAxes = Axes.Both
                        },
                        icon = new Sprite
                        {
                            RelativeSizeAxes = Axes.Both
                        }
                    }
                },
                starRatingContainer = new StarRatingContainer(1)
                {
                    Anchor = Anchor.BottomLeft,
                    Origin = Anchor.BottomLeft,
                    Scale = new Vector2(0.3f),
                    Y = -15,
                    X = 15
                },
                new Box
                {
                    RelativeSizeAxes = Axes.X,
                    Size = new Vector2(0.76f, 4),
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                    Y = -10,
                    X = 80,
                    Colour = Colour4.Black
                },
                new Container
                {
                    Anchor = Anchor.TopLeft,
                    Origin = Anchor.TopLeft,
                    Margin = new MarginPadding { Left = 80, Top = 24 },
                    Child = name = new SpriteText
                    {
                        Text = "Empty",
                        Anchor = Anchor.CentreLeft,
                        Origin = Anchor.CentreLeft,
                        Font = FontUsage.Default.With(size: 28)
                    }
                },
                new Container
                {
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                    Size = new Vector2(320, 40),
                    Margin = new MarginPadding { Left = 80 },
                    Y = 15,
                    Child = infoContainer = new FillFlowContainer
                    {
                        AutoSizeAxes = Axes.X,
                        Direction = FillDirection.Horizontal,
                        Spacing = new Vector2(10, 0),
                        Anchor = Anchor.CentreLeft,
                        Origin = Anchor.CentreLeft,
                        Children = new Drawable[]
                        {
                            infoContainer1 = new Container
                            {
                                AutoSizeAxes = Axes.X,
                            },
                            new Box
                            {
                                Origin = Anchor.Centre,
                                Colour = Colour4.Black,
                                Size = new Vector2(5, 40)
                            },
                            infoContainer2 = new Container { AutoSizeAxes = Axes.X },
                            new Box
                            {
                                Origin = Anchor.Centre,
                                Colour = Colour4.Black,
                                Size = new Vector2(5, 40)
                            },
                            infoContainer3 = new Container
                            {
                                AutoSizeAxes = Axes.X,
                                Child = new FillFlowContainer
                                {
                                    AutoSizeAxes = Axes.X,
                                    Direction = FillDirection.Horizontal,
                                    Spacing = new Vector2(10, 0),
                                    Children = new Drawable[]
                                    {
                                        removeItemButton = new InnerInventoryButton(new SpriteText { Text = "Remove", Origin = Anchor.Centre, Anchor = Anchor.Centre, Padding = new MarginPadding(5) })
                                        {
                                            Origin = Anchor.CentreLeft,
                                            AutoSizeAxes = Axes.Both
                                        },
                                        swapItemButton = new InnerInventoryButton(new SpriteText { Text = "Swap", Origin = Anchor.Centre, Anchor = Anchor.Centre, Padding = new MarginPadding(5) })
                                        {
                                            Origin = Anchor.CentreLeft,
                                            AutoSizeAxes = Axes.Both
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            starRatingContainer.Hide();

            if (entityReference == null) return;

            name.Text = entityReference.Name;
            starRatingContainer.starRating.Value = entity.StarRating.Value;
            backgroundBox.Hide();
            removeItemButton.Show();
            swapItemButton.Show();
            starRatingContainer.Show();
        }

        [BackgroundDependencyLoader]
        private void load(TextureStore textureStore)
        {
            this.textureStore = textureStore;

            if (entityReference?.TextureMapping != null)
            {
                icon.Texture = textureStore.Get(entityReference.TextureMapping.Get("Icon"));
            }
        }

        public void SetEquip(EntityBase entity)
        {
            infoContainer1.Clear();
            infoContainer2.Clear();

            SpriteText levelText = new SpriteText()
            {
                Text = "Level 0",
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre
            };
            infoContainer1.Add(levelText);

            if (entity != null)
            {
                entityReference = entity;
                if (entityReference.TextureMapping != null) icon.Texture = textureStore.Get(entityReference.TextureMapping.Get("Icon"));
                icon.Show();
                swapItemButton.Show();
                removeItemButton.Show();
                name.Text = entity.Name;
                starRatingContainer.starRating.Value = entity.StarRating.Value;
                levelText.Text = entity.Experience.Level.ToString();
                starRatingContainer.Show();

                switch (entity)
                {
                    case Weapon weapon:
                        infoContainer2.Add(
                            new SpriteText
                            {
                                Text = $"{weapon.Damage.Total().ToString()} Damage",
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre
                            }
                        );
                        break;

                    case Artifact artifact:
                        infoContainer2.Add(
                            new DrawableBuffIcon(artifact.MainAttribute)
                            {
                                Scale = new Vector2(0.5f),
                                Y = 5,
                                Margin = new MarginPadding { Left = 25, Right = 80 },
                            }
                        );
                        break;
                }
            }
            else
            {
                icon.Hide();
                swapItemButton.Hide();
                removeItemButton.Hide();
                starRatingContainer.Hide();
                name.Text = "Empty";
            }
        }

        public void SetSwapAction(Action action) => swapItemButton.SetAction(action);
        public void SetRemoveAction(Action action) => removeItemButton.SetAction(action);

        protected override bool OnClick(ClickEvent e)
        {
            clickAction?.Invoke();
            return base.OnClick(e);
        }

        protected override bool OnHover(HoverEvent e)
        {
            return base.OnHover(e);
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            base.OnHoverLost(e);
        }
    }
}
