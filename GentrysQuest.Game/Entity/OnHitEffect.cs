using GentrysQuest.Game.Utils;

namespace GentrysQuest.Game.Entity.Weapon
{
    /// <summary>
    /// The effect that may apply on hit
    /// </summary>
    /// <param name="chance">represents the chance of succession. 1.0 = 100%</param>
    public class OnHitEffect(float chance = 1)
    {
        /// <summary>
        /// The effect
        /// </summary>
        public StatusEffect Effect;

        /// <summary>
        /// Chance of succession;
        /// </summary>
        public float Chance = chance;

        /// <summary>
        /// If the effects will apply
        /// Resets the effect beforehand
        /// </summary>
        /// <returns>boolean</returns>
        public bool Applies()
        {
            Effect.Reset();
            return MathBase.IsChanceSuccessful(Chance);
        }
    }
}
