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
        private AttackKeyframe firstResting;
        private AttackKeyframe secondResting;

        public Hammer()
        {
            Damage.SetDefaultValue(100);

            TextureMapping.Add("Icon", "weapons_hammer.png");
            TextureMapping.Add("Base", "weapons_hammer.png");

            Vector2 size = new Vector2(0.8f, 1.2f);

            attackAnimationRegistry.RegisterAnimation("first");
            attackAnimationRegistry.AddKeyframe(new AttackKeyframe { Direction = 115, Distance = 100, Size = size, KnockbackMultiplier = 3 });
            attackAnimationRegistry.AddKeyframe(new AttackKeyframe(1000)
            {
                Direction = -115, Distance = 100, Event = () =>
                {
                    nextAnimation = "second";
                    RestingEvent = firstResting;
                },
                KnockbackMultiplier = 3,
                Transition = Easing.In
            });
            firstResting = new AttackKeyframe(200)
            {
                Direction = -115, Distance = 100
            };

            attackAnimationRegistry.AddKeyframe(firstResting);

            attackAnimationRegistry.RegisterAnimation("second");
            attackAnimationRegistry.AddKeyframe(new AttackKeyframe
            {
                Direction = -115, Distance = 100, Size = size, KnockbackMultiplier = 3
            });
            attackAnimationRegistry.AddKeyframe(new AttackKeyframe(1000)
            {
                Direction = 115, Distance = 100, Event = () =>
                {
                    nextAnimation = "first";
                    RestingEvent = secondResting;
                },
                KnockbackMultiplier = 3,
                Transition = Easing.In
            });
            secondResting = new AttackKeyframe(200) { Direction = 115, Distance = 100 };

            attackAnimationRegistry.AddKeyframe(secondResting);
        }

        public override void OnClick(float direction)
        {
            base.OnClick(direction);
            DrawableInstance.PlayAnimation(attackAnimationRegistry.GetAnimation(nextAnimation));
        }
    }
}
