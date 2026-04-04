using System.Collections.Generic;

namespace GentrysQuest.Game.Entity
{
    /// <summary>
    /// Used to keep track of damage so you don't repeatedly strike enemies in the frame.
    /// </summary>
    public class DamageQueue
    {
        private readonly List<HitBox> hitBoxes = new();

        /// <summary>
        /// Add a hitbox to the queue
        /// </summary>
        /// <param name="hitBox">The entity queue</param>
        public void Add(HitBox hitBox) => hitBoxes.Add(hitBox);

        /// <summary>
        /// Remove the hitbox from the queue
        /// </summary>
        /// <param name="hitBox">The entity hitbox</param>
        public void Remove(HitBox hitBox) => hitBoxes.Remove(hitBox);

        /// <summary>
        /// Check if the hitbox is registered in the queue
        /// </summary>
        /// <param name="hitBox">The entity hitbox</param>
        /// <returns>If the hitbox is inside the queue</returns>
        public bool Check(HitBox hitBox) => hitBoxes.Contains(hitBox);

        /// <summary>
        /// Clear the damage queue
        /// </summary>
        public void Clear() => hitBoxes.Clear();
    }
}
