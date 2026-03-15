using GentrysQuest.Game.Location;
using osu.Framework.Graphics;
using osuTK;

namespace GentrysQuest.Game.Entity
{
    /// <summary>
    /// Map objects need a HitBox to determine if something is inside.
    /// </summary>
    public partial class IntersectingHitBox : HitBox
    {
        public IntersectingHitBox(MapObject parent)
            : base(parent)
        {
            Colour = new Colour4(1, 1, 1, 0.5f);
            Size = Vector2.One;
        }
    }
}
