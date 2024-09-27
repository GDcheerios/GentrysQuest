// * Name              : GentrysQuest.Game
//  * Author           : Brayden J Messerschmidt
//  * Created          : 09/27/2024
//  * Course           : CIS 169 C#
//  * Version          : 1.0
//  * OS               : Windows 11 22H2
//  * IDE              : Jet Brains Rider 2023
//  * Copyright        : This is my work.
//  * Description      : desc.
//  * Academic Honesty : I attest that this is my original work.
//  * I have not used unauthorized source code, either modified or
//  * unmodified. I have not given other fellow student(s) access
//  * to my program.

using GentrysQuest.Game.Content.Effects;
using GentrysQuest.Game.Entity;
using GentrysQuest.Game.Entity.Weapon;
using GentrysQuest.Game.Utils;

namespace GentrysQuest.Game.Content.Skills;

public class FrisbeeThrow : Skill
{
    public override string Name { get; protected set; } = "Frisbee Throw";
    public override string Description { get; protected set; } = "Throws a frisbee from Frisbee Golf";
    public override double Cooldown { get; protected set; } = new Second(6);

    public override int MaxStack { get; protected set; } = 3;

    public override void Act()
    {
        base.Act();
        User.GetBase().AddProjectile(new ProjectileParameters
        {
            Speed = 15,
            PassthroughAmount = 1,
            Damage = (int)User.GetBase().Stats.Attack.GetPercentFromTotal(50f),
            OnHitEffects = [new OnHitEffect(1000) { Effect = new Stun(new Second(2)) }],
            Lifetime = new Second(5)
        });
    }
}
