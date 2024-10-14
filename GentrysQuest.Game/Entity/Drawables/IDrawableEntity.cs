using System.Collections.Generic;
using GentrysQuest.Game.Graphics;
using osu.Framework.Graphics.Sprites;

namespace GentrysQuest.Game.Entity.Drawables
{
    public interface IDrawableEntity
    {
        /// <summary>
        /// Sprite
        /// </summary>
        Sprite Sprite { get; set; }

        /// <summary>
        /// HitBox
        /// </summary>
        HitBox HitBox { get; set; }

        /// <summary>
        /// The affiliation.
        /// Is it an opp?
        /// </summary>
        AffiliationType Affiliation { get; set; }

        /// <summary>
        /// Particles
        /// </summary>
        List<Particle> Particles { get; set; }
    }
}
