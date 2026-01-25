using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;

namespace GentrysQuest.Game.Entity.Drawables
{
    public partial class CharacterInfoDrawable : EntityInfoDrawable
    {
        public FillFlowContainer ArtifactContainer { get; private set; }

        public CharacterInfoDrawable(Character entity)
            : base(entity)
        {
            AddInternal(ArtifactContainer = new FillFlowContainer
            {
                Name = "ArtifactContainer",
                Direction = FillDirection.Horizontal,
                AutoSizeAxes = Axes.Both,
                Anchor = Anchor.CentreRight,
                Origin = Anchor.CentreRight,
                Margin = new MarginPadding { Right = 30 }
            });

            var artifacts = entity.Artifacts.Get();

            for (var index = 0; index < 5; index++)
            {
                var artifact = artifacts[index];
                ArtifactIcon anIcon = new ArtifactIcon(artifact);
                ArtifactContainer.Add(anIcon);
            }
        }
    }
}
