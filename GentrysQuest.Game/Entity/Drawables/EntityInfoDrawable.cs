using System;
using GentrysQuest.Game.Graphics;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Effects;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Input.Events;
using osuTK;

namespace GentrysQuest.Game.Entity.Drawables
{
    public partial class EntityInfoDrawable : GqButton
    {
        protected EntityIconDrawable icon;
        protected SpriteText name;
        protected SpriteText level;
        protected TextFlowContainer mainInfoContainer;
        public StarRatingContainer starRatingContainer;
        public EntityBase entity;
        protected Container BuffContainer;
        protected Box ColourBox;
        public bool IsSelected { get; private set; }
        public event EventHandler OnClickEvent;

        public EntityInfoDrawable(EntityBase entity)
        {
            this.entity = entity;

            RelativeSizeAxes = Axes.X;
            Origin = Anchor.TopCentre;
            Anchor = Anchor.TopCentre;
            CornerRadius = 0.2f;
            Margin = new MarginPadding(2);
            Size = new Vector2(0.8f, 100);
            CornerExponent = 2;
            CornerRadius = 15;
            Masking = true;
            BorderColour = Colour4.Black;
            BorderThickness = 2f;
            InternalChildren = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = new Colour4(25, 25, 25, 255)
                },
                new FillFlowContainer
                {
                    Direction = FillDirection.Horizontal,
                    AutoSizeAxes = Axes.Both,
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                    Children = new Drawable[]
                    {
                        new Container
                        {
                            Anchor = Anchor.CentreLeft,
                            Origin = Anchor.CentreLeft,
                            Size = new Vector2(60),
                            Child = icon = new EntityIconDrawable()
                        },
                        new FillFlowContainer
                        {
                            Direction = FillDirection.Vertical,
                            AutoSizeAxes = Axes.Both,
                            Anchor = Anchor.CentreLeft,
                            Origin = Anchor.CentreLeft,
                            Padding = new MarginPadding { Left = 5, Right = 20 },
                            Children = new Drawable[]
                            {
                                name = new SpriteText
                                {
                                    Text = entity.Name,
                                    Size = new Vector2(200, 35),
                                    Font = FontUsage.Default.With(size: 28),
                                    Truncate = true,
                                    AllowMultiline = false
                                },
                                starRatingContainer = new StarRatingContainer(entity.StarRating.Value)
                                {
                                    Size = new Vector2(200, 40)
                                }
                            }
                        },
                        level = new SpriteText
                        {
                            Text = entity.Experience.Level.ToString(),
                            Size = new Vector2(120, 35),
                            Anchor = Anchor.CentreLeft,
                            Origin = Anchor.CentreLeft,
                            Font = FontUsage.Default.With(size: 24),
                            AllowMultiline = false
                        },
                        BuffContainer = new Container
                        {
                            RelativeSizeAxes = Axes.Y,
                            Width = 64,
                        }
                    }
                },
                ColourBox = new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    RelativePositionAxes = Axes.Both,
                    Size = new Vector2(0.5f, 1),
                    Colour = ColourInfo.GradientHorizontal(new Colour4(0, 0, 0, 0), Colour4.White),
                    Anchor = Anchor.CentreRight,
                    Origin = Anchor.CentreRight,
                }
            };
            starRatingContainer.starRating.BindValueChanged(updateColorWithStarRating, true);
            entity.Experience.Level.Current.ValueChanged += delegate { level.Text = entity.Experience.Level.ToString(); };
        }

        [BackgroundDependencyLoader]
        private void load(TextureStore textures)
        {
            if (entity.TextureMapping != null) icon.Texture = textures.Get(entity.TextureMapping.Get("Icon"));
        }

        protected override bool OnHover(HoverEvent e)
        {
            this.ScaleTo(new Vector2(1.05f, 1f), 30);
            name.FadeColour(StarRatingContainer.GetColor(entity.StarRating.Value));
            name.ScaleTo(1.1f, 30);

            BorderColour = StarRatingContainer.GetColor(entity.StarRating.Value);
            return base.OnHover(e);
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            if (!IsSelected)
            {
                BorderColour = Colour4.Black;
                this.ScaleTo(new Vector2(1f, 1f), 30);
                name.FadeColour(Colour4.White, 30);
                name.ScaleTo(1f, 30);
            }

            base.OnHoverLost(e);
        }

        protected override bool OnClick(ClickEvent e)
        {
            this.FadeColour(Colour4.White);

            switch (IsSelected)
            {
                case true:
                    Unselect();
                    break;

                case false:
                    Select();
                    break;
            }

            OnClickEvent?.Invoke(this, null);

            return base.OnClick(e);
        }

        public void Select()
        {
            IsSelected = true;
            BorderColour = StarRatingContainer.GetColor(entity.StarRating.Value);
            EdgeEffect = new EdgeEffectParameters
            {
                Type = EdgeEffectType.Glow,
                Colour = StarRatingContainer.GetColor(entity.StarRating.Value),
                Radius = 10,
                Roundness = 3
            };
        }

        public void Unselect()
        {
            IsSelected = false;
            BorderColour = Colour4.Black;
            EdgeEffect = new EdgeEffectParameters();
            this.ScaleTo(new Vector2(1f, 1f), 30);
            name.FadeColour(Colour4.White, 30);
            name.ScaleTo(1f, 30);
        }

        private void updateColorWithStarRating(ValueChangedEvent<int> valueChangedEvent)
        {
            switch (valueChangedEvent.NewValue)
            {
                case 1:
                    // Colour = ColourInfo.GradientHorizontal(Colour4.White, Colour4.White);
                    ColourBox.Colour = ColourInfo.GradientHorizontal(new Colour4(0, 0, 0, 0), Colour4.Gray);
                    break;

                case 2:
                    // Colour = ColourInfo.GradientHorizontal(Colour4.White, Colour4.LimeGreen);
                    ColourBox.Colour = ColourInfo.GradientHorizontal(new Colour4(0, 0, 0, 0), Colour4.LimeGreen);
                    break;

                case 3:
                    // Colour = ColourInfo.GradientHorizontal(Colour4.White, Colour4.Aqua);
                    ColourBox.Colour = ColourInfo.GradientHorizontal(new Colour4(0, 0, 0, 0), Colour4.Aqua);
                    break;

                case 4:
                    // Colour = ColourInfo.GradientHorizontal(Colour4.DeepPink, Colour4.White);
                    ColourBox.Colour = ColourInfo.GradientHorizontal(new Colour4(0, 0, 0, 0), Colour4.DeepPink);
                    break;

                case 5:
                    // Colour = ColourInfo.GradientHorizontal(Colour4.Gold, Colour4.White);
                    ColourBox.Colour = ColourInfo.GradientHorizontal(new Colour4(0, 0, 0, 0), Colour4.Gold);
                    break;
            }
        }
    }
}
