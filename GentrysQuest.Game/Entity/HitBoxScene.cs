using System.Collections.Generic;
using System.Linq;
using osu.Framework.Graphics.Primitives;

namespace GentrysQuest.Game.Entity
{
    /// <summary>
    /// The hitbox scene class.
    /// Manages all the hitboxes
    /// </summary>
    public static class HitBoxScene
    {
        private static readonly List<HitBox> hitBoxes = new();

        public static void Add(HitBox hitBox)
        {
            if (hitBox == null) return;
            hitBoxes.Add(hitBox);
        }

        public static void Remove(HitBox hitBox) => hitBoxes.Remove(hitBox);

        public static void Clear() => hitBoxes.Clear();

        private static HitBox[] Snapshot() => hitBoxes.ToArray().Where(hitBox => hitBox != null && hitBox.Parent != null).ToArray();

        /// <summary>
        /// Retrieves intersections for hitboxes.
        /// </summary>
        /// <param name="theHitBox">The hitbox to check for intersections</param>
        /// <returns></returns>
        public static List<HitBox> GetIntersections(HitBox theHitBox) =>
            theHitBox == null || theHitBox.Parent == null
                ? []
                : Snapshot().Where(theHitBox.CheckCollision).ToList();

        public static List<CollisonHitBox> GetCollisions(CollisonHitBox theHitBox) =>
            (from hitBox in Snapshot()
             where hitBox.GetType() == typeof(CollisonHitBox)
             where theHitBox.CheckCollision(hitBox)
             select (CollisonHitBox)hitBox).ToList();

        public static bool Collides(HitBox theHitBox) =>
            theHitBox != null
            && theHitBox.Parent != null
            && Snapshot().Where(hitBox => hitBox.GetType() == typeof(CollisonHitBox)).Any(theHitBox.CheckCollision);

        public static bool Collides(RectangleF bounds, AffiliationType affiliation) =>
            Snapshot()
                .Where(hitBox => hitBox.GetType() == typeof(CollisonHitBox))
                .Any(hitBox =>
                    hitBox.Enabled
                    && hitBox.Parent != null
                    && hitBox.Affiliation != affiliation
                    && intersects(bounds, hitBox.ScreenSpaceAabb));

        private static bool intersects(RectangleF first, RectangleF second) =>
            first.Left < second.Right
            && first.Right > second.Left
            && first.Top < second.Bottom
            && first.Bottom > second.Top;
    }
}
