using System.Collections.Generic;
using osu.Framework.Graphics;
using osuTK;

namespace GentrysQuest.Game.Entity.Weapon
{
    public class AttackPatternEvent(int timeMs = 0)
    {
        /// <summary>
        /// How long until the pattern condition is met.
        /// Ex: The size will size down to 20px in 600ms.
        /// </summary>
        public int TimeMs { get; set; } = timeMs;

        /// <summary>
        /// The direction based on where you're looking.
        /// </summary>
        public float Direction = 0;

        /// <summary>
        /// The Position.
        /// This can be a little tricky depending on the situation.
        /// Keep in mind the direction while editing the value.
        /// Ex case: if the direction is set to 90 degrees,
        /// it would look like the x value got changed.
        /// </summary>
        public Vector2 Position = new Vector2(0);

        /// <summary>
        /// The size of the drawable
        /// </summary>
        public Vector2 Size = new Vector2(1);

        /// <summary>
        /// The size of the hitbox
        /// mostly useful if the texture of the drawable isn't the actual size
        /// </summary>
        public Vector2 HitboxSize = new Vector2(1);

        /// <summary>
        /// Distance away from origin
        /// </summary>
        public float Distance = 0;

        /// <summary>
        /// How much extra damage the object will do
        /// </summary>
        public int DamagePercent = 0;

        /// <summary>
        /// affects how quick the wielder will move.
        /// big for game stabilization and making it feel more enjoyable
        /// </summary>
        public float MovementSpeed = 1;

        /// <summary>
        /// fluency of transition to the next event
        /// </summary>
        public Easing Transition = Easing.None;

        /// <summary>
        /// if this should cause the entity to get hit again
        /// </summary>
        public bool ResetHitBox = false;

        /// <summary>
        /// does this do damage?
        /// </summary>
        public bool DoesDamage = true;

        /// <summary>
        /// What happens when the entity gets hit
        /// </summary>
        public OnHitEffect OnHitEffect = null;

        /// <summary>
        /// The projectiles
        /// </summary>
        public List<ProjectileParameters> Projectiles = null;

        /// <summary>
        /// If this forces the wielder to move in a certain direction.
        /// </summary>
        public bool ForcedMovement = false;

        /// <summary>
        /// The direction the character will move towards.
        /// </summary>
        public int ForcedMovementDirection = 0;

        /// <summary>
        /// How strong the movement is.
        /// </summary>
        public int ForcedMovementStrength = 1;

        /// <summary>
        /// How long the movement will last.
        /// </summary>
        public int ForcedMovementDuration = 0;

        public override string ToString()
        {
            return $"{TimeMs}\n"
                   + $"{Direction}\n"
                   + $"{Position}\n"
                   + $"{Size}\n"
                   + $"{HitboxSize}\n"
                   + $"{Distance}\n"
                   + $"{DamagePercent}";
        }
    }
}
