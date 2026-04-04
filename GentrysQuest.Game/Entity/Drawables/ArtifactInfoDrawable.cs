using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK;

namespace GentrysQuest.Game.Entity.Drawables
{
    public partial class ArtifactInfoDrawable : EntityInfoDrawable
    {
        private readonly FillFlowContainer buffsListContainer;

        public ArtifactInfoDrawable(Artifact entity)
            : base(entity)
        {
            initializeMainAttribute(entity);
            buffsListContainer = createBuffsListContainer();
            AddInternal(buffsListContainer);
            addAttributeBuffIcons(entity);
        }

        private void initializeMainAttribute(Artifact entity)
        {
            BuffContainer.Add(new DrawableBuffIcon(entity.MainAttribute));
        }

        private FillFlowContainer createBuffsListContainer()
        {
            return new FillFlowContainer
            {
                Direction = FillDirection.Vertical,
                Anchor = Anchor.CentreRight,
                Origin = Anchor.CentreRight,
                AutoSizeAxes = Axes.Both,
                Padding = new MarginPadding { Right = 12 },
            };
        }

        private void addAttributeBuffIcons(Artifact entity)
        {
            foreach (Buff attribute in entity.Attributes)
            {
                buffsListContainer.Add(new DrawableBuffIcon(attribute, true)
                {
                    Size = new Vector2(16)
                });
            }
        }
    }
}
