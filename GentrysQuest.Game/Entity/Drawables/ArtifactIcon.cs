using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osuTK;
using GentrysQuest.Game.Content.Artifacts;

namespace GentrysQuest.Game.Entity.Drawables
{
    public partial class ArtifactIcon : CompositeDrawable
    {
        private readonly Sprite icon;
        private readonly Container randomIconContainer;
        private DrawableBuffIcon? buffIcon;
        private readonly Artifact? entityReference;
        private readonly StarRatingContainer starRatingContainer;
        private TextureStore? textureStore;

        public ArtifactIcon(Artifact? entity)
        {
            entityReference = entity;

            Size = new Vector2(35);
            Origin = Anchor.TopCentre;
            Margin = new MarginPadding(10);

            InternalChildren =
            [
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Colour4.Black.Opacity(0.2f),
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre
                },
                icon = new Sprite
                {
                    RelativeSizeAxes = Axes.Both,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre
                },
                randomIconContainer = new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre
                },
                starRatingContainer = new StarRatingContainer(1)
                {
                    Anchor = Anchor.TopLeft,
                    Origin = Anchor.TopCentre,
                    Scale = new Vector2(0.175f)
                }
            ];

            if (entityReference != null)
            {
                AddInternal(buffIcon = new DrawableBuffIcon(entityReference.MainAttribute, true)
                {
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(0.7f),
                    Anchor = Anchor.BottomCentre,
                    Origin = Anchor.BottomCentre
                });

                starRatingContainer.starRating.Value = entityReference.StarRating.Value;
            }
            else
            {
                starRatingContainer.starRating.Value = 0;
                icon.Colour = Colour4.Gray.Opacity(0.35f);
            }

            Show();
        }

        [BackgroundDependencyLoader]
        private void load(TextureStore textureStore)
        {
            this.textureStore = textureStore;

            if (entityReference is RandomArtifact randomArtifact)
            {
                icon.Texture = null;
                randomIconContainer.Clear();

                foreach (var shape in randomArtifact.IconShapes)
                {
                    randomIconContainer.Add(new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        RelativePositionAxes = Axes.Both,
                        Size = shape.RelativeSize,
                        Position = shape.RelativePosition - (shape.RelativeSize / 2f),
                        Origin = Anchor.Centre,
                        Anchor = Anchor.TopLeft,
                        Colour = shape.Colour,
                        Rotation = shape.Rotation
                    });
                }
            }
            else if (entityReference != null)
                icon.Texture = textureStore.Get(entityReference.TextureMapping.Get("Icon"));
            else
                icon.Texture = null;
        }
    }
}
