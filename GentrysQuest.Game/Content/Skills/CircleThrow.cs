using GentrysQuest.Game.Entity;
using GentrysQuest.Game.Utils;

namespace GentrysQuest.Game.Content.Skills
{
    public class CircleThrow : Skill
    {
        public override string Name { get; protected set; } = "Circle Throw";
        public override string Description { get; protected set; } = "Brayden's amazing secondary skill";
        public override double Cooldown { get; protected set; } = new Second(2.5);

        public override void Act()
        {
            base.Act();

            User.GetBase().AddProjectile(new ProjectileParameters
            {
                Speed = 20,
                PassthroughAmount = 2,
                Lifetime = new Second(0.4),
                Damage = (int)User.GetBase().Stats.Attack.GetPercentFromTotal(120)
            });
        }
    }
}
