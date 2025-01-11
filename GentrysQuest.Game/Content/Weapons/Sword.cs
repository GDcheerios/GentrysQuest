using GentrysQuest.Game.Entity.Weapon;
using osu.Framework.Graphics;
using osuTK;

namespace GentrysQuest.Game.Content.Weapons
{
    public class Sword : Weapon
    {
        public override string Name { get; set; } = "Sword";
        public override string Type => "Sword";
        public override int Distance => 250;
        public override string Description { get; protected set; } = "Just a sword";

        private readonly AttackAnimationRegistry attackAnimationRegistry = new AttackAnimationRegistry();

        private string nextAnimation = "first";

        private AttackKeyframe firstRestingKeyFrame;
        private AttackKeyframe secondRestingKeyFrame;

        public Sword()
        {
            TextureMapping.Add("Icon", "weapons_sword.png");
            TextureMapping.Add("Base", "weapons_sword.png");

            Damage.SetDefaultValue(22);

            Vector2 size = new Vector2(0.25f, 1);

            attackAnimationRegistry.RegisterAnimation("first");
            attackAnimationRegistry.AddKeyframe(new AttackKeyframe { Direction = 115, Distance = 100, Size = size });
            attackAnimationRegistry.AddKeyframe(new AttackKeyframe(650) { Direction = -90, Distance = 100, Size = size, Transition = Easing.In, KnockbackMultiplier = 1.5f });
            firstRestingKeyFrame = new AttackKeyframe(100)
            {
                Direction = -115, Distance = 100, Size = size, Transition = Easing.Out, KnockbackMultiplier = 1.5f, Event = () =>
                {
                    nextAnimation = "second";
                    RestingEvent = firstRestingKeyFrame;
                }
            };
            attackAnimationRegistry.AddKeyframe(firstRestingKeyFrame);

            attackAnimationRegistry.RegisterAnimation("second");
            attackAnimationRegistry.AddKeyframe(new AttackKeyframe { Direction = -115, Distance = 100, Size = size });
            attackAnimationRegistry.AddKeyframe(new AttackKeyframe(650) { Direction = 90, Distance = 100, Size = size, Transition = Easing.In, KnockbackMultiplier = 1.5f });
            secondRestingKeyFrame = new AttackKeyframe(100)
            {
                Direction = 115, Distance = 100, Size = size, Transition = Easing.Out, KnockbackMultiplier = 1.5f, Event = () =>
                {
                    nextAnimation = "first";
                    RestingEvent = secondRestingKeyFrame;
                }
            };
            attackAnimationRegistry.AddKeyframe(secondRestingKeyFrame);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            DrawableInstance.PlayAnimation(attackAnimationRegistry.GetAnimation(nextAnimation));
        }
    }
}
