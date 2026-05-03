using GentrysQuest.Game.Entity;
using GentrysQuest.Game.Entity.AI;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osuTK;

namespace GentrysQuest.Game.Content.Enemies
{
    public class LostSpirit : Enemy
    {
        public LostSpirit()
        {
            Name = "Lost Spirit";
            Description = "A lost spirit";
            AiProfile = AiProfile.Defensive();

            DrawableTexture = new DrawableTexture
            {
                Child = new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(0.5f),
                    Colour = Colour4.Gray,
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre
                }
            };
        }

        public override void UpdateStats()
        {
            base.UpdateStats();
            Stats.Health.SetDefaultValue(100);
            Stats.Speed.SetDefaultValue(0.25);
        }
    }
}
