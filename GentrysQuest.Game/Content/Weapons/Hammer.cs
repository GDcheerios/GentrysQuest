using GentrysQuest.Game.Entity.Weapon;
using osu.Framework.Graphics;
using osuTK;

namespace GentrysQuest.Game.Content.Weapons
{
    public class Hammer : Weapon
    {
        public override string Type => "Hammer";
        public override string Name { get; set; } = "Hammer";
        public override int Distance => 200;
        public override string Description { get; protected set; } = "Just a hammer";

        private readonly AttackAnimationRegistry attackAnimationRegistry = new AttackAnimationRegistry();

        private string nextAnimation = "first";

        public Hammer()
        {
            Damage.SetDefaultValue(100);

            TextureMapping.Add("Icon", "weapons_hammer.png");
            TextureMapping.Add("Base", "weapons_hammer.png");

            Vector2 size = new Vector2(0.8f, 1.2f);

            attackAnimationRegistry.RegisterAnimation("first");
            attackAnimationRegistry.AddKeyframe(new AttackKeyframe { Direction = 115, Distance = 100, Size = size });
            attackAnimationRegistry.AddKeyframe(new AttackKeyframe(100) { Direction = 100, Distance = 100, Size = size, DoesDamage = false, KnockbackMultiplier = 3 });
            attackAnimationRegistry.AddKeyframe(new AttackKeyframe(850) { Direction = -90, Distance = 100, Size = size, Transition = Easing.In, KnockbackMultiplier = 3 });
            attackAnimationRegistry.AddKeyframe(new AttackKeyframe(50) { Direction = -115, Distance = 100, Size = size, Transition = Easing.Out, KnockbackMultiplier = 3, Event = () => nextAnimation = "second"});

            attackAnimationRegistry.RegisterAnimation("second");
            attackAnimationRegistry.AddKeyframe(new AttackKeyframe { Direction = -115, Distance = 100, Size = size });
            attackAnimationRegistry.AddKeyframe(new AttackKeyframe(100) { Direction = -100, Distance = 100, Size = size, DoesDamage = false, KnockbackMultiplier = 3 });
            attackAnimationRegistry.AddKeyframe(new AttackKeyframe(850) { Direction = 90, Distance = 100, Size = size, Transition = Easing.In, KnockbackMultiplier = 3 });
            attackAnimationRegistry.AddKeyframe(new AttackKeyframe(50) { Direction = 115, Distance = 100, Size = size, Transition = Easing.Out, KnockbackMultiplier = 3, Event = () => nextAnimation = "first"});
        }
    }
}
