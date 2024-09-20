using GentrysQuest.Game.Entity.Drawables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;

namespace GentrysQuest.Game.Overlays.SkillOverlay
{
    public partial class SkillOverlay : CompositeDrawable
    {
        private readonly FillFlowContainer skillContainer;

        public SkillOverlay()
        {
            InternalChildren = new Drawable[]
            {
                new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Masking = true,
                    CornerRadius = 6,
                    CornerExponent = 2,
                    Children = new Drawable[]
                    {
                        new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Colour = new Colour4(0, 0, 0, 65)
                        },
                        skillContainer = new FillFlowContainer
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            AutoSizeAxes = Axes.Both,
                            Direction = FillDirection.Horizontal
                        }
                    }
                }
            };
        }

        public void SetUpSkills(Entity.Entity entity)
        {
            skillContainer.Add(new SkillDrawable(entity.Weapon?.SkillRef));
            skillContainer.Add(new SkillDrawable(entity.Secondary));
            skillContainer.Add(new SkillDrawable(entity.Utility));
            skillContainer.Add(new SkillDrawable(entity.Ultimate));
        }

        public void ClearSkills() => skillContainer.Clear();
    }
}
