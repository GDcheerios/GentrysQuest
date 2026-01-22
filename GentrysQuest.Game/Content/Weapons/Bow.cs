using GentrysQuest.Game.Content.Effects;
using GentrysQuest.Game.Entity;
using GentrysQuest.Game.Entity.Weapon;

namespace GentrysQuest.Game.Content.Weapons
{
    public class Bow : Weapon
    {
        public override string Type => "Bow";
        public override int Distance => 1000;
        public override string Name { get; set; } = "Bow";
        public override string Description { get; protected set; } = "Just a bow.";
        public override bool IsGeneralDamageMode => false;

        private readonly AttackAnimationRegistry animationRegistry = new();
        public override AttackKeyframe RestingEvent { get; protected set; } = new AttackKeyframe(100) { Distance = 100 };

        private int currentDamage = 10;
        private int projectileSpeed = 15;

        private string currentAnimation = "aim1";

        public Bow()
        {
            TextureMapping = new();

            animationRegistry.RegisterAnimation("aim1");
            animationRegistry.AddKeyframe(new AttackKeyframe(200) { Distance = 80 });

            animationRegistry.RegisterAnimation("aim2");
            animationRegistry.AddKeyframe(new AttackKeyframe(300) { Distance = 60 });

            animationRegistry.RegisterAnimation("aim3");
            animationRegistry.AddKeyframe(new AttackKeyframe(500) { Distance = 40 });

            animationRegistry.RegisterAnimation("aim4");
            animationRegistry.AddKeyframe(new AttackKeyframe(10) { Distance = 20 });

            animationRegistry.RegisterAnimation("aim5");
            animationRegistry.AddKeyframe(new AttackKeyframe(10) { Distance = 20 });
        }

        private void playAnimation(string animation)
        {
            if (currentAnimation == animation) return;

            currentAnimation = animation;
            DrawableInstance.StopAnimation();
            DrawableInstance.PlayAnimation(animationRegistry.GetAnimation(animation));
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (!IsClicking) return;

            playAnimation("aim1");

            switch (HoldDuration())
            {
                case > 2000:
                    playAnimation("aim4");
                    currentDamage = 85;
                    projectileSpeed = 40;
                    break;

                case > 1000:
                    playAnimation("aim3");
                    currentDamage = 45;
                    projectileSpeed = 30;
                    break;

                case > 500:
                    playAnimation("aim2");
                    currentDamage = 30;
                    projectileSpeed = 25;
                    break;

                case > 200:
                    currentDamage = 20;
                    projectileSpeed = 20;
                    break;
            }
        }

        public override void OnRelease()
        {
            if (!IsClicking) return;

            base.OnRelease();
            AttackAnimation shootAnimation = new AttackAnimation();
            shootAnimation.AddEvent(new AttackKeyframe
            {
                Distance = 50,
                Projectiles =
                [
                    new ProjectileParameters
                    {
                        Damage = currentDamage,
                        Speed = projectileSpeed,
                        PassthroughAmount = 1,
                    }
                ]
            });
            shootAnimation.AddEvent(new AttackKeyframe(100) { Distance = 100 });
            Holder.AddEffect(new Disarm(300));
            DrawableInstance.StopAnimation();
            DrawableInstance.PlayAnimation(shootAnimation);
            currentDamage = 20;
            projectileSpeed = 15;
        }
    }
}
