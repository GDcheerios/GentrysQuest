using System.Collections.Generic;
using GentrysQuest.Game.Entity.Weapon;
using GentrysQuest.Game.Graphics.TextStyles;
using GentrysQuest.Game.Utils;

namespace GentrysQuest.Game.Entity
{
    public class ProjectileParameters
    {
        /// <summary>
        /// Design of the projectile
        /// </summary>
        public CustomSprite Design;

        /// <summary>
        /// Speed of the projectile
        /// </summary>
        public double Speed = 1;

        /// <summary>
        /// Direction of the projectile
        /// </summary>
        public double Direction = 0;

        /// <summary>
        /// How long the projectile will last
        /// </summary>
        public double Lifetime = new Second(5);

        /// <summary>
        /// Hitbox of the projectile
        /// </summary>
        public HitBox HitBox;

        /// <summary>
        /// The amount of times the projectile can pass through enemies
        /// </summary>
        public int PassthroughAmount = 0;

        /// <summary>
        /// projectile damage
        /// </summary>
        public int Damage;

        /// <summary>
        /// How it affects
        /// </summary>
        public List<OnHitEffect> OnHitEffects = null;

        /// <summary>
        /// If the damage takes defense into account
        /// </summary>
        public bool TakesDefense = true;

        /// <summary>
        /// If the damage takes weapon damage into account
        /// </summary>
        public bool TakesNormalDamage = true;

        /// <summary>
        /// If the damage takes holder damage into account
        /// </summary>
        public bool TakesHolderDamage = true;
    }
}
