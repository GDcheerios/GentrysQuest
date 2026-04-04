using GentrysQuest.Game.Entity.Drawables;
using GentrysQuest.Game.Location;
using osu.Framework.Graphics;
using osuTK;

namespace GentrysQuest.Game.Entity
{
    public partial class CollisonHitBox : HitBox
    {
        public CollisonHitBox(DrawableEntity parent)
            : base(parent)
        {
            Colour = Colour4.LimeGreen;
            Size = new Vector2(0.3f);
        }

        public CollisonHitBox(MapObject parent)
            : base(parent)
        {
            Colour = Colour4.LimeGreen;
            Size = new Vector2(1);
        }
    }
}
