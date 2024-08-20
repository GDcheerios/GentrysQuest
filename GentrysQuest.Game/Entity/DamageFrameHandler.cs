using System.Collections.Generic;
using System.Linq;
using GentrysQuest.Game.Entity.Drawables;

namespace GentrysQuest.Game.Entity
{
    public class DamageFrameHandler
    {
        public DamageFrameHandler(List<HitBox> intersections, DamageQueue queue, Entity sender)
        {
            foreach (var box in intersections.Where(box =>
                         box.GetType() == typeof(CollisonHitBox)
                         || box.GetType() == typeof(MovementHitBox)
                         || box.GetType() == typeof(VisibilityBox)
                         || box.GetParent().GetType() == typeof(Projectile)
                         || box.GetParent().GetType() == typeof(DrawableWeapon)
                         || queue.Check(box)).ToList())
            {
                intersections.Remove(box);
            }

            foreach (var hitBox in intersections)
            {
                _ = new HitHandler(sender, hitBox.GetParent());
                queue.Add(hitBox);
            }
        }
    }
}
