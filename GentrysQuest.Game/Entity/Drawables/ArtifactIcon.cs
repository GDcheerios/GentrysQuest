using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osuTK;

namespace GentrysQuest.Game.Entity.Drawables
{
    public partial class ArtifactIcon : CompositeDrawable
    {
        private readonly Sprite icon;
        private DrawableBuffIcon buffIcon;
        private Artifact entityReference;
        private readonly StarRatingContainer starRatingContainer;
        private TextureStore textureStore;

        public ArtifactIcon(Artifact entity)
        {
            entityReference = entity;
            Size = new Vector2(35);
            Origin = Anchor.TopCentre;
            Margin = new MarginPadding(10);
            InternalChildren = new Drawable[]
            {
                icon = new Sprite
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
                },
            };
            AddInternal(buffIcon = new DrawableBuffIcon(entity.MainAttribute, true)
            {
                RelativeSizeAxes = Axes.Both,
                Size = new Vector2(0.7f),
                Anchor = Anchor.BottomCentre,
                Origin = Anchor.BottomCentre
            });
            starRatingContainer.starRating.Value = entity.StarRating.Value;
            Show();
        }

        [BackgroundDependencyLoader]
        private void load(TextureStore textureStore)
        {
            this.textureStore = textureStore;
            if (entityReference != null) icon.Texture = textureStore.Get(entityReference.TextureMapping.Get("Icon"));
        }
    }
}
