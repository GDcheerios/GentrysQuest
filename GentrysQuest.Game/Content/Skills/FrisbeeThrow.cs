using GentrysQuest.Game.Content.Effects;
using GentrysQuest.Game.Entity;
using GentrysQuest.Game.Entity.Weapon;
using GentrysQuest.Game.Utils;

namespace GentrysQuest.Game.Content.Skills
{
    public class FrisbeeThrow : Skill
    {
        public override string Name { get; protected set; } = "Frisbee Throw";
        public override string Description { get; protected set; } = "Throws a frisbee from Frisbee Golf";
        public override double Cooldown { get; protected set; } = new Second(1);

        public override int MaxStack { get; protected set; } = 3;

        protected override void SkillDo()
        {
            User.GetBase().AddProjectile(new ProjectileParameters
            {
                Speed = 15,
                PassthroughAmount = 2,
                Damage = (int)User.GetBase().Stats.Attack.GetPercentFromTotal(50f),
                OnHitEffects = [new OnHitEffect(1000) { Effect = new Stun(new Second(2)) }],
                Lifetime = new Second(5)
            });
        }
    }
}
