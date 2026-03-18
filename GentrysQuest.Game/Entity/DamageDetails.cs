using System.Collections.Generic;
using osuTK;

namespace GentrysQuest.Game.Entity
{
    public class DamageDetails
    {
        /// <summary>
        /// If the attack was a critical hit
        /// </summary>
        public bool IsCrit = false;

        /// <summary>
        /// The amount of damage from the attack
        /// </summary>
        public int Damage = 0;

        /// <summary>
        /// The sender of the attack
        /// </summary>
        public Entity Sender = null;

        /// <summary>
        /// The receiver of the attack
        /// </summary>
        public Entity Receiver = null;

        /// <summary>
        /// Where the receiver was hit from
        /// </summary>
        public Vector2 HitFromPosition = Vector2.Zero;

        /// <summary>
        /// Where the receiver was hit at
        /// </summary>
        public Vector2 HitAtPosition = Vector2.Zero;

        /// <summary>
        /// Current effects applied to this hit
        /// </summary>
        public List<StatusEffect> StatusEffects = new();

        /// <summary>
        /// If this attack ignores defense
        /// </summary>
        public bool IgnoreDefense = false;

        /// <summary>
        /// If the attack was dodged or not.
        /// </summary>
        public bool WasDodged = false;

        /// <summary>
        /// Get the amount of times the sender has attacked the receiver
        /// </summary>
        /// <returns>The hit count of Receiver</returns>
        public int GetHitAmount() => Sender.EnemyHitCounter[Receiver];
    }
}
