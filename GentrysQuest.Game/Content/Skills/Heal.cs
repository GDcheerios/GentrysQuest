using GentrysQuest.Game.Entity;
using GentrysQuest.Game.Utils;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;

namespace GentrysQuest.Game.Content.Skills
{
    public class Heal : Skill
    {
        public override string Name { get; protected set; } = "Heal";
        public override string Description { get; protected set; } = "Heals 10% of your HP";
        public override double Cooldown { get; protected set; } = new Second(7);

        protected override void SkillDo()
        {
            int healAmount = (int)User.GetBase().Stats.Health.GetPercentFromTotal(10);
            User.GetBase().Heal(healAmount);
            User.GetBase().DisplayHealthEvent($"{healAmount}", ColourInfo.GradientVertical(Colour4.Lime, Colour4.Green));
        }
    }
}
