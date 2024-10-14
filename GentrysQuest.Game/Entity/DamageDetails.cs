using System.Collections.Generic;

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
        /// Current effects applied to this hit
        /// </summary>
        public List<StatusEffect> StatusEffects = new List<StatusEffect>();

        public int GetHitAmount() => Sender.EnemyHitCounter[Receiver];
    }
}
