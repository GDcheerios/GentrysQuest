using GentrysQuest.Game.Entity.Drawables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK;

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
                    Child = skillContainer = new FillFlowContainer
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        AutoSizeAxes = Axes.X,
                        RelativeSizeAxes = Axes.Y,
                        Direction = FillDirection.Horizontal,
                        Spacing = new Vector2(25, 0)
                    }
                }
            };
        }

        public void SetUpSkills(Entity.Entity entity)
        {
            skillContainer.Add(new SkillDrawable(entity.Weapon?.SkillRef, "M1"));
            skillContainer.Add(new SkillDrawable(entity.Secondary, "M2"));
            skillContainer.Add(new SkillDrawable(entity.Utility, "Space"));
            skillContainer.Add(new SkillDrawable(entity.Ultimate, "R"));
        }

        public void ClearSkills() => skillContainer.Clear();
    }
}
