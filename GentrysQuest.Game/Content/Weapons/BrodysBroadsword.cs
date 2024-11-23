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
        public override string Type { get; } = "Broadsword";

        public override string Description { get; protected set; } =
            "Brody the mighty warrior's broadsword. The weapon was wielded for centuries by Brody himself, but was lost when the great calamity struck and he lost his life to the invading Waifu's. "
            + "The first hit on an enemy deals an extra 1% + 2.5 each difficulty of their health. "
            + "Moves: "
            + "Slash - normal attack. "
            + "Hilt attack - stuns enemies for 0.5 seconds. ";

        public override StarRating StarRating { get; protected set; } = new StarRating(1);

        public override int Distance { get; set; } = 200;

        private readonly AttackPattern attackPattern = new AttackPattern();

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

            attackPattern.AddCase(1);
            attackPattern.Add(new AttackPatternEvent { Direction = 110, Distance = distance, DamagePercent = 15 });
            attackPattern.Add(new AttackPatternEvent(time) { Direction = -75, Distance = distance, Transition = Easing.InCubic, DamagePercent = 15 });

            attackPattern.AddCase(2);
            attackPattern.Add(new AttackPatternEvent { Direction = -75, Distance = distance, DamagePercent = 30 });
            attackPattern.Add(new AttackPatternEvent(new Second(0.2)) { Direction = -110, Distance = distance, Transition = Easing.OutCubic, DamagePercent = 30 });
            attackPattern.Add(new AttackPatternEvent(time) { Direction = 75, Distance = distance, Transition = Easing.InCubic, DamagePercent = 30 });

            Vector2 boxSize = new Vector2(0);

            attackPattern.AddCase(3);
            attackPattern.Add(new AttackPatternEvent { Direction = 75, MovementSpeed = 0.2f, HitboxSize = boxSize });
            attackPattern.Add(new AttackPatternEvent(new Second(0.2)) { Direction = 180, MovementSpeed = 0.1f, HitboxSize = boxSize });
            attackPattern.Add(new AttackPatternEvent(new Second(0.2)) { Direction = 180, MovementSpeed = 0, HitboxSize = boxSize });
            attackPattern.Add(new AttackPatternEvent(new Second(0.1))
                { Direction = 180, Position = new Vector2(0, -100), MovementSpeed = 0.1f, ResetHitBox = true, OnHitEffects = [hiltAttack] });

            #endregion

            #region cases

            OnHitEntity += details =>
            {
                Entity.Entity receiver = details.Receiver;
                if (details.GetHitAmount() == 1) receiver.Damage((int)receiver.Stats.Health.GetPercentFromTotal((float)(1 + Holder.Difficulty * 2.5)));
            };

            #endregion

            #region TextureMapping

            TextureMapping.Add("Icon", "weapons_brodys_broadsword.png");
            TextureMapping.Add("Base", "weapons_brodys_broadsword.png");

            #endregion
        }
    }
}
