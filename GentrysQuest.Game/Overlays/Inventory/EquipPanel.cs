using System;
using GentrysQuest.Game.Entity;
using GentrysQuest.Game.Entity.Drawables;
using GentrysQuest.Game.Entity.Weapon;
using GentrysQuest.Game.Graphics;
using JetBrains.Annotations;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osuTK;

namespace GentrysQuest.Game.Overlays.Inventory
{
    public partial class EquipPanel : GqButton
    {
        private Sprite icon;
        private SpriteText name;
        private readonly EntityBase entityReference;
        private StarRatingContainer starRatingContainer;
        private string nameRef;
        private int starRating;
        private string textureRef;
        private RemoveButton removeButton;
        private SwapButton swapButton;

        public EquipPanel([CanBeNull] EntityBase entity)
        {
            entityReference = entity;
            nameRef = "Empty";
            starRating = 0;
            textureRef = "huh.png";
            swapButton = new SwapButton
            {
                Anchor = Anchor.BottomCentre,
                Origin = Anchor.TopCentre,
                Margin = new MarginPadding { Top = 10 },
                X = -15,
                Alpha = 0
            };
            removeButton = new RemoveButton
            {
                Anchor = Anchor.BottomCentre,
                Origin = Anchor.TopCentre,
                Margin = new MarginPadding { Top = 10 },
                X = 15,
                Alpha = 0
            };

            if (entity == null) return;

            nameRef = entity.Name;
            starRating = entity.StarRating;
            textureRef = entity.TextureMapping?.Get("Icon");
            starRatingContainer = new StarRatingContainer(starRating)
            {
                Anchor = Anchor.BottomLeft,
                Origin = Anchor.BottomLeft,
                X = 20,
                Scale = new Vector2(0.25f)
            };
            swapButton.Alpha = 1;
            removeButton.Alpha = 1;
        }

        [BackgroundDependencyLoader]
        private void load(TextureStore store)
        {
            Size = new Vector2(84);
            InternalChildren =
            [
                icon = new Sprite
                {
                    RelativeSizeAxes = Axes.Both,
                    Texture = store.Get(textureRef),
                },
                new Container
                {
                    Masking = true,
                    CornerExponent = 2,
                    CornerRadius = 10,
                    RelativeSizeAxes = Axes.X,
                    Height = 16,
                    Children =
                    [
                        new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Colour = new Colour4(0, 0, 0, 0.5f)
                        },
                        name = new SpriteText
                        {
                            Text = nameRef,
                            Anchor = Anchor.TopCentre,
                            Origin = Anchor.TopCentre,
                            Font = FontUsage.Default.With(size: 14)
                        }
                    ]
                },
                removeButton,
                swapButton
            ];

            if (starRatingContainer != null) Add(starRatingContainer);

            switch (entityReference)
            {
                case Artifact artifact:
                    Add(new DrawableBuffIcon(artifact.MainAttribute, true)
                    {
                        Anchor = Anchor.BottomRight,
                        Origin = Anchor.BottomRight,
                        Scale = new Vector2(0.7f)
                    });
                    break;

                case Weapon weapon:
                    Add(new DrawableBuffIcon(weapon.Buff, true)
                    {
                        Anchor = Anchor.BottomRight,
                        Origin = Anchor.BottomRight,
                        Scale = new Vector2(0.7f)
                    });
                    break;
            }
        }

        public void SetSwapAction(Action action) => swapButton.SetAction(action);
        public void SetRemoveAction(Action action) => removeButton.SetAction(action);
    }
}
