using GentrysQuest.Game.Content.Effects;
using GentrysQuest.Game.Entity;
using GentrysQuest.Game.Entity.Weapon;
using GentrysQuest.Game.Utils;
using osu.Framework.Graphics;
using osuTK;

namespace GentrysQuest.Game.Content.Weapons
{
    public class BrodysBroadsword : Weapon
    {
        public override string Name { get; set; } = "Brody's Broadsword";
        public override string Type => "Broadsword";

        public override string Description { get; protected set; } =
            "Brody the mighty warrior's broadsword. The weapon was wielded for centuries by Brody himself, but was lost when the great calamity struck and he lost his life to the invading Waifu's."
            + "\nThe [condition]first hit[/condition] on an [type]enemy[/type] deals an extra [unit]1%[/unit] [details]+ 2.5 each difficulty[/details] of their health. "
            + "\nMoves: "
            + "\nSlash - normal attack. "
            + "\nHilt attack - stuns enemies for [unit]0.5 seconds[/unit]. ";

        public override StarRating StarRating { get; protected set; } = new StarRating(1);

        public override int Distance => 200;

        private readonly AttackAnimationRegistry attackAnimationRegistry = new AttackAnimationRegistry();

        private string nextAnimation = "first";

        public BrodysBroadsword()
        {
            Damage.SetDefaultValue(24);

            #region Design

            Origin = Anchor.BottomCentre;

            #endregion

            #region AttackPattern

            var distance = 0.35f;
            var time = new Second(0.75f);
            OnHitEffect hiltAttack = new OnHitEffect
            {
                Effect = new Stun()
            };

            attackAnimationRegistry.RegisterAnimation("first");
            attackAnimationRegistry.AddKeyframe(new AttackKeyframe { Direction = 110, Distance = distance, DamagePercent = 15 });
            attackAnimationRegistry.AddKeyframe(new AttackKeyframe(time) { Direction = -75, Distance = distance, Transition = Easing.InCubic, DamagePercent = 15, Event = () => nextAnimation = "second"});

            attackAnimationRegistry.RegisterAnimation("second");
            attackAnimationRegistry.AddKeyframe(new AttackKeyframe { Direction = -75, Distance = distance, DamagePercent = 30 });
            attackAnimationRegistry.AddKeyframe(new AttackKeyframe(new Second(0.2)) { Direction = -110, Distance = distance, Transition = Easing.OutCubic, DamagePercent = 30 });
            attackAnimationRegistry.AddKeyframe(new AttackKeyframe(time) { Direction = 75, Distance = distance, Transition = Easing.InCubic, DamagePercent = 30, Event = () => nextAnimation = "third" });

            Vector2 boxSize = new Vector2(0);

            attackAnimationRegistry.RegisterAnimation("third");
            attackAnimationRegistry.AddKeyframe(new AttackKeyframe { Direction = 75, MovementSpeed = 0.2f, HitboxSize = boxSize });
            attackAnimationRegistry.AddKeyframe(new AttackKeyframe(new Second(0.2)) { Direction = 180, MovementSpeed = 0.1f, HitboxSize = boxSize });
            attackAnimationRegistry.AddKeyframe(new AttackKeyframe(new Second(0.2)) { Direction = 180, MovementSpeed = 0, HitboxSize = boxSize });
            attackAnimationRegistry.AddKeyframe(new AttackKeyframe(new Second(0.1))
                { Direction = 180, Position = new Vector2(0, -100), MovementSpeed = 0.1f, ResetHitBox = true, OnHitEffects = [hiltAttack], Event = () => nextAnimation = "first" });

            #endregion

            #region cases

            OnHitEntity += details =>
            {
                Entity.Entity receiver = details.Receiver;
                // if (details.GetHitAmount() == 1) receiver.Damage((int)receiver.Stats.Health.GetPercentFromTotal((float)(1 + Holder.Difficulty * 2.5)));
            };

            #endregion

            #region TextureMapping

            TextureMapping = new();
            TextureMapping.Add("Icon", "weapons_brodys_broadsword.png");
            TextureMapping.Add("Base", "weapons_brodys_broadsword.png");

            #endregion
        }
    }
}
