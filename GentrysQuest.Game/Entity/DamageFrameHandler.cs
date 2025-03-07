using System.Collections.Generic;
using System.Linq;
using GentrysQuest.Game.Entity.Drawables;
using GentrysQuest.Game.Entity.Weapon;

namespace GentrysQuest.Game.Entity
{
    public class DamageFrameHandler
    {
        public DamageFrameHandler(List<HitBox> intersections, DamageQueue queue, Entity sender, DrawableWeapon weapon)
        {
            foreach (var box in intersections.Where(box =>
                         box.GetType() == typeof(CollisonHitBox)
                         || box.GetType() == typeof(MovementHitBox)
                         || box.GetType() == typeof(VisibilityBox)
                         || box.GetType() == typeof(IntersectingHitBox)
                         || box.GetParent().GetType() == typeof(Projectile)
                         || box.GetParent().GetType() == typeof(DrawableWeapon)
                         || queue.Check(box)).ToList())
            {
                intersections.Remove(box);
            }

            foreach (var hitBox in intersections)
            {
                _ = new HitHandler(sender, hitBox.GetParent(), getStatusEffects(weapon.OnHitEffects));
                queue.Add(hitBox);
            }
        }

        /// <summary>
        /// Damage Frame Handler for projectiles
        /// </summary>
        public DamageFrameHandler(List<HitBox> intersections, DamageQueue queue, Entity sender, Projectile projectile)
        {
            foreach (var box in intersections.Where(box =>
                         box.GetType() == typeof(MovementHitBox)
                         || box.GetType() == typeof(VisibilityBox)
                         || box.GetParent().GetType() == typeof(Projectile)
                         || box.GetParent().GetType() == typeof(DrawableWeapon)
                         || box.GetParent().GetType() == typeof(IntersectingHitBox)
                         || queue.Check(box)).ToList())
            {
                intersections.Remove(box);
            }

            foreach (var hitBox in intersections)
            {
                if (hitBox.GetType() == typeof(CollisonHitBox))
                {
                    projectile.Disable();
                    return;
                }

                if (hitBox.GetParent() is not DrawableEntity receiver) continue;

                _ = new HitHandler(sender, receiver, getStatusEffects(projectile.OnHitEffects), projectile.Damage);
                projectile.Hits++;
                queue.Add(hitBox);
            }
        }

        private List<StatusEffect> getStatusEffects(List<OnHitEffect> onHitEffects) =>
            onHitEffects != null ? (from hitEffect in onHitEffects where hitEffect.Applies() select hitEffect.Effect).ToList() : [];
    }
}
