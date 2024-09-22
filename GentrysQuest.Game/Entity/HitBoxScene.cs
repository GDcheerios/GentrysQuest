using System.Collections.Generic;
using System.Linq;

namespace GentrysQuest.Game.Entity
{
    /// <summary>
    /// The hitbox scene class.
    /// Manages all the hitboxes
    /// </summary>
    public static class HitBoxScene
    {
        private static List<HitBox> hitBoxes = new();

        public static void Add(HitBox hitBox) => hitBoxes.Add(hitBox);

        public static void Remove(HitBox hitBox) => hitBoxes.Remove(hitBox);

        public static void Clear() => hitBoxes.Clear();

        /// <summary>
        /// Gets intersections for hitboxes.
        /// </summary>
        /// <param name="theHitBox">The hitbox to check for intersections</param>
        /// <returns></returns>
        public static List<HitBox> GetIntersections(HitBox theHitBox) => hitBoxes.Where(theHitBox.CheckCollision).ToList();

        public static List<CollisonHitBox> GetCollisions(CollisonHitBox theHitBox) => (from hitBox in hitBoxes where hitBox.GetType() == typeof(CollisonHitBox) where theHitBox.CheckCollision(hitBox) select (CollisonHitBox)hitBox).ToList();

        public static bool Collides(HitBox theHitBox) => hitBoxes.Where(hitBox => hitBox.GetType() == typeof(CollisonHitBox)).Any(theHitBox.CheckCollision);
    }
}
