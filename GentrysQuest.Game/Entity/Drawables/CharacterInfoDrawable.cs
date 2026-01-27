using System.Linq;
using GentrysQuest.Game.Overlays.Inventory;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK;

namespace GentrysQuest.Game.Entity.Drawables
{
    public partial class CharacterInfoDrawable : EntityInfoDrawable
    {
        public FillFlowContainer EquippedItemContainer { get; private set; }

        private readonly Vector2 size = new(64);

        public CharacterInfoDrawable(Character entity)
            : base(entity)
        {
            AddInternal(EquippedItemContainer = new FillFlowContainer
            {
                Name = "ArtifactContainer",
                Direction = FillDirection.Horizontal,
                AutoSizeAxes = Axes.Both,
                Anchor = Anchor.CentreRight,
                Origin = Anchor.CentreRight,
                Margin = new MarginPadding { Right = 30 }
            });

            var artifacts = entity.Artifacts.Get();

            EquippedItemContainer.Add(new EquipPanel(entity.Weapon)
            {
                Size = size,
                Margin = new MarginPadding { Right = 20 }
            });

            for (var index = 0; index < 5; index++)
            {
                var artifact = artifacts.ElementAtOrDefault(index);
                EquippedItemContainer.Add(new EquipPanel(artifact) { Size = size });
            }
        }
    }
}
