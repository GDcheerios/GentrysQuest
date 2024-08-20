using GentrysQuest.Game.Entity.Drawables;
using osu.Framework.Graphics;
using osuTK;

namespace GentrysQuest.Game.Entity
{
    public partial class VisibilityBox : HitBox
    {
        public VisibilityBox(DrawableEntity parent, bool surrounding = false)
            : base(parent)
        {
            Colour = Colour4.White;
            Size = surrounding ? new Vector2(5) : new Vector2(0.1f);
        }
    }
}
